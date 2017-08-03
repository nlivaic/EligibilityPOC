using EligibilityPOC.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Concrete {
    public class EligibilityProductDirector : IProductDirector {
        public EligibilityProductDirector() {

        }

        public void Construct(IProductBuilder builder, int productId) {
            builder.BuildEligibility();
        }
    }
}
