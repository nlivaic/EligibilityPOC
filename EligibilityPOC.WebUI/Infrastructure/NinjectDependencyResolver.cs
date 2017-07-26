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
            kernel.Bind<IProductDataRepository>().To<EFProductDataRepository>();
            kernel.Bind<IProductEligibilityParamRepository>().To<EFProductEligibilityParamRepository>();
        }
    }
}