// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Config.cs" >
// -
// Copyright (C) 2017 J.Vogel
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// -
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
// -
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// -
// </copyright>
// <summary>
// </summary>
//  -------------------------------------------------------------------------------------------------------------------
// http://www.gnu.org/licenses/>.
namespace SpacerHotKeys
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    [Serializable]
    public class Config
    {
        public string LastProcessName { get; set; }

        public string LastSelectedItem { get; set; }

        public static Config Load(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Config));

                    using (var stringReader = new StringReader(File.ReadAllText(path)))
                    {
                        using (XmlReader xmlReader = XmlReader.Create(stringReader))
                        {
                            return xmlSerializer.Deserialize(xmlReader) as Config;
                        }
                    }
                }
                catch
                {
                    // We can not load the config. Ignore it.
                    return null;
                }
            }

            return null;
        }

        public void Save(string path)
        {
            var xmlSerializer = new XmlSerializer(typeof(Config));

            using (var stringWriter = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    xmlSerializer.Serialize(writer, this);
                    File.WriteAllText(path, stringWriter.ToString());
                }
            }
        }
    }
}