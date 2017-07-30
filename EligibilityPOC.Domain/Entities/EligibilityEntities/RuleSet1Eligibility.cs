using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Entities.EligibilityEntities {
    /// <summary>
    /// Composite IEligibility.
    /// </summary>
    public class RuleSet1Eligibility : IEligibility {
        public int RuleSet { get; set; }
        public IList<IEligibility> Components { get; set; }

        public RuleSet1Eligibility() {
            Components = new List<IEligibility>();
        }

        public bool IsEligible() {
            throw new NotImplementedException();
        }

        public void AddComponent(IEligibility eligibility) {
            Components.Add(eligibility);
        }
    }
}
