using EligibilityPOC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Abstract {
    public interface IProductBuilder {
        IProductBuilder BuildProductData();
        IProductBuilder BuildEligibility();
        Product Build();
    }
}
