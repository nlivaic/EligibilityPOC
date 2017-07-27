using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Entities.EligibilityEntities {
    public class NullEligibility : IEligibility {
        public int RuleSet { get; set; }

        public bool IsEligible() {
            return true;
        }
    }
}
