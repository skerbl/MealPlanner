using System.IO;
using System.Xml.Serialization;

namespace MPData
{
    public static class UserSettingsXml
    {
        private static string _settingsPath = @"settings\settings.xml";

        public static void Serialize(UserSettings settings)
        {
            FileInfo file = new FileInfo(_settingsPath);
            file.Directory.Create();
            
            using (TextWriter writer = new StreamWriter(_settingsPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UserSettings));
                serializer.Serialize(writer, settings);
            }
        }

        public static UserSettings Deserialize()
        {
            FileInfo file = new FileInfo(_settingsPath);
            XmlSerializer deserializer = new XmlSerializer(typeof(UserSettings));
            UserSettings settings;

            if (file.Exists)
            {
                using (TextReader reader = new StreamReader(_settingsPath))
                {
                    object obj = deserializer.Deserialize(reader);
                    settings = obj as UserSettings;

                    if (string.IsNullOrEmpty(settings.ExportPath))
                    {
                        settings.ExportPath = "";
                    }

                    if (string.IsNullOrEmpty(settings.TemplatePath))
                    {
                        settings.TemplatePath = "";
                    }
                }
            }
            else
            {
                file.Directory.Create();
                settings = new UserSettings { ExportPath = "", TemplatePath = "", SaveAsPdf = true };

                using (TextWriter writer = new StreamWriter(_settingsPath))
                {
                    deserializer.Serialize(writer, settings);
                }
            }

            return settings;
        }
    }
}
