using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Entities {
    class Product {
        public ProductData ProductData { get; set; }
        public IEligible Eligibility { get; set; }
    }
}
