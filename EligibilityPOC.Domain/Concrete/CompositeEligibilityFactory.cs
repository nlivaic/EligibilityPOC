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
            string eligibilityName = String.Empty;
            Type type;
            IEligibility eligible = null;
            List<IEligibility> eligList = new List<IEligibility>();

            foreach (ProductEligibilityParam param in eligParams.OrderBy(p => p.EligibilityName)) {
                // Multiple parameters might belong to the same eligibility.
                if (eligibilityName != param.EligibilityName) {
                    type = typeof(IEligibility).Assembly.GetTypes().Single(t => t.Name == param.EligibilityName);
                    eligible = (IEligibility)Activator.CreateInstance(type);
                    eligibilityName = param.EligibilityName;
                    eligList.Add(eligible);
                }
                // Set eligibilities property values.
                eligible.GetType().GetProperty(param.ParamName).SetValue(eligible, param.ParamValue);
            }
            return eligList;
        }

    }
}
