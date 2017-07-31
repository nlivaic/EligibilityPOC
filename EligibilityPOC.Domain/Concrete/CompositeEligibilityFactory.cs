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

        /// <summary>
        /// Retrieves eligibility parameters, transforms them into eligibilities, groups into rule sets and builds a Composite structure.
        /// </summary>
        /// <param name="productId">Build an eligibility structure belonging to the specified product.</param>
        /// <returns>Product's Composite eligibility structure.</returns>
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
            // Process the list of IEligibles into a Composite structure, based on rulesets.
            compositeEligibilities = CreateComposite(rawRuleSetEligibilities);
            return compositeEligibilities;
        }

        /// <summary>
        /// Create a rule set based on the eligibilities provided.
        /// </summary>
        /// <typeparam name="T">Rule set eligibility type to create.</typeparam>
        /// <param name="eligibilities">A list of eligibilities to become part of a rule set. 
        ///                             Provide a list of eligibilities that have been filtered upfront for a specific rule set.</param>
        /// <returns>Type T rule set.</returns>
        private T CreateRuleSet<T>(IEnumerable<IEligibility> eligibilities) where T : IEligibility, new() {
            T compositeEligibilities = new T();
            foreach (IEligibility eligibility in eligibilities) {
                compositeEligibilities.AddComponent(eligibility);
            }
            return compositeEligibilities;
        }

        /// <summary>
        /// Goes through provided flat rule set structure and builds a hierarchical composite structure.
        /// </summary>
        /// <param name="rawEligibilities">A flat list of rule set eligibilities.</param>
        /// <returns>A hierarchical composite structure of rule set eligibilities</returns>
        private IEligibility CreateComposite(IList<IEligibility> rawEligibilities) {
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
