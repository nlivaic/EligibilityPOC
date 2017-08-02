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
        private int _productId;
        private Product _product;

        public ProductBuilder(IEligibilityFactory eligFactory, IProductDataRepository productDataRepo, int productId) {
            _eligibilityFactory = eligFactory;
            _productDataRepository = productDataRepo;
            _productId = productId;
            _product = new Product();
        }

        /// <summary>
        /// Build eligibility for the specified product.
        /// </summary>
        /// <returns>Builder itself.</returns>
        public IProductBuilder BuildEligibility() {
            ProductData productData = _productDataRepository.ProductDatas.Where(p => p.Id == _productId).FirstOrDefault();
            _product.ProductData = productData;
            return this;
        }

        /// <summary>
        /// Build eligibility for the specified product.
        /// </summary>
        /// <param name="_productId"></param>
        /// <returns>Builder itself.</returns>
        public IProductBuilder BuildProductData() {
            _product.Eligibility = _eligibilityFactory.Create(_productId);
            return this;
        }

        /// <summary>
        /// Returns fully built product, according to previously called build methods.
        /// </summary>
        /// <returns></returns>
        public Product Build() {
            // Missing product data means product either isn't constructed properly or it doesn't exist in the repo.
            if (_product.ProductData == null) {
                _product = null;
            }
            return _product;
        }

    }
}
