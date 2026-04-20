using System;
using System.Configuration;
using System.Xml;

namespace Elipgo.SmartClient.Common.Helpers
{
    static public class SmartClientEnvironmentUtils
    {

        static public ExeConfigurationFileMap GetExeConfigurationFileMap()
        {
            return new ExeConfigurationFileMap
            {
                ExeConfigFilename = System.IO.Path.Combine(GetConfigFolder(), ConfigurationManager.AppSettings["fileConfig"])
            };
        }

        static public Configuration GetConfiguration()
        {
            ExeConfigurationFileMap configMap = GetExeConfigurationFileMap();
            return ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        }

        static public string GetConfigFolder()
        {
            return Environment.ExpandEnvironmentVariables("%APPDATA%") + "\\Elipgo\\Config";
        }

        static public string GetJoystickConfigFolder()
        {
            return Environment.ExpandEnvironmentVariables("%APPDATA%") + "\\Elipgo\\JoystickConfig";
        }

        static public string GetAccessFolder()
        {
            return Environment.ExpandEnvironmentVariables("%APPDATA%") + "\\Elipgo";
        }

        static public string GetWorkFolder()
        {
            return Environment.ExpandEnvironmentVariables("%APPDATA%") + "\\Elipgo";
        }

        static public string GetLogsFolder()
        {
            return Environment.ExpandEnvironmentVariables("%PROGRAMDATA%") + "\\Elipgo\\Logs";
        }

        static public string GetWebView2CacheFolder()
        {
            return Environment.ExpandEnvironmentVariables("%APPDATA%") + "\\Elipgo\\WebCache";
        }

        static public string GetMessageQueueFolder()
        {
            return Environment.ExpandEnvironmentVariables("%APPDATA%") + "\\Elipgo\\MessageQueue";
        }

        static public string GetProfileFolder()
        {
            return Environment.ExpandEnvironmentVariables("%APPDATA%") + "\\Elipgo";
        }

        static public bool SetAttributeConfigFile(string attribute, string value)
        {
            bool result = false;
            try
            {
                XmlDocument xmlDoc = new XmlDocument
                {
                    PreserveWhitespace = true
                };
                string path = GetExeConfigurationFileMap().ExeConfigFilename;
                xmlDoc.Load(path);

                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    if (node.HasChildNodes)
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            if (node.ChildNodes[i].Attributes != null)
                            {
                                if (node.ChildNodes[i].Attributes[0].Value == attribute)
                                {
                                    node.ChildNodes[i].Attributes[1].Value = value;
                                    break;
                                }
                            }
                        }
                    }
                }
                xmlDoc.Save(path);
                result = true;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.ToString(), LogPriority.Important);
            }
            return result;
        }
    }
}
