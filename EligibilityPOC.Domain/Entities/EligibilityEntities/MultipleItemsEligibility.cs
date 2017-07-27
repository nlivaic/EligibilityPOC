using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Entities.EligibilityEntities {
    public class MultipleItemsEligibility : IEligibility {
        public int MinCount { get; set; }
        public int MaxCount { get; set; }

        public bool IsEligible() {
            throw new NotImplementedException();
        }
    }
}
