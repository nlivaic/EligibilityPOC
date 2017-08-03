using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EligibilityPOC.Domain.Entities {
    /// <summary>
    /// Please note this class is not complete. It is here simply to facilitate IProductDirector, so we can tie everything up.
    /// </summary>
    public class Order {
        public IList<int> Products { get; set; }

        public Order(IList<int> products) {
            Products = products;
        }
    }
}
