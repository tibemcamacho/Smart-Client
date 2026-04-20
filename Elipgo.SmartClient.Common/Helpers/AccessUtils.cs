using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;

namespace Elipgo.SmartClient.Common.Helpers
{
    public static class AccessUtils
    {
        private const string FILE_NAME = "Access.xml";

        public static Dictionary<string, int> GetDefaultAccess(string value)
        {
            var parameter = new Dictionary<string, int>();
            string file = Path.Combine(SmartClientEnvironmentUtils.GetAccessFolder(), FILE_NAME);
            if (File.Exists(file))
            {
                parameter = GetParameter("config", value);
            }
            return parameter;
        }

        private static Dictionary<string, int> GetParameter(string findParameter, string findValue)
        {
            var result = new Dictionary<string, int>();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Path.Combine(SmartClientEnvironmentUtils.GetAccessFolder(), FILE_NAME));

                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    if (node.HasChildNodes)
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            if (node.ChildNodes[i].Name == findParameter)
                            {
                                var value = node.ChildNodes[i].InnerText;
                                if (value == findValue)
                                {
                                    var id = int.Parse(node.ChildNodes[i].Attributes["Id"].Value);
                                    result.Add(value, id);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return result;
        }

        public static void KillAccess()
        {
            try
            {
                var file = Path.Combine(SmartClientEnvironmentUtils.GetAccessFolder(), FILE_NAME);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(file);

                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    if (node.HasChildNodes)
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            if (node.ChildNodes[i].Name == "config")
                            {
                                node.RemoveChild(node.ChildNodes[i]);
                            }
                        }
                    }
                }
                xmlDoc.Save(file);
            }
            catch (System.Exception)
            {
            }
        }

        public static void Kill()
        {
            try
            {
                var file = Path.Combine(SmartClientEnvironmentUtils.GetAccessFolder(), FILE_NAME);
                var config = ConfigurationManager.AppSettings["fileConfig"];
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(file);

                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    if (node.HasChildNodes)
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            if (node.ChildNodes[i].Name == "config")
                            {
                                var value = node.ChildNodes[i].InnerText;
                                if (config == value)
                                {
                                    node.RemoveChild(node.ChildNodes[i]);
                                }
                            }
                        }
                    }
                }
                xmlDoc.Save(file);
            }
            catch (System.Exception)
            {
            }
        }

        public static void CreateUpdateAccessXml(string defaultLocation)
        {
            try
            {
                var file = Path.Combine(SmartClientEnvironmentUtils.GetAccessFolder(), FILE_NAME);
                var current = Process.GetCurrentProcess();

                if (File.Exists(file))
                {
                    XmlDocument xmlDoc = new XmlDocument
                    {
                        PreserveWhitespace = true
                    };
                    xmlDoc.Load(file);

                    var exist = false;
                    foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                    {
                        if (node.HasChildNodes)
                        {
                            foreach (XmlNode child in node.ChildNodes)
                            {
                                if (child.Name.Equals("config"))
                                {
                                    if (child.InnerText == defaultLocation)
                                    {
                                        exist = true;
                                        break;
                                    }
                                }
                            }

                            if (!exist)
                            {
                                //Create a new node.
                                XmlElement elem = xmlDoc.CreateElement(string.Empty, "config", string.Empty);
                                elem.InnerText = defaultLocation;
                                elem.SetAttribute("Id", current.Id.ToString());
                                XmlNode co = xmlDoc.SelectSingleNode("Elipgo/Access");
                                co.AppendChild(elem);
                            }
                        }
                    }
                    xmlDoc.Save(file);
                }
                else
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);

                    XmlElement root = xmlDoc.DocumentElement;
                    xmlDoc.InsertBefore(xmlDeclaration, root);

                    XmlElement element1 = xmlDoc.CreateElement(string.Empty, "Elipgo", string.Empty);
                    xmlDoc.AppendChild(element1);

                    XmlElement element2 = xmlDoc.CreateElement(string.Empty, "Access", string.Empty);
                    element1.AppendChild(element2);

                    XmlElement element3 = xmlDoc.CreateElement(string.Empty, "config", string.Empty);
                    XmlText text1 = xmlDoc.CreateTextNode(defaultLocation);
                    element3.SetAttribute("Id", current.Id.ToString());
                    element3.AppendChild(text1);
                    element2.AppendChild(element3);

                    if (!Directory.Exists(SmartClientEnvironmentUtils.GetAccessFolder()))
                    {
                        Directory.CreateDirectory(SmartClientEnvironmentUtils.GetAccessFolder());
                    }

                    xmlDoc.Save(file);
                }
            }
            catch (System.Exception)
            {
            }
        }

        public static void CreateImagesFolder(string pathApplication)
        {
            if (!Directory.Exists(pathApplication))
            {
                Directory.CreateDirectory(pathApplication);
            }

            string[] filePaths = Directory.GetFiles(pathApplication);
            if (filePaths.Length > 0)
            {
                if (File.Exists(filePaths.Where(x => x.Contains("fondo")).FirstOrDefault())) File.Delete(filePaths.Where(x => x.Contains("fondo")).FirstOrDefault());
                if (File.Exists(filePaths.Where(x => x.Contains("logo")).FirstOrDefault())) File.Delete(filePaths.Where(x => x.Contains("logo")).FirstOrDefault());

            }
        }

    }
}
