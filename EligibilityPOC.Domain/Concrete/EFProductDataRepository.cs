using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EligibilityPOC.Domain.Entities;

namespace EligibilityPOC.Domain.Concrete {
    public class EFProductDataRepository : IProductDataRepository {
        private EFDbContext context;

        public EFProductDataRepository() {
            context = new EFDbContext();
        }

        public IQueryable<ProductData> ProductDatas {
            get {
                return context.ProductData;
            }
        }
    }
}
