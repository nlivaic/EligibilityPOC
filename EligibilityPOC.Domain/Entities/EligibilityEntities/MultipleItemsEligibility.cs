using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Entities.EligibilityEntities {
    public class MultipleItemsEligibility : IEligibility {
        public string MinCount { get; set; }
        public string MaxCount { get; set; }

        public bool IsEligible() {
            throw new NotImplementedException();
        }
    }
}
