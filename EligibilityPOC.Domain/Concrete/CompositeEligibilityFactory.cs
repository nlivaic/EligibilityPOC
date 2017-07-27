using EligibilityPOC.Domain.Abstract;
using EligibilityPOC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Concrete {
    public class CompositeEligibilityFactory : IEligibilityFactory {
        private IProductEligibilityParamRepository prodEligRepository;

        public CompositeEligibilityFactory(IProductEligibilityParamRepository prodEligRepo) {
            prodEligRepository = prodEligRepo;
        }

        public IEligibility Create(int productId) {
            IQueryable<ProductEligibilityParam> eligibilityParameters = prodEligRepository.GetProductEligibilityParams(productId);
            IList<IEligibility> eligibilities = MapParamsToEligible(eligibilityParameters.ToList<ProductEligibilityParam>());
            // Process the list of IEligibles into a Composite structure, based on rulesets.
            // ... here ...
            return eligibilities[0];
        }

        /// <summary>
        /// Takes basic list of eligibility parameters and transforms it into a list of eligibilities.
        /// Multiple eligibility parameters might be necessary to create a single eligibility.
        /// </summary>
        /// <param name="eligParams">A list with a basic representation of eligibility parameters.</param>
        /// <returns>A list of eligibilities, with no particular structure.</returns>
        private IList<IEligibility> MapParamsToEligible(IList<ProductEligibilityParam> eligParams) {
            var type = typeof(IEligibility).Assembly.GetTypes().Single(t => t.Name == eligParams[0].EligibilityName);
            IEligibility eligible = (IEligibility)Activator.CreateInstance(type);
            List<IEligibility> eligList = new List<IEligibility>();
            eligList.Add(eligible);
            return eligList;
        }

    }
}
