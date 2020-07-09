using System;
using System.Collections.Generic;

namespace TestOrg.Checkout.Models
{
    public class SkuInDB
    {
        public List<SingleSkuInDB> SkusInDB { get; set;}
    }

    public class SingleSkuInDB
    {
        public string SkuId { get; set; }
        public double SkuPrice { get; set; }


    }

    public class SkuInCart
    {
        public List<SingleSkuInCart> SkusInDB { get; set; }
    }


    public class SingleSkuInCart
    {
        public string SkuId { get; set; }
        public int SkuQty { get; set; }
    }

    public class Promotions
        {
            public int PromotionId { get; set; }
            public string PromotionData { get; set; }

        }
}
