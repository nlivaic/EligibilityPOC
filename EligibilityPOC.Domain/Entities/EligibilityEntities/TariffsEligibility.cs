using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Entities.EligibilityEntities {
    public class TariffsEligibility : IEligibility {
        public int RuleSet { get; set; }
        public string AllowedTariffs { get; set; }

        public bool IsEligible() {
            throw new NotImplementedException();
        }

        public void AddComponent(IEligibility eligibility) {
            throw new NotImplementedException();
        }
    }
}
