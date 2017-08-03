using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Abstract {
    public interface IProductDirector {
        void Construct(IProductBuilder builder, int productId);
    }
}
