using Ookii.Dialogs.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using FusionExplorer.Models.Math;
using FusionExplorer.Models.Utility;

namespace FusionExplorer
{
    class Utility
    {   
        // Read LZMA compression properties from a byte array
        

        

        

        public static string GetConfigValue(string key)
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;
            return confCollection[key].Value;
        }

        public static void SetConfigValue(string key, string value)
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;
            confCollection[key].Value = value; ;
        }
    }
}
