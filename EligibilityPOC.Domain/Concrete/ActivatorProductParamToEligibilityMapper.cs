using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EligibilityPOC.Domain.Entities;

namespace EligibilityPOC.Domain.Concrete {
    public class ActivatorProductParamToEligibilityMapper : IProductParamToEligibilityMapper {
        /// <summary>
        /// Takes basic list of eligibility parameters and transforms it into a list of eligibilities.
        /// Multiple eligibility parameters might be necessary to create a single eligibility.
        /// </summary>
        /// <param name="eligParams">A list with a basic representation of eligibility parameters.</param>
        /// <returns>A list of eligibilities, with no particular structure.</returns>
        public IList<IEligibility> MapParamsToEligibility(IList<ProductEligibilityParam> eligParams) {
            string eligibilityName = String.Empty;
            int ruleSet = 0;
            Type type;
            IEligibility eligible = null;
            List<IEligibility> eligList = new List<IEligibility>();

            foreach (ProductEligibilityParam param in eligParams.OrderBy(p => p.RuleSet).ThenBy(p => p.EligibilityName)) {
                // Multiple parameters might belong to the same eligibility.
                if (eligibilityName != param.EligibilityName || ruleSet != param.RuleSet) {
                    type = typeof(IEligibility).Assembly.GetTypes().Single(t => t.Name == param.EligibilityName);
                    eligible = (IEligibility)Activator.CreateInstance(type);
                    eligible.GetType().GetProperty("RuleSet").SetValue(eligible, param.RuleSet);
                    eligibilityName = param.EligibilityName;
                    ruleSet = param.RuleSet;
                    eligList.Add(eligible);
                }
                // Set eligibilities property values.
                eligible.GetType().GetProperty(param.ParamName).SetValue(eligible, param.ParamValue);
            }
            return eligList;
        }
    }
}
