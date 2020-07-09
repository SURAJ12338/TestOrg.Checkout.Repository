using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using TestOrg.Checkout.Models;
using TestOrg.Checkout.PromotionEngine;

namespace TestOrg.Checkout
{
    class Program
    {
       

        public static void Main(string[] args)
        {
             List<SingleSkuInCart> itemsInCart;

            using (StreamReader r = new StreamReader("itemsInCart.json"))
            {
                string json = r.ReadToEnd();
                Console.WriteLine("==============================================================");
                Console.WriteLine("Items in Cart are :" + json);
                Console.WriteLine("==============================================================");
                itemsInCart = JsonConvert.DeserializeObject<List<SingleSkuInCart>>(json);
            }
            ///ideally get data from request
            SkuInCart skuInCart = new SkuInCart { SkusInCart = itemsInCart };

            // interface implementation to keep extension open... injecting PromotionEngineImpl here manually
            IPromotionEngineImpl ipromotionEngineImpl = new PromotionEngineImpl(skuInCart);


            double total = ipromotionEngineImpl.ApplyPromotion();
            Console.WriteLine("==============================================================");
            Console.WriteLine("Total is :"+total.ToString());
            Console.WriteLine("==============================================================");





        }

    }
}
