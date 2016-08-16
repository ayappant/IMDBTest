using System;
using System.Configuration;

namespace IMDBTests.Utilities
{
    public static class ConfigurationHelper
    {
        public static T Get<T>(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (typeof(T).IsEnum)
                return (T) Enum.Parse(typeof (T), value);
            return (T)Convert.ChangeType(value, typeof(T));
        }
        public static string GetConnectionString(string name)
        {
            var value = ConfigurationManager.ConnectionStrings[name];
            return value.ConnectionString;
        }
        public static string filePath = ConfigurationManager.AppSettings["FilePath"];
        public static string chromeDriverPath = ConfigurationManager.AppSettings["ChromeDriverPath"];
        public static string internetExplorerDriverPath = ConfigurationManager.AppSettings["InternetExplorerDriverPath"];
    }
}
