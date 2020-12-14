using System;
using System.Collections.Generic;
//
//  QLContextUser.cs
//
//  Copyright © 2016 Brainbow. All rights reserved.
//

namespace Peak.QuixelLogic.Scripts.Common.AnalyticsScripts
{
    public class QLContextUser : iEventInterface
    {
        private string userId;

        private SHRDeviceAudioPortType audioPort;

        private float batteryLevel;

        private string appVersion;

        private string deviceId;

        private string osVersion;

        private string buildType;

        public QLContextUser(string userId, SHRDeviceAudioPortType audioPort, float batteryLevel, string appVersion, string deviceId, string osVersion, string buildType)
        {

            this.userId = userId;

            this.audioPort = audioPort;

            this.batteryLevel = batteryLevel;

            this.appVersion = appVersion;

            this.deviceId = deviceId;

            this.osVersion = osVersion;

            this.buildType = buildType;

        }
        public string name()
        {
            return "ql_user_context";
        }
        public string version()
        {
            return "1-0-0";
        }
        public string snowplowName()
        {
            return "ql_user_context";
        }
        public Dictionary<string, object> snowplowProperties()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object> {
                {"user_id", this.userId},
                {"audio_port", (int)this.audioPort},
                {"battery_level", this.batteryLevel},
                {"app_version", this.appVersion},
                {"device_id", this.deviceId},
                {"os_version", this.osVersion},
                {"build_type", this.buildType}
            };
            return dictionary;
        }
        public string debugDescription()
        {
            return string.Format("{0}: <user_id: {1}, audio_port: {2}, battery_level: {3}, app_version: {4}, device_id: {5}, os_version: {6}, build_type: {7}>", this, this.userId, this.audioPort, this.batteryLevel, this.appVersion, this.deviceId, this.osVersion, this.buildType);
        }
    }
}
