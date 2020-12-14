using System;
using System.Collections.Generic;
//
//  QLGameEvent.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLGameEvent : iEventInterface
    {


        private QLGameSourceType GameSource;

        private QLGameType GameType;

        private string GameId;

        private string GameLevel;

        private int TimeTakenToSolve;

        private int StarsEarned;

        private int remainingFilm;

        public QLGameEvent(QLGameSourceType GameSource, QLGameType GameType, string GameId, string GameLevel, int TimeTakenToSolve = 0, int StarsEarned = 0, int remainingFilm = 0)
        {

            this.GameSource = GameSource;

            this.GameType = GameType;

            this.GameId = GameId;

            this.GameLevel = GameLevel;

            this.TimeTakenToSolve = TimeTakenToSolve;

            this.StarsEarned = StarsEarned;

            this.remainingFilm = remainingFilm;

        }
        public string name()
        {
            return "ql_game_event";
        }
        public string version()
        {
            return "1-0-1";
        }
        public string snowplowName()
        {
            return "ql_game_event";
        }
        public Dictionary<string, object> snowplowProperties()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
        {"GameSource", (int)this.GameSource},
        {"GameType", (int)this.GameType},
        {"GameId", this.GameId},
        {"GameLevel", this.GameLevel}
    };
            if (this.TimeTakenToSolve != 0)
            {
                dictionary["TimeTakenToSolve"] = this.TimeTakenToSolve;
            }
            if (this.StarsEarned != 0)
            {
                dictionary["StarsEarned"] = this.StarsEarned;
            }
            if (this.remainingFilm != 0)
            {
                dictionary["remaining_film"] = this.remainingFilm;
            }
            return dictionary;
        }
        public string debugDescription()
        {
            return string.Format("{0}: <GameSource: {1}, GameType: {2}, GameId: {3}, GameLevel: {4}, TimeTakenToSolve: {5}, StarsEarned: {6}, remaining_film: {7}>", this, this.GameSource, this.GameType, this.GameId, this.GameLevel, this.TimeTakenToSolve, this.StarsEarned, this.remainingFilm);
        }

    }
}
