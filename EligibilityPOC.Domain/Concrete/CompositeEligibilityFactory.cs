using EligibilityPOC.Domain.Abstract;
using EligibilityPOC.Domain.Entities;
using EligibilityPOC.Domain.Entities.EligibilityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Concrete {
    public class CompositeEligibilityFactory : IEligibilityFactory {
        private IProductEligibilityParamRepository _prodEligRepository;
        private IProductParamToEligibilityMapper _eligibilityMapper;

        public CompositeEligibilityFactory(IProductEligibilityParamRepository prodEligRepo, IProductParamToEligibilityMapper mapper) {
            _prodEligRepository = prodEligRepo;
            _eligibilityMapper = mapper;
        }

        public IEligibility Create(int productId) {
            IQueryable<ProductEligibilityParam> eligibilityParameters = _prodEligRepository.GetProductEligibilityParams(productId);
            IList<IEligibility> eligibilities = _eligibilityMapper.MapParamsToEligibility(eligibilityParameters.ToList<ProductEligibilityParam>());
            IList<IEligibility> rawRuleSetEligibilities = new List<IEligibility>();
            IEligibility compositeEligibilities = null;
            IEligibility ruleSetEligibility = null;
            int ruleSetCount = eligibilities.Any() ? eligibilities.Max(e => e.RuleSet) : 0;     // .Max() throws if collection is empty.
            for (int i = 1; i <= ruleSetCount; i++) {
                // Rule set 1 is handled differently from the subsequent rule sets.
                if (i == 1) {
                    ruleSetEligibility = CreateRuleSet1(eligibilities.Where(e => e.RuleSet == i));
                    rawRuleSetEligibilities.Add(ruleSetEligibility);
                } else {
                    ruleSetEligibility = CreateRuleSetOther(eligibilities.Where(e => e.RuleSet == i));
                    rawRuleSetEligibilities.Add(ruleSetEligibility);
                }
                ruleSetEligibility.RuleSet = i;
            }
            compositeEligibilities = CreateAllRuleSets(rawRuleSetEligibilities);

            // Rule set 1 is handled differently from the subsequent rule sets.
            //foreach (IEligibility eligibility in eligibilities.Where(e => e.RuleSet == 1)) {
            //    ruleSet = 1;
            //    compositeEligibilities.AddComponent(eligibility);
            //}
            //// Rule sets 2 and on have a dedicated rule set eligibility type.
            //foreach (IEligibility eligibility in eligibilities.Where(e => e.RuleSet > 1)) {
            //    if (ruleSet != eligibility.RuleSet) {
            //        ruleSet = eligibility.RuleSet;
            //        compositeEligibilities.AddComponent(new RuleSetOtherEligibility());
            //    }
            //    compositeEligibilities.AddComponent(eligibility);
            //}
            // Process the list of IEligibles into a Composite structure, based on rulesets.
            // ... here ...
            return compositeEligibilities;
        }

        private IEligibility CreateRuleSet1(IEnumerable<IEligibility> eligibilities) {
            RuleSet1Eligibility compositeEligibilities = new RuleSet1Eligibility();
            foreach (IEligibility eligibility in eligibilities) {
                compositeEligibilities.AddComponent(eligibility);
            }
            return compositeEligibilities;
        }

        private IEligibility CreateRuleSetOther(IEnumerable<IEligibility> eligibilities) {
            RuleSetOtherEligibility compositeEligibilities = new RuleSetOtherEligibility();
            foreach (IEligibility eligibility in eligibilities) {
                compositeEligibilities.AddComponent(eligibility);
            }
            return compositeEligibilities;
        }


        /// <summary>
        /// Builds a hierarchical composite structure of rule set eligibilities from a flat list of rule set eligibilities.
        /// </summary>
        /// <param name="rawEligibilities">A flat list of rule set eligibilities.</param>
        /// <returns></returns>
        private IEligibility CreateAllRuleSets(IList<IEligibility> rawEligibilities) {
            RuleSet1Eligibility compositeEligibilities = null;
            RuleSetOtherEligibility priorEligibility = null;
            IEligibility initialRuleSetOtherEligibility = null;
            if (rawEligibilities.Count == 0) {
                return new NullEligibility();
            }
            // Add rule set 1 explicitly.
            if (rawEligibilities.Count > 0) {
                compositeEligibilities = (RuleSet1Eligibility)rawEligibilities[0];
            }
            // Rule set 2 must be added to rule set 1. 
            // Since this involves handling two different types, it seems wiser to use a dedicated piece of code just for that.
            if (rawEligibilities.Count > 1) {
                initialRuleSetOtherEligibility = rawEligibilities[1];
            }
            // Skip rule sets 1 and 2.
            for (int i = 2; i < rawEligibilities.Count; i++) {
                priorEligibility = (RuleSetOtherEligibility)rawEligibilities[i-1];
                priorEligibility.AddComponent(rawEligibilities[i]);
            }
            // In other wods, we have more than 2 rule sets.
            if (initialRuleSetOtherEligibility != null) {
                compositeEligibilities.AddComponent(initialRuleSetOtherEligibility);
            }
            return (IEligibility)compositeEligibilities;
        }
    }
}
