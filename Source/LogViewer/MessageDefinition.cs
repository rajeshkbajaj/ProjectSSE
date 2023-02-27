using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace LogViewer
{
    public class MessageDefinition
    {
        public String Name { get; set; }
        public String SrcFile { get; set; }
        public List<KeyValuePair<string, string>> MetaData { get; set; }
        public List<FieldDefinition> Fields { get; set; }
        public List<UnionDefinition> Unions { get; set; }

        /// <summary>
        /// ctor()
        /// </summary>
        /// <param name="name">Name of the message</param>
        public MessageDefinition( string name )
        {
            Name = name;
        }


        /// <summary>
        /// Loads a file which describes (potentially) multiple messages
        /// </summary>
        /// <param name="filename">Filename to be loaded</param>
        /// <returns>List of DeviceDefinition objects</returns>
        public static List<MessageDefinition> LoadDefinitions(string filename)
        {
            List<MessageDefinition> results = new List<MessageDefinition>();
            MessageDefinition msgDefn = null;

            string xmlStr = System.IO.File.ReadAllText(filename);

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml(xmlStr);
            }
            catch( Exception e )
            {
                throw;
            }


            XmlNodeList messages = xmlDoc.SelectNodes("//message");
            foreach (XmlElement message in messages)
            {
                string name = message.GetAttribute("name");
                if (null != name)
                {
                    msgDefn = new MessageDefinition(name);
                    msgDefn.SrcFile = filename;
                    results.Add(msgDefn);
                }
                else
                {
                    continue;  // if the message is not named, then ignore it
                }

                FieldDefinition fieldDefn = null ;
                XmlNodeList fields = xmlDoc.SelectNodes("//message[@name='" + name + "']/fields/field");
                foreach( XmlElement field in fields )
                {
                    fieldDefn = new FieldDefinition() ;
                    if  ( fieldDefn.Itor( field ) )
                    {
                        msgDefn.Fields.Add( fieldDefn ) ;
                    }
                }
            }

            return( results ) ;
        }
    }
}
