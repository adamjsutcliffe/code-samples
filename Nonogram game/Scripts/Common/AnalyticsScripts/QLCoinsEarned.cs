using System;
using System.Collections.Generic;
//
//  QLCoinsEarned.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLCoinsEarned : iEventInterface
    {


        private QLCoinSourceType CoinSource;

        private int CoinsAwarded;

        private int PlayerCoins;

        private string GameId;

        private string GameLevel;

        public QLCoinsEarned(QLCoinSourceType CoinSource, int CoinsAwarded, int PlayerCoins, string GameId, string GameLevel)
        {

            this.CoinSource = CoinSource;

            this.CoinsAwarded = CoinsAwarded;

            this.PlayerCoins = PlayerCoins;

            this.GameId = GameId;

            this.GameLevel = GameLevel;

        }
        public string name()
        {
            return "ql_coins_earned";
        }
        public string version()
        {
            return "1-0-0";
        }
        public string snowplowName()
        {
            return "ql_coins_earned";
        }
        public Dictionary<string, object> snowplowProperties()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
        {"CoinSource", (int)this.CoinSource},
        {"CoinsAwarded", this.CoinsAwarded},
        {"PlayerCoins", this.PlayerCoins},
        {"GameId", this.GameId},
        {"GameLevel", this.GameLevel}
    };
            return dictionary;
        }
        public string debugDescription()
        {
            return string.Format("{0}: <CoinSource: {1}, CoinsAwarded: {2}, PlayerCoins: {3}, GameId: {4}, GameLevel: {5}>", this, this.CoinSource, this.CoinsAwarded, this.PlayerCoins, this.GameId, this.GameLevel);
        }

    }
}
