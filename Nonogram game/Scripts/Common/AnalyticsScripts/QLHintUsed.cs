using System;
using System.Collections.Generic;
//
//  QLHintUsed.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLHintUsed : iEventInterface
    {


        private QLHintType HintType;

        private string GameId;

        private string GameLevel;

        public QLHintUsed(QLHintType HintType, string GameId, string GameLevel)
        {

            this.HintType = HintType;

            this.GameId = GameId;

            this.GameLevel = GameLevel;

        }
        public string name()
        {
            return "ql_hint_used";
        }
        public string version()
        {
            return "1-0-0";
        }
        public string snowplowName()
        {
            return "ql_hint_used";
        }
        public Dictionary<string, object> snowplowProperties()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                {"HintType",(int)this.HintType},
                {"GameId", this.GameId},
                {"GameLevel", this.GameLevel}
            };
            return dictionary;
        }
        public string debugDescription()
        {
            return string.Format("{0}: <HintType: {1}, GameId: {2}, GameLevel: {3}>", this, this.HintType, this.GameId, this.GameLevel);
        }

    }
}
