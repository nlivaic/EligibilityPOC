using EligibilityPOC.Domain.Abstract;
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
        public ActionResult Index()
        {
            return View();
        }
    }
}