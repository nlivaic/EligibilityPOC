using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EligibilityPOC.Domain.Entities;

namespace EligibilityPOC.Domain.Concrete {
    public class ProductBuilder : IProductBuilder {
        private IEligibilityFactory _eligibilityFactory;
        private IProductDataRepository _productDataRepository;

        public ProductBuilder(IEligibilityFactory eligFactory, IProductDataRepository productDataRepo) {
            _eligibilityFactory = eligFactory;
            _productDataRepository = productDataRepo;
    }

        public Product Create(int productId) {
            ProductData productData = _productDataRepository.ProductDatas.Where(p => p.Id == productId).FirstOrDefault();
            if (productData == null) {
                return null;
            }
            Product product = new Product {
                ProductData = productData,
                Eligibility = _eligibilityFactory.Create(productId)
            };
            return product;
        }
    }
}
