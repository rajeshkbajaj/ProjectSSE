// ------------------------------------------------------------------------------
//                    Copyright (c) 2022 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Covidien.CGRS.PcAgentInterfaceBusiness
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class DeviceComponentInfo
    {
        private const string COMPONENT_WRAPPER = "//components/component";
        private const string TYPE_ATTRIB = "type";
        private const string NAME_TAG = "name";
        private const string PART_NUMBER_TAG = "part_number";
        private const string SERIAL_NUMBER_TAG = "serial_number";
        private const string REVISION_TAG = "revision";
        private const string RIGHTS_TAG = "rights";
        private const string NOT_INSTALLED_1 = "Not Installed";
        private const string NOT_INSTALLED_2 = "NotInstall";
        private static readonly string[] KNOWN_TAGS = new string[] { NAME_TAG, PART_NUMBER_TAG, SERIAL_NUMBER_TAG, REVISION_TAG, RIGHTS_TAG };

        public string Name { get; set; }
        public string Type { get; set; }
        public string PartNumber { get; set; }
        public string SerialNumber { get; set; }
        public string Revision { get; set; }
        public bool ? IsInstalled { get; set; }

        public List<string> AccessRights { get; set; }
        public List<KeyValuePair<string, string>> OtherAttributes { get; set; } 

        public DeviceComponentInfo( string name )
        {
            Name = name;
            IsInstalled = null;
            OtherAttributes = null;
        }

        static public List<DeviceComponentInfo> LoadDeviceComponentInfo( string xmlStr )
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml( xmlStr ) ;
            }
            catch (Exception)
            {
                throw;
            }

            return( LoadDeviceComponentInfo( xmlDoc ) ) ;
        }

        static public List<DeviceComponentInfo> LoadDeviceComponentInfo( XmlDocument xmlDoc )
        {
            List<DeviceComponentInfo> results = new List<DeviceComponentInfo>();

            XmlNodeList deviceInfo = xmlDoc.SelectNodes( COMPONENT_WRAPPER ) ;
            foreach( XmlElement componentInfo in deviceInfo )
            {
                DeviceComponentInfo compInfo = null;

                string type = componentInfo.GetAttribute( TYPE_ATTRIB ) ;
                if  ( !string.IsNullOrEmpty(type)  &&  ( null != componentInfo[ NAME_TAG ] ) )
                {
                    compInfo = new DeviceComponentInfo( componentInfo[ NAME_TAG ].InnerText ) ;
                    compInfo.Type = type;
                    results.Add(compInfo);
                }
                else
                {
                    continue;  // if the message is not named & typed, then ignore it
                }

                if (null != componentInfo[ PART_NUMBER_TAG ])
                {
                    compInfo.PartNumber = componentInfo[ PART_NUMBER_TAG ].InnerText ;
                    if  ( compInfo.PartNumber.Equals( NOT_INSTALLED_1, StringComparison.OrdinalIgnoreCase )
                        || compInfo.PartNumber.Equals( NOT_INSTALLED_2, StringComparison.OrdinalIgnoreCase ) )
                    {
                        compInfo.IsInstalled = false ;
                        compInfo.PartNumber = null ;
                    }
                    else
                    {
                        compInfo.IsInstalled = true ;
                    }
                }
                else
                {
                    compInfo.IsInstalled = null ;
                }

                if  ( null != componentInfo[ REVISION_TAG ] )
                {
                    compInfo.Revision = componentInfo[ REVISION_TAG ].InnerText ;
                }

                if  ( null != componentInfo[ SERIAL_NUMBER_TAG ] )
                {
                    compInfo.SerialNumber = componentInfo[ SERIAL_NUMBER_TAG ].InnerText ;
                }

                if  ( null != componentInfo[ RIGHTS_TAG ] )
                {
                    foreach( XmlElement elm in componentInfo[ RIGHTS_TAG ] )
                    {
                        if  ( null == compInfo.AccessRights )
                        {
                            compInfo.AccessRights = new List<string>();
                        }
                        compInfo.AccessRights.Add( elm.InnerText ) ;
                    }
                }

                foreach( XmlElement elm in componentInfo )
                {
                    if  ( !KNOWN_TAGS.Contains( elm.Name ) )
                    {
                        if  ( null == compInfo.OtherAttributes )
                        {
                            compInfo.OtherAttributes = new List<KeyValuePair<string, string>>();
                        }
                        compInfo.OtherAttributes.Add( new KeyValuePair<string, string>( elm.Name, elm.InnerText ) ) ;
                    }
                }
            }
            return( results ) ;
        }
    }
}
