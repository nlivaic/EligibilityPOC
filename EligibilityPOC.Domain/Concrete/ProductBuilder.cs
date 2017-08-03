using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EligibilityPOC.Domain.Entities;

namespace EligibilityPOC.Domain.Concrete {
    /// <summary>
    /// Builds a product. BuildProductData(int) must be called first, in order to initialize product identifier.
    /// </summary>
    public class ProductBuilder : IProductBuilder {
        private IEligibilityFactory _eligibilityFactory;
        private IProductDataRepository _productDataRepository;
        private Product _product;
        // The following two fields are in charge of maintaining product id consistency over the builder life cycle.
        private Nullable<int> _id;
        private int _productId {
            get {
                if (_id == null) {
                    throw new InvalidOperationException("Product id not provided. This exception was most likely thrown because you haven't called BuildProductData() first.");
                }
                return (int)_id;
            }
            set {
                if (_id == null) {
                    _id = value;
                } else {
                    throw new InvalidOperationException("Product id is already provided. This exception was most likely thrown because you have tried to reinitialize the builder. Are you calling BuildProductData() again?");
                }
            }
        }

        public ProductBuilder(IEligibilityFactory eligFactory, IProductDataRepository productDataRepo) {
            _eligibilityFactory = eligFactory;
            _productDataRepository = productDataRepo;
            _product = new Product();
        }

        /// <summary>
        /// Build eligibility for the specified product.
        /// </summary>
        /// <returns>Builder itself.</returns>
        public IProductBuilder BuildEligibility() {
            _product.Eligibility = _eligibilityFactory.Create(_productId);
            return this;
        }

        /// <summary>
        /// Build eligibility for the specified product.
        /// </summary>
        /// <param name="_productId"></param>
        /// <returns>Builder itself.</returns>
        public IProductBuilder BuildProductData(int productId) {
            _productId = productId;
            ProductData productData = _productDataRepository.ProductDatas.Where(p => p.Id == _productId).FirstOrDefault();
            _product.ProductData = productData;
            return this;
        }

        /// <summary>
        /// Returns fully built product, according to previously called build methods.
        /// </summary>
        /// <returns></returns>
        public Product Build() {
            // Prepare the builder for reuse.
            resetProductId();
            // Missing product data means product either isn't constructed properly or it doesn't exist in the repo.
            if (_product.ProductData == null) {
                _product = null;
            }
            return _product;
        }

        /// <summary>
        /// Allows for reuse of the builder. Resets product id's internal representation back to null.
        /// </summary>
        private void resetProductId() {
            _id = null;
        }
    }
}
