using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Entities {
    public class Product {
        public ProductData ProductData { get; set; }
        public IEligibility Eligibility { get; set; }
    }
}
