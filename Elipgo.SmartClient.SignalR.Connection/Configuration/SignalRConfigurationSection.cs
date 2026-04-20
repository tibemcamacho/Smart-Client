using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Configuration;

namespace Elipgo.SmartClient.SignalR.Connection.Configuration
{
    public class SignalRConfigurationSection : ConfigurationSection
    {
        public static SignalRConfigurationSection Settings { get => settings().Value; }

        /// <summary>
        /// Url del sevidor de SignalR
        /// </summary>
        [ConfigurationProperty("url_server", IsRequired = true)]
        public string UrlServer
        {
            get { return this["url_server"].ToString(); }
            set { this["url_server"] = value; }
        }

        [ConfigurationProperty("hub", IsRequired = true)]
        public string Hub
        {
            get { return this["hub"].ToString(); }
            set { this["hub"] = value; }
        }

        [ConfigurationProperty("version", IsRequired = true)]
        public string Version
        {
            get { return this["version"].ToString(); }
            set { this["version"] = value; }
        }

        private static Lazy<SignalRConfigurationSection> settings()
        {
            var config = SmartClientEnvironmentUtils.GetConfiguration();
            var VMON_URL = config.AppSettings.Settings["VMON5_URL"].Value + "/vmon";

            return new Lazy<SignalRConfigurationSection>(() => (
           new SignalRConfigurationSection()
           {
               UrlServer = VMON_URL,
               Hub = "connection"
           }));
        }
    }
}
