using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestOrg.Checkout.Models;

namespace TestOrg.Checkout.PromotionEngine
{
    public class PromotionEngineImpl : IPromotionEngineImpl
    {
        private SkuInCart _skuInCart;

        public PromotionEngineImpl(SkuInCart skuInCart)
        {
            _skuInCart = skuInCart;
        }

        public double ApplyPromotion()
        {
            SkuInCart skuInCart = _skuInCart;
            double total = 0;
            var cartList = skuInCart.SkusInCart;
            var skusInCartList = cartList.Select(p => p.SkuId).ToList();

            List<SingleSkuInDB> itemsInDB;
            using (StreamReader r = new StreamReader("itemsInDB.json"))
            {
                string json = r.ReadToEnd();

                itemsInDB = JsonConvert.DeserializeObject<List<SingleSkuInDB>>(json);
            }
            ////  idaeally take data from DB
            SkuInDB skuInDB = new SkuInDB { SkusInDB = itemsInDB };


            List<SinglePromotion> promotionItems;
            using (StreamReader r = new StreamReader("promotions.json"))
            {
                string json = r.ReadToEnd();
                Console.WriteLine("==============================================================");
                Console.WriteLine("Active Promotions are :" + json);
                Console.WriteLine("==============================================================");

                promotionItems = JsonConvert.DeserializeObject<List<SinglePromotion>>(json);
            }
            /////////// ideally take data from DB

            Promotions promotions = new Promotions { Promotion = promotionItems };
            var promotionsList = promotions.Promotion.OrderBy(p => p.PromotionId); //considering n item promotion to be appiled first

            foreach (var promotion in promotionsList)
            {
                if(promotion.PromotionId == 1)
                {
                    PromotionType1 promotionData = JsonConvert.DeserializeObject<PromotionType1>(promotion.PromotionData);

                    var skuId = promotionData.SkuId;
                    var skuQty = cartList.Where(p => p.SkuId == skuId).Select(p => p.SkuQty).FirstOrDefault();

                    

                    if (skusInCartList.Contains(skuId) && skuQty >= promotionData.SkuQty)
                    {
                        var countOfNitemOccurence = skuQty / promotionData.SkuQty;
                        total = total + promotionData.PromotionalPrice* countOfNitemOccurence;
                        var updatedSkuQty = skuQty - promotionData.SkuQty* countOfNitemOccurence;
                        //deduct that qty from list
                        cartList.Where(p => p.SkuId == skuId).ToList().ForEach(p => p.SkuQty = updatedSkuQty);

                    }
                }
                else if(promotion.PromotionId == 2)
                {
                    PromotionType2 promotionData = JsonConvert.DeserializeObject<PromotionType2>(promotion.PromotionData);

                    var skuOneId = promotionData.SkuOneId;
                    var skuOneQty = cartList.Where(p => p.SkuId == skuOneId).Select(p => p.SkuQty).FirstOrDefault();
                    var skuTwoId = promotionData.SkuTwoId;
                    var skuTwoQty = cartList.Where(p => p.SkuId == skuTwoId).Select(p => p.SkuQty).FirstOrDefault();

                    if (skusInCartList.Contains(skuOneId) && skusInCartList.Contains(skuTwoId))
                    {
                        var numberOfCommonOccurence = Math.Min(skuOneQty, skuTwoQty);
                        total = total + promotionData.PromotionalPrice* numberOfCommonOccurence;
                        var updatedSkuOneQty = skuOneQty - numberOfCommonOccurence;
                        var updatedSkuTwoQty = skuTwoQty - numberOfCommonOccurence;
                        //deduct that qty from list
                        cartList.Where(p => p.SkuId == skuOneId).ToList().ForEach(p => p.SkuQty = updatedSkuOneQty);
                        cartList.Where(p => p.SkuId == skuTwoId).ToList().ForEach(p => p.SkuQty = updatedSkuTwoQty);

                    }
                }
                else
                {
                    // promotion of new type is not implemented yet
                    continue;
                }

            }

            ////check if there are any items remaining in cart with positive value
            var remainingItems = cartList.Where(p => p.SkuQty > 0).ToList();

            foreach (var remainingItem in remainingItems)
            {
                //calculate sum for remaining qtys
                var normalPrice = skuInDB.SkusInDB.Where(p => p.SkuId == remainingItem.SkuId).Select(p => p.SkuPrice).FirstOrDefault();
                ///ideally query in DB
                total = total + normalPrice* remainingItem.SkuQty;
                //deduct that qty from list
                 cartList.Where(p => p.SkuId == remainingItem.SkuId).ToList().ForEach(p => p.SkuQty = 0);

            }


            return total; 
        }
    }
}
