using System;
using System.Collections.Generic;
//
//  QLNewUser.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public interface iEventInterface
    {
        string name();
        string version();
        string snowplowName();
        Dictionary<string, object> snowplowProperties();
        string debugDescription();
    }
}
