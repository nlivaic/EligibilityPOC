using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EligibilityPOC.Domain.Entities;

namespace EligibilityPOC.Domain.Concrete {
    public class ProductBuilder : IProductBuilder {
        public Product Create(int productId) {
            throw new NotImplementedException();
        }
    }
}
