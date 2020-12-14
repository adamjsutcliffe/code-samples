using System;
using System.Collections.Generic;
//
//  QLNotification.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLNotification : iEventInterface
    {
        private string Allowed;

        public QLNotification(string Allowed)
        {

            this.Allowed = Allowed;

        }
        public string name()
        {
            return "ql_notification";
        }
        public string version()
        {
            return "1-0-0";
        }
        public string snowplowName()
        {
            return "ql_notification";
        }
        public Dictionary<string, object> snowplowProperties()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                {"Allowed", this.Allowed}
            };
            return dictionary;
        }
        public string debugDescription()
        {
            return string.Format("{0}: <Allowed: {1}>", this, this.Allowed);
        }

    }
}
