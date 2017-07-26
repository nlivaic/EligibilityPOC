using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EligibilityPOC.Domain.Entities;

namespace EligibilityPOC.Domain.Concrete {
    public class EFProductEligibilityParamRepository : IProductEligibilityParamRepository {
        private EFDbContext context = new EFDbContext();

        public IQueryable<ProductEligibilityParam> GetProductEligibilityParams(int productId) {
            return context.ProductEligibilityParam.Where(p => p.ProductId == productId);
        }
    }
}
