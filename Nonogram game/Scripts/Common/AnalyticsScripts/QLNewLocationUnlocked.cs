using System;
using System.Collections.Generic;
//
//  QLNewLocationUnlocked.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLNewLocationUnlocked : iEventInterface
    {


        private int NewLocationIndex;

        private string GameId;

        private string GameLevel;

        public QLNewLocationUnlocked(int NewLocationIndex, string GameId, string GameLevel)
        {

            this.NewLocationIndex = NewLocationIndex;

            this.GameId = GameId;

            this.GameLevel = GameLevel;

        }
        public string name()
        {
            return "ql_new_location_unlocked";
        }
        public string version()
        {
            return "1-0-0";
        }
        public string snowplowName()
        {
            return "ql_new_location_unlocked";
        }
        public Dictionary<string, object> snowplowProperties()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                {"NewLocationIndex", this.NewLocationIndex},
                {"GameId", this.GameId},
                {"GameLevel", this.GameLevel}
            };
            return dictionary;
        }
        public string debugDescription()
        {
            return string.Format("{0}: <NewLocationIndex: {1}, GameId: {2}, GameLevel: {3}>", this, this.NewLocationIndex, this.GameId, this.GameLevel);
        }

    }
}
