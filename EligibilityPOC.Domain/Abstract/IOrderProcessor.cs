using EligibilityPOC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Abstract {
    /// <summary>
    /// Please note this interface is here simply to tie everything up.
    /// </summary>
    public interface IOrderProcessor {
        void Process(Order order);
    }
}
