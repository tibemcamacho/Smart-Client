using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Elipgo.SmartClient.Drivers
{

    public class JoystickConfiguration
    {
        private const string JOYSTICK_SETTINGS_XML_FILENAME = "JoystickSettings.xml";
        private const string JOYSTICK_SETTINGS_XSD_FILENAME = "JoystickSettings.xsd";

        private const string MAIN_TABLE_NAME = "Settings";
        private const string CHILD_TABLE_NAME = "Settings_Sensors";

        public const int MAX_PRESETS = 20;
        public const int MAX_GUARDS = 20;

        /// <summary>
        /// Propiedades generales del joystick. Por ejemplo, inversion de los ejes,
        /// deadzone, etc.
        /// </summary>
        private Hashtable settings;

        /// <summary>
        /// Tabla con los mapeos de los botones del joystick.
        /// Cada entrada es de la forma [ButtonOrAxis, ActionCommand]
        /// </summary>
        private Hashtable _commandMappings;
        private string _pathDirectory;

        public JoystickConfiguration()
        {
            settings = new Hashtable();
            _pathDirectory = SmartClientEnvironmentUtils.GetJoystickConfigFolder();
            if (!Directory.Exists(_pathDirectory))
            {
                Directory.CreateDirectory(_pathDirectory);
            }
            CommandMappings = new Hashtable();
        }

        public List<string> GetCustomConfig()
        {
            var l = Directory.GetFiles(this._pathDirectory, "*.xml", SearchOption.AllDirectories).ToList().Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
            return l;
        }

        public void Load(string name = "")
        {
            CommandMappings.Clear();
            string filepath = "";
            if (name == "" && !string.IsNullOrEmpty(Properties.Settings.Default.CurrentJoystickConf))
            {
                filepath = Path.Combine(this._pathDirectory, Properties.Settings.Default.CurrentJoystickConf + ".xml");
            }
            else if (name.ToUpper() != "NUEVO" && !string.IsNullOrEmpty(name))
            {
                filepath = Path.Combine(this._pathDirectory, name + ".xml");
            }
            else
            {
                filepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), "Joystick", JOYSTICK_SETTINGS_XML_FILENAME);
            }

            DataSet dataSet = new DataSet();
            dataSet.ReadXmlSchema(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), "Joystick", JOYSTICK_SETTINGS_XSD_FILENAME));
            dataSet.ReadXml(filepath);

            DataTable joystickSettingsTable = dataSet.Tables[MAIN_TABLE_NAME];
            DataRowCollection rows = joystickSettingsTable.Rows;

            if (rows.Count > 0)
            {
                #region Global Joystick Settings
                object invertXAxis = rows[0][GlobalJoystickSetting.InvertXAxis.ToString()];
                object invertYAxis = rows[0][GlobalJoystickSetting.InvertYAxis.ToString()];
                object invertZAxis = rows[0][GlobalJoystickSetting.InvertZAxis.ToString()];
                object invertXAxisRotation = rows[0][GlobalJoystickSetting.InvertXAxisRotation.ToString()];
                object invertYAxisRotation = rows[0][GlobalJoystickSetting.InvertYAxisRotation.ToString()];
                object invertZAxisRotation = rows[0][GlobalJoystickSetting.InvertZAxisRotation.ToString()];

                settings[GlobalJoystickSetting.InvertXAxis] = invertXAxis != null ? invertXAxis : "false";
                settings[GlobalJoystickSetting.InvertYAxis] = invertYAxis != null ? invertYAxis : "false";
                settings[GlobalJoystickSetting.InvertZAxis] = invertZAxis != null ? invertZAxis : "false";
                settings[GlobalJoystickSetting.InvertXAxisRotation] = invertXAxisRotation != null ? invertXAxisRotation : "false";
                settings[GlobalJoystickSetting.InvertYAxisRotation] = invertYAxisRotation != null ? invertYAxisRotation : "false";
                settings[GlobalJoystickSetting.InvertZAxisRotation] = invertZAxisRotation != null ? invertZAxisRotation : "false";
                #endregion

                DataRow[] settingsDataRow = rows[0].GetChildRows(CHILD_TABLE_NAME);

                foreach (DataRow row in settingsDataRow)
                {
                    ButtonOrAxis buttonOrAxis;
                    string buttonOrAxisName = row.ItemArray[0].ToString();
                    string commandName = row.ItemArray[1].ToString();
                    string parameter = row.ItemArray[2].ToString() != "" ? row.ItemArray[2].ToString() : "1";
                    try
                    {
                        ActionCommand actionCommand = new ActionCommand();
                        actionCommand.command = (PtzCommand)Enum.Parse(typeof(PtzCommand), commandName, true);
                        actionCommand.Parameter = double.Parse(parameter);

                        buttonOrAxis = (ButtonOrAxis)Enum.Parse(typeof(ButtonOrAxis), buttonOrAxisName, true);
                        actionCommand.buttonOrAxis = buttonOrAxis;

                        CommandMappings[buttonOrAxis] = actionCommand;
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        public void Save(string name)
        {
            try
            {
                string filename = name;
                string filepath = Path.Combine(this._pathDirectory, filename + ".xml");
                string xmlFilePath = filepath;
                xmlFilePath = new Uri(xmlFilePath).AbsolutePath.Replace("%20", " ");
                XmlTextWriter xmlWriter = new XmlTextWriter(new FileStream(xmlFilePath, FileMode.Create), System.Text.Encoding.UTF8);
                xmlWriter.Formatting = Formatting.Indented;

                xmlWriter.WriteStartElement("Settings");
                xmlWriter.WriteAttributeString("xmlns", "http://tempuri.org/JoystickSettings.xsd");

                xmlWriter.WriteElementString(GlobalJoystickSetting.InvertXAxis.ToString(), GetJoystickSetting(GlobalJoystickSetting.InvertXAxis));
                xmlWriter.WriteElementString(GlobalJoystickSetting.InvertYAxis.ToString(), GetJoystickSetting(GlobalJoystickSetting.InvertYAxis));
                xmlWriter.WriteElementString(GlobalJoystickSetting.InvertZAxis.ToString(), GetJoystickSetting(GlobalJoystickSetting.InvertZAxis));
                xmlWriter.WriteElementString(GlobalJoystickSetting.InvertXAxisRotation.ToString(), GetJoystickSetting(GlobalJoystickSetting.InvertXAxisRotation));
                xmlWriter.WriteElementString(GlobalJoystickSetting.InvertYAxisRotation.ToString(), GetJoystickSetting(GlobalJoystickSetting.InvertYAxisRotation));
                xmlWriter.WriteElementString(GlobalJoystickSetting.InvertZAxisRotation.ToString(), GetJoystickSetting(GlobalJoystickSetting.InvertZAxisRotation));

                foreach (DictionaryEntry entry in CommandMappings)
                {
                    ActionCommand actionCommand = (ActionCommand)entry.Value;

                    string button = actionCommand.buttonOrAxis.ToString();
                    string command = actionCommand.command.ToString();
                    string param = actionCommand.Parameter.ToString();

                    xmlWriter.WriteStartElement("Sensors");
                    xmlWriter.WriteElementString("Sensor", button);
                    xmlWriter.WriteElementString("Command", command);
                    xmlWriter.WriteElementString("Parameter", param);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteFullEndElement();
                xmlWriter.Close();
                SaveCurrentJoystickConf(name);
            }
            catch (Exception)
            {

            }
        }

        private void SaveCurrentJoystickConf(string name)
        {
            Properties.Settings.Default.CurrentJoystickConf = name;
            Properties.Settings.Default.Save();
        }

        public string GetCurrentJoystickConf()
        {
            return Properties.Settings.Default.CurrentJoystickConf;
        }

        public string GetJoystickSetting(GlobalJoystickSetting setting)
        {
            return settings[setting] as string;
        }

        /// <summary>
        /// Devuelve el ActionCommand asociado al boton o eje especificado
        /// </summary>
        /// <param name="buttonOrAxisName">el nombre del boton o eje</param>
        /// <returns>el ActionCommand</returns>
        public ActionCommand GetActionCommand(string buttonOrAxisName)
        {
            ButtonOrAxis buttonOrAxis = buttonOrAxis = (ButtonOrAxis)Enum.Parse(typeof(ButtonOrAxis), buttonOrAxisName, true);

            return GetActionCommand(buttonOrAxis);
        }

        /// <summary>
        /// Devuelve el ActionCommand asociado al boton o eje especificado
        /// </summary>
        /// <param name="buttonOrAxisName">el nombre del boton o eje</param>
        /// <returns>el ActionCommand</returns>
        public ActionCommand GetActionCommand(ButtonOrAxis buttonOrAxis)
        {
            ActionCommand command = null;
            lock (CommandMappings.SyncRoot)
            {
                command = CommandMappings[buttonOrAxis] as ActionCommand;
            }

            return command;
        }
        public ActionCommand GetActionCommand(PtzCommand ptzCommand, double parameter, bool buttonsOnly)
        {
            foreach (DictionaryEntry entry in CommandMappings)
            {
                ActionCommand actionCommand = (ActionCommand)entry.Value;

                if (buttonsOnly && !IsAButton(actionCommand.buttonOrAxis))
                {
                    continue;
                }

                if (actionCommand.command == ptzCommand &&
                    (actionCommand.Parameter == parameter))
                {
                    return actionCommand;
                }
            }

            return null;
        }

        public ActionCommand GetActionCommand(PtzCommand ptzCommand, bool buttonsOnly)
        {
            foreach (DictionaryEntry entry in CommandMappings)
            {
                ActionCommand actionCommand = (ActionCommand)entry.Value;

                if (buttonsOnly && !IsAButton(actionCommand.buttonOrAxis))
                {
                    continue;
                }

                if (actionCommand.command == ptzCommand)
                {
                    return actionCommand;
                }
            }

            return null;
        }

        private bool IsAButton(ButtonOrAxis boa)
        {
            return boa.ToString().ToLower().IndexOf("button") >= 0;
        }

        private Hashtable CommandMappings
        {
            get => _commandMappings;
            set => _commandMappings = value;
        }

        public void SetJoystickSetting(GlobalJoystickSetting setting, string settingValue)
        {
            settings[setting] = settingValue;
        }

        public void SetActionCommand(ButtonOrAxis buttonOrAxis, ActionCommand actionCommand)
        {
            ActionCommand oldActionCommand = GetActionCommand(buttonOrAxis);
            if (oldActionCommand != null)
            {
                oldActionCommand.buttonOrAxis = ButtonOrAxis.None;
            }

            lock (CommandMappings.SyncRoot)
            {
                CommandMappings.Remove(buttonOrAxis);

                /// Si el comando estaba asignado a un boton 
                /// o no estaba asignado lo reemplazo por el nuevo
                if (IsAButton(actionCommand.buttonOrAxis) || actionCommand.buttonOrAxis == ButtonOrAxis.None)
                {
                    CommandMappings.Remove(actionCommand.buttonOrAxis);
                }
                /// Si el comando estaba asignado a un eje, 
                /// creo una copia del comando con el nuevo boton
                else
                {
                    actionCommand = actionCommand.clone();
                }

                //Asigno el nuevo boton
                actionCommand.buttonOrAxis = buttonOrAxis;

                //Agrego el comando a la tabla
                CommandMappings[buttonOrAxis] = actionCommand;
            }
        }
    }
}
