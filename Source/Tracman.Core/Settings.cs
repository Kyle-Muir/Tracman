using System;
using System.Configuration;

namespace Tracman.Core
{
    public class Settings
    {
        public static Setting Load(string key)
        {
            try
            {
                return new Setting(key, ConfigurationManager.AppSettings[key]);
            }
            catch (Exception error)
            {
                throw new ConfigurationErrorsException(string.Format("App Setting with name {0} was not defined", key), error);
            }
        }
    }
}