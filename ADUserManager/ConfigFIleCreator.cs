using System;
using System.IO;
using System.Xml;

public static class ConfigFileCreator
{
    public static void CreateConfigFileIfNotExists()
    {
        string configFile = "config.xml";
        string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile);

        if (!File.Exists(configFilePath))
        {
            using (XmlWriter writer = XmlWriter.Create(configFilePath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Configuration");
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }
}
