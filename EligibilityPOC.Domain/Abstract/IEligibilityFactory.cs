﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Abstract {
    public interface IEligibilityFactory {
        IEligibility Create(int productId);
    }
}
