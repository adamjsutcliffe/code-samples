using System;
using System.Collections.Generic;
//
//  QLFtueProgress.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLFtueProgress : iEventInterface
    {


        private string Step;

        public QLFtueProgress(string Step)
        {

            this.Step = Step;

        }
        public string name()
        {
            return "ql_ftue_progress";
        }
        public string version()
        {
            return "1-0-0";
        }
        public string snowplowName()
        {
            return "ql_ftue_progress";
        }
        public Dictionary<string, object> snowplowProperties()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                {"Step", this.Step}
            };
            return dictionary;
        }
        public string debugDescription()
        {
            return string.Format("{0}: <Step: {1}>", this, this.Step);
        }

    }
}
