using Elipgo.SmartClient.Common.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace Elipgo.SmartClient.Common.Helpers
{
    static public class WorkFolderUtils
    {
        private const string FILE_NAME = "WorkFolder.xml";
        private static string _fileConfig = ConfigurationManager.AppSettings["fileConfig"];

        static public string GetUserSettings(string parameterName, bool reviewDefault = false)
        {
            string value = string.Empty;
            string file = Path.Combine(SmartClientEnvironmentUtils.GetWorkFolder(), FILE_NAME);
            if (File.Exists(file))
            {
                var parameterValue = GetParameter(parameterName);
                value = parameterValue.ToString();
            }

            if (string.IsNullOrEmpty(value) && reviewDefault)
            {
                value = GetDefaultValues(parameterName);
            }
            return value;
        }

        static private string GetDefaultValues(string parameterName)
        {
            switch (parameterName)
            {
                case "UserLanguage":
                    return CultureInfo.InstalledUICulture.Name;
                case "DefaultLocation":
                    return Settings.Default["DefaultLocation"].ToString();
                case "takeObj":
                case "VerifyStatus":
                case "BitRate":
                case "TakeAlarmsQuickView":
                case "ShowPanelQuickView":
                    return SmartClientEnvironmentUtils.GetConfiguration().AppSettings.Settings[parameterName].Value;
                default:
                    return string.Empty;
            }
        }

        static public string GetDefaultLocation()
        {
            string defaultLocation = string.Empty;
            if (Settings.Default.Properties.OfType<SettingsProperty>().Any(p => p.Name == "DefaultLocation"))
            {
                defaultLocation = Settings.Default["DefaultLocation"].ToString();
                string file = Path.Combine(SmartClientEnvironmentUtils.GetWorkFolder(), FILE_NAME);
                if (File.Exists(file))
                {
                    var location = GetParameter("DefaultLocation");
                    if (location != string.Empty)
                    {
                        defaultLocation = location;
                    }
                }
            }
            return defaultLocation;
        }

        static private string GetParameter(string parameter)
        {
            try
            {
                var file = Path.Combine(SmartClientEnvironmentUtils.GetWorkFolder(), FILE_NAME);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(file);

                XmlNode paramNode = xmlDoc.DocumentElement.SelectSingleNode($"//parameters[@Config='{_fileConfig}']");
                if (paramNode != null)
                {
                    XmlNode node = paramNode.SelectSingleNode(parameter);
                    return node != null ? node.InnerText : string.Empty;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("private string GetParameter Exception ") + ex.Message, LogPriority.Information);
                return string.Empty;
            }

        }

        static public bool CreateUpdateWorkXml(string parameterName, string parameterValue)
        {
            try
            {
                XmlDocument xmlDoc = GetCustomSettingsFile();
                string filePath = Path.Combine(SmartClientEnvironmentUtils.GetWorkFolder(), FILE_NAME);

                // Find the parameters element or create it if it doesn't exist
                XmlElement paramElement = xmlDoc.SelectSingleNode($"//Elipgo/parameters[@Config='{_fileConfig}']") as XmlElement;
                if (paramElement == null)
                {
                    XmlElement elipgoElement = xmlDoc.SelectSingleNode("/Elipgo") as XmlElement;
                    paramElement = xmlDoc.CreateElement("parameters");
                    paramElement.SetAttribute("Config", _fileConfig);
                    elipgoElement.AppendChild(paramElement);
                }

                // Check if the parameter already exists
                XmlNode existingParameter = paramElement.SelectSingleNode(parameterName);
                if (existingParameter != null)
                {
                    // If it exists, update its value
                    existingParameter.InnerText = parameterValue;
                }
                else
                {
                    // If it doesn't exist, create a new parameter element
                    XmlElement newParameter = xmlDoc.CreateElement(parameterName);
                    newParameter.InnerText = parameterValue;
                    paramElement.AppendChild(newParameter);
                }

                // Save the XML document
                xmlDoc.Save(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get the existing XML Document 
        /// </summary>
        /// <returns></returns>
        static private XmlDocument GetCustomSettingsFile()
        {
            XmlDocument xmlDoc = new XmlDocument();
            string filePath = Path.Combine(SmartClientEnvironmentUtils.GetWorkFolder(), FILE_NAME);

            try
            {
                if (!File.Exists(filePath))
                {
                    CreateXMLDoc(ref xmlDoc, filePath);
                }

                xmlDoc.Load(filePath);
                return xmlDoc;
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("GetCustomSettingsFile Exception ") + ex.Message, LogPriority.Information);
                CreateXMLDoc(ref xmlDoc, filePath);
                xmlDoc.Load(filePath);
                return xmlDoc;
            }

        }

        /// <summary>
        ///  If the file doesn't exist, create a new XML document
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="filePath"></param>
        static private void CreateXMLDoc(ref XmlDocument xmlDoc, string filePath)
        {

            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDeclaration);

            XmlElement elipgoElement = xmlDoc.CreateElement("Elipgo");
            xmlDoc.AppendChild(elipgoElement);

            XmlElement groupsElement = xmlDoc.CreateElement("parameters");
            groupsElement.SetAttribute("Config", _fileConfig);
            elipgoElement.AppendChild(groupsElement);

            xmlDoc.Save(filePath);
        }

        static public Dictionary<string, string> GetDictonaryParams()
        {
            Dictionary<string, string> paramsSettings = new Dictionary<string, string>();
            XmlDocument xmlDoc = GetCustomSettingsFile();
            XmlNode paramNode = xmlDoc.DocumentElement.SelectSingleNode($"//parameters[@Config='{_fileConfig}']");
            if (paramNode != null)
            {
                foreach (XmlNode node in paramNode.ChildNodes)
                {
                    paramsSettings[node.Name] = node.InnerText;
                }
            }

            return paramsSettings;
        }

        /// <summary>
        /// Set many user setting usign a Dictonary
        /// </summary>
        /// <param name="paramsDictonary"></param>
        static public void SetDictonaryParams(Dictionary<string, string> paramsDictonary)
        {
            string filePath = Path.Combine(SmartClientEnvironmentUtils.GetWorkFolder(), FILE_NAME);
            XmlDocument xmlDoc = GetCustomSettingsFile();
            var elipgoElement = xmlDoc.SelectSingleNode("Elipgo");
            XmlElement paramElement = xmlDoc.SelectSingleNode($"//Elipgo/parameters[@Config='{_fileConfig}']") as XmlElement;
            if (paramElement == null)
            {
                paramElement = xmlDoc.CreateElement("parameters");
                paramElement.SetAttribute("Config", _fileConfig);
                elipgoElement.AppendChild(paramElement);
            }

            if (paramElement != null)
            {
                foreach (KeyValuePair<string, string> kvp in paramsDictonary)
                {
                    XmlNode existingParameter = paramElement.SelectSingleNode(kvp.Key);
                    if (existingParameter != null)
                    {
                        // If it exists, update its value
                        existingParameter.InnerText = kvp.Value;
                    }
                    else
                    {
                        // If it doesn't exist, create a new parameter element
                        XmlElement newParameter = xmlDoc.CreateElement(kvp.Key);
                        newParameter.InnerText = kvp.Value;
                        paramElement.AppendChild(newParameter);
                    }
                }
            }
            xmlDoc.Save(filePath);
        }
    }
}
