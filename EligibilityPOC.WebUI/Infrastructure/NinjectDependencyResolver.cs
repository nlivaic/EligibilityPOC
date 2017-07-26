using EligibilityPOC.Domain.Abstract;
using EligibilityPOC.Domain.Concrete;
using EligibilityPOC.Domain.Entities;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EligibilityPOC.WebUI.Infrastructure {
    public class NinjectDependencyResolver : IDependencyResolver {
        private IKernel kernel;

        public NinjectDependencyResolver() {
            kernel = new StandardKernel();
            AddBindings();
        }


        public object GetService(Type serviceType) {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings() {
            Mock<IProductDataRepository> mockProductDataRepo = new Mock<IProductDataRepository>();
            mockProductDataRepo.Setup(m => m.ProductDatas).Returns(new ProductData[] {
                new ProductData { Id = 1, Name = "Podjetni M", Type = 1 },
                new ProductData { Id = 2, Name = "Podjetni L", Type = 1 },
                new ProductData { Id = 3, Name = "Speed Service 20/1", Type = 2 },
                new ProductData { Id = 4, Name = "Speed Service 40/1", Type = 2 }
            }.AsQueryable<ProductData>());
            Mock<IProductEligibilityParamRepository> mockProductEligParamRepo = new Mock<IProductEligibilityParamRepository>();
            mockProductEligParamRepo.Setup(m => m.GetProductEligibilityParams(1)).Returns(
                new ProductEligibilityParam[] {
                    new ProductEligibilityParam {
                        Id = 1,
                        ProductId = 1,
                        EligibilityName = "FormSubtype",
                        ParamName = "ValidSubtypes",
                        ParamValue = "1,5,7",
                        RuleSet = 1
                    },
                    new ProductEligibilityParam {
                        Id = 2,
                        ProductId = 1,
                        EligibilityName = "SubscriberType",
                        ParamName = "SubscriberType",
                        ParamValue = "1,2,3",
                        RuleSet = 1
                    },
                    new ProductEligibilityParam {
                        Id = 3,
                        ProductId = 1,
                        EligibilityName = "FormSubtype",
                        ParamName = "ValidSubtypes",
                        ParamValue = "7",
                        RuleSet = 2
                    }//,
                    //new ProductEligibilityParam {
                    //    Id = 4,
                    //    ProductId = 2,
                    //    EligibilityName = "FormSubtype",
                    //    ParamName = "ValidSubtypes",
                    //    ParamValue = "5",
                    //    RuleSet = 1
                    //}
                }.AsQueryable<ProductEligibilityParam>());

            kernel.Bind<IProductDataRepository>().To<EFProductDataRepository>();
            kernel.Bind<IProductEligibilityParamRepository>().To<EFProductEligibilityParamRepository>();
        }
    }
}