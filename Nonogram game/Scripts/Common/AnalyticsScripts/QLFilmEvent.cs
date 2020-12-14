using System;
using System.Collections.Generic;
//
//  QLFilmEvent.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLFilmEvent : iEventInterface
    {
        
        
        private QLFilmSourceType filmSource;
        
        private int playerFilm;
        
        public  QLFilmEvent(QLFilmSourceType filmSource, int playerFilm) 
        {
            
		this.filmSource = filmSource;

		this.playerFilm = playerFilm;

        }
        public string name() 
        {
            return "ql_film_event";
        }
        public string version() 
        {
            return "1-0-0";
        }
        public string snowplowName() 
        {
            return "ql_film_event";
        }
        public Dictionary<string,object> snowplowProperties() 
        {
            Dictionary<string,object> dictionary = new Dictionary<string,object> {
		{"film_source", (int)this.filmSource},
		{"player_film", this.playerFilm}
	};
	return dictionary;
        }
        public string debugDescription() 
        {
            return string.Format("{0}: <film_source: {1}, player_film: {2}>", this, this.filmSource, this.playerFilm);
        }
    
    }
}
