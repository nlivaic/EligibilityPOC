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
                    ruleSetEligibility = CreateRuleSet<RuleSet1Eligibility>(eligibilities.Where(e => e.RuleSet == i));
                    rawRuleSetEligibilities.Add(ruleSetEligibility);
                } else {
                    ruleSetEligibility = CreateRuleSet<RuleSetOtherEligibility>(eligibilities.Where(e => e.RuleSet == i));
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

        private T CreateRuleSet<T>(IEnumerable<IEligibility> eligibilities) where T : IEligibility, new() {
            T compositeEligibilities = new T();
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
            IEligibility compositeEligibilities = null;
            if (rawEligibilities.Count == 0) {
                return new NullEligibility();
            }
            // Rule set 1 becomes the composite root by default.
            compositeEligibilities = rawEligibilities[0];
            // Skip the last eligibility as it doesn't contain any new Composites.
            for (int i = 1; i < rawEligibilities.Count; i++) {
                compositeEligibilities.AddComponent(rawEligibilities[i]);
                compositeEligibilities = rawEligibilities[i];
            }
            // Return composite root.
            return rawEligibilities[0];
        }
    }
}
