using System;
using System.Collections.Generic;
//
//  QLStoreOpen.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLStoreOpen : iEventInterface
    {
        
        
        private QLStoreOpenSource source;
        
        public  QLStoreOpen(QLStoreOpenSource source) 
        {
            
		this.source = source;

        }
        public string name() 
        {
            return "ql_store_open";
        }
        public string version() 
        {
            return "1-0-0";
        }
        public string snowplowName() 
        {
            return "ql_store_open";
        }
        public Dictionary<string,object> snowplowProperties() 
        {
            Dictionary<string,object> dictionary = new Dictionary<string,object> {
		{"source", (int)this.source}
	};
	return dictionary;
        }
        public string debugDescription() 
        {
            return string.Format("{0}: <source: {1}>", this, this.source);
        }
    
    }
}
