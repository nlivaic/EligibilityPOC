using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Entities {
    public class ProductEligibilityParam {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string EligibilityName { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
        public int RuleSet { get; set; }
    }
}
