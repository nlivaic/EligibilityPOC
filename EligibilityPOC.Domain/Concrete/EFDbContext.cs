using EligibilityPOC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Concrete {
    class EFDbContext : DbContext {
        public DbSet<ProductData> ProductData { get; set; }
        public DbSet<ProductEligibilityParam> ProductEligibilityParam { get; set; }
    }
}
