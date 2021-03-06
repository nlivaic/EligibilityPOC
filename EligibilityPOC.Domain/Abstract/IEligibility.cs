﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Abstract {
    public interface IEligibility {
        int RuleSet { get; set; }

        bool IsEligible();

        void AddComponent(IEligibility eligibility);
        }
}
