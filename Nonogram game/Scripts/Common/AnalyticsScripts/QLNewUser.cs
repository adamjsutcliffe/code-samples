using System;
using System.Collections.Generic;
//
//  QLNewUser.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLNewUser : iEventInterface
    {
        public QLNewUser()
        {

        }
        public string name()
        {
            return "ql_new_user";
        }
        public string version()
        {
            return "1-0-0";
        }
        public string snowplowName()
        {
            return "ql_new_user";
        }
        public Dictionary<string, object> snowplowProperties()
        {
            return new Dictionary<string, object> { };
        }
        public string debugDescription()
        {
            return string.Format("{0}: <>", this);
        }

    }
}
