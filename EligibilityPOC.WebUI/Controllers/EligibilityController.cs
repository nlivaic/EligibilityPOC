using EligibilityPOC.Domain.Abstract;
using EligibilityPOC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EligibilityPOC.WebUI.Controllers
{
    public class EligibilityController : Controller
    {
        private IProductDataRepository productDataRepository;
        private IProductEligibilityParamRepository productEligibilityParamRepository;

        public EligibilityController(IProductDataRepository productDataRepo, IProductEligibilityParamRepository productEligibilityParamRepo) {
            productDataRepository = productDataRepo;
            productEligibilityParamRepository = productEligibilityParamRepo;
        }

        // GET: Eligibility
        public ActionResult RawProducts()
        {
            ProductViewModel productVM = new ProductViewModel();
            productVM.ProductData = productDataRepository.ProductDatas.Where(p => p.Id == 1).FirstOrDefault();
            productVM.ProductEligibilityParams = productEligibilityParamRepository.GetProductEligibilityParams(1);
            return View(productVM);
        }
    }
    public class ProductViewModel {
        public ProductData ProductData { get; set; }
        public IQueryable<ProductEligibilityParam> ProductEligibilityParams { get; set; }
    }
}