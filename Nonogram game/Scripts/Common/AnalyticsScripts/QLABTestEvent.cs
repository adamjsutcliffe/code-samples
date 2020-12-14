using System;
using System.Collections.Generic;
//
//  QLABTestEvent.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLABTestEvent : iEventInterface
    {
        
        
        private QLABTestState abTestState;
        
        private string abTestTreatmentKey;
        
        private string abTestExperimentName;
        
        public  QLABTestEvent(QLABTestState abTestState, string abTestTreatmentKey, string abTestExperimentName) 
        {
            
		this.abTestState = abTestState;

		this.abTestTreatmentKey = abTestTreatmentKey;

		this.abTestExperimentName = abTestExperimentName;

        }
        public string name() 
        {
            return "ql_ab_test_event";
        }
        public string version() 
        {
            return "1-0-0";
        }
        public string snowplowName() 
        {
            return "ql_ab_test_event";
        }
        public Dictionary<string,object> snowplowProperties() 
        {
            Dictionary<string,object> dictionary = new Dictionary<string,object> {
		{"ab_test_state", (int)this.abTestState},
		{"ab_test_treatment_key", this.abTestTreatmentKey},
		{"ab_test_experiment_name", this.abTestExperimentName}
	};
	return dictionary;
        }
        public string debugDescription() 
        {
            return string.Format("{0}: <ab_test_state: {1}, ab_test_treatment_key: {2}, ab_test_experiment_name: {3}>", this, this.abTestState, this.abTestTreatmentKey, this.abTestExperimentName);
        }
    
    }
}
