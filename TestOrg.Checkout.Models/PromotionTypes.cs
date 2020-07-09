using System;
using System.Collections.Generic;
using System.Text;

namespace TestOrg.Checkout.Models
{
    public class PromotionType1
    {
        public string SkuId { get; set; }
        public int SkuQty { get; set; }
        public double PromotionalPrice { get; set; }
    }

    public class PromotionType2
    {
        public string SkuOneId { get; set; }
        public string SkuTwoId { get; set; }
        public double PromotionalPrice { get; set; }
    }
}
