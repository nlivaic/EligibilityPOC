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

            public EFDbContext() {
                // This is a hack to ensure that Entity Framework SQL Provider is copied across to the output folder.
                // As it is installed in the GAC, Copy Local does not work. It is required for probing.
                // Fixed "Provider not loaded" error.
                var ensureDLLIsCopied =
                    System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            }
    }
}
