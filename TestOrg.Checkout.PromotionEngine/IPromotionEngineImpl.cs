using System;
using System.Collections.Generic;
using System.Text;
using TestOrg.Checkout.Models;

namespace TestOrg.Checkout.PromotionEngine
{
    public interface IPromotionEngineImpl
    {
        public double ApplyPromotion();
    }
}
