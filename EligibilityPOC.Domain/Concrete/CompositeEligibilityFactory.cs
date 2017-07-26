using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Concrete {
    class CompositeEligibilityFactory : IEligibilityFactory {
        private IProductEligibilityParamRepository prodEligRepository;

        public CompositeEligibilityFactory(IProductEligibilityParamRepository prodEligRepo) {
            prodEligRepository = prodEligRepo;
        }

        public IEligible Create(int productId) {
            throw new NotImplementedException();
        }
    }
}
