using System;
using System.Collections.Generic;
//
//  QLPurchase.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLPurchase : iEventInterface
    {
        private QLPurchaseState PurchaseState;

        private string ProductId;

        private float Price;

        private string Currency;

        private int Reward;

        private QLPurchaseSource Source;

        private string FailReason;

        public QLPurchase(QLPurchaseState PurchaseState, string ProductId, float Price, string Currency, int Reward, QLPurchaseSource Source, string FailReason = null)
        {
            this.PurchaseState = PurchaseState;

            this.ProductId = ProductId;

            this.Price = Price;

            this.Currency = Currency;

            this.Reward = Reward;

            this.Source = Source;

            this.FailReason = FailReason;

        }
        public string name()
        {
            return "ql_purchase";
        }
        public string version()
        {
            return "1-0-0";
        }
        public string snowplowName()
        {
            return "ql_purchase";
        }
        public Dictionary<string, object> snowplowProperties()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                {"PurchaseState", (int)this.PurchaseState},
                {"ProductId", this.ProductId},
                {"Price", this.Price},
                {"Currency", this.Currency},
                {"Reward", this.Reward},
                {"Source", (int)this.Source}
            };
            if (this.FailReason != null)
            {
                dictionary["FailReason"] = this.FailReason;
            }
            return dictionary;
        }
        public string debugDescription()
        {
            return string.Format("{0}: <PurchaseState: {1}, ProductId: {2}, Price: {3}, Currency: {4}, Reward: {5}, Source: {6}, FailReason: {7}>", this, this.PurchaseState, this.ProductId, this.Price, this.Currency, this.Reward, this.Source, this.FailReason);
        }

    }
}
