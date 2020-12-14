using System;
using System.Collections.Generic;
//
//  QLAdEvent.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLAdEvent : iEventInterface
    {
        private QLAdResultType AdResult;

        private QLAdType AdType;

        private QLAdSourceType AdSource;

        public QLAdEvent(QLAdResultType AdResult, QLAdType AdType, QLAdSourceType AdSource)
        {

            this.AdResult = AdResult;

            this.AdType = AdType;

            this.AdSource = AdSource;

        }
        public string name()
        {
            return "ql_ad_event";
        }
        public string version()
        {
            return "1-0-0";
        }
        public string snowplowName()
        {
            return "ql_ad_event";
        }
        public Dictionary<string, object> snowplowProperties()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                {"AdResult", (int)this.AdResult},
                {"AdType", (int)this.AdType},
                {"AdSource", (int)this.AdSource}
            };
            return dictionary;
        }
        public string debugDescription()
        {
            return string.Format("{0}: <AdResult: {1}, AdType: {2}, AdSource: {3}>", this, this.AdResult, this.AdType, this.AdSource);
        }

    }
}
