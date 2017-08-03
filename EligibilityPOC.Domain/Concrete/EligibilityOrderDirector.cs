using EligibilityPOC.Domain.Abstract;
using EligibilityPOC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Concrete {
    public class EligibilityOrderDirector : IOrderProcessor {
        private IProductBuilder _builder;
        private Product _product;

        public EligibilityOrderDirector(IProductBuilder builder) {
            _builder = builder;
        }

        private void Construct(int productId) {
            _product = _builder.BuildProductData(productId).BuildEligibility().Build();
        }

        public void Process(Order order) {
            foreach (int productId in order.Products) {
                Construct(productId);
                _product.Eligibility.IsEligible();
            }
        }
    }
}
