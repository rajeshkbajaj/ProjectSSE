
namespace Covidien.CGRS.PcAgentInterfaceBusiness
{

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Oasis.Agent;
using Serilog;


    // Naming Conventions:
    // Prefixes:
    // M - static members
    // p - properties
    // m - members

    public class DeviceInformation
    {
        private const string BODY_WRAPPER = "//body";
        private const string DEVICE_IDENTITY_WRAPPER = "//body/device_identity";

        private const string MAKE_TAG = "make";
        private const string TYPE_TAG = "type";
        private const string MODEL_TAG = "model";
        private const string MODEL_TAG2 = "modle";
        private const string SERIAL_NUMBER_TAG = "serial_number";
        private const string SOFTWARE_VERSION = "release";
        private const string DATA_KEY_TYPE = "datakey_type";


        public string Make { get; set; }
        public string DeviceType { get; set; }
        public string Model { get; set; }

        public PertinentType PertinentType { get; set; }    // to be deprecated, I think, now will use make/type/model mostly.  probably have to keep this for the internal key/guid
        public string SerialNumber { get; set; }
        public string SoftwareVersion { get; set; }
        public string SoftwarePartnumber { get; set; }
        public string DeviceKeyType { get; set; }

        public List<DeviceComponentInfo> Components { get; set; }

        public DeviceInformation()
        {
        }

        public DeviceInformation( string devClass, string serialNumber )
        {
            PertinentType = new PertinentType( devClass, ModalConstants.Instance.GetConstant( ModalConstants.DEVICE_INFO ) ) ;
            SerialNumber = serialNumber;
        }

        static public DeviceInformation LoadDeviceInfo( string xmlStr )
        {
            if  ( string.IsNullOrEmpty( xmlStr ) )
            {
                return( null ) ;
            }

            bool foundData = false;
            DeviceInformation result = new DeviceInformation() ;

            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.LoadXml( xmlStr ) ;
            }
            catch( Exception ex)
            {
                Log.Error($"DeviceInformation::LoadDeviceInfo Exception:{ex.Message}");
                throw;  // someone lied, this is not a good xml string
            }

            XmlNode info;
            info = xmlDoc.SelectSingleNode( DEVICE_IDENTITY_WRAPPER ) ;
            if  ( null != info )
            {
                if  ( null != info[ MAKE_TAG ] )
                {
                    result.Make = info[ MAKE_TAG ].InnerText;
                    foundData = true;
                }
                if  ( null != info[ TYPE_TAG ] )
                {
                    result.DeviceType = info[ TYPE_TAG ].InnerText;
                    foundData = true;
                }
                if  ( null != info[ MODEL_TAG2 ] )
                {
                    result.Model = info[ MODEL_TAG2 ].InnerText;
                    foundData = true;
                }
                if  ( null != info[ MODEL_TAG ] )
                {
                    result.Model = info[ MODEL_TAG ].InnerText;
                    foundData = true;
                }
                if  ( null != info[ SERIAL_NUMBER_TAG ] )
                {
                    result.SerialNumber = info[ SERIAL_NUMBER_TAG ].InnerText;
                    foundData = true;
                }
                if  ( null != info[ SOFTWARE_VERSION ] )
                {
                    result.SoftwareVersion = info[ SOFTWARE_VERSION ].InnerText;
                    foundData = true;
                }
                
                string devType = "UNKNOWN_ERROR";
                if  ( !string.IsNullOrEmpty( result.Model ) )
                {
                    switch( result.Model )
                    {
                        case "PB840":
                            devType = ModalConstants.PB840_VENTILATOR;
                            result.DeviceType = devType;
                            foundData = true;
                            break;
                        case "PB980":
                            devType = ModalConstants.PB980_VENTILATOR;
                            result.DeviceType = devType;
                            foundData = true;
                            break;
                        default:
                            break;
                    }
                }
                result.PertinentType = new PertinentType( ModalConstants.Instance.GetConstant( devType ), ModalConstants.Instance.GetConstant( ModalConstants.DEVICE_INFO ) ) ;
            }

            info = xmlDoc.SelectSingleNode( BODY_WRAPPER ) ;
            if  ( null != info )
            {
                result.Components = DeviceComponentInfo.LoadDeviceComponentInfo( xmlDoc ) ;

                if (null != info[DATA_KEY_TYPE])
                {
                    result.DeviceKeyType = info[DATA_KEY_TYPE].InnerText;
                    foundData = true;
                }

                //search for data key type
                foreach (DeviceComponentInfo compInfo in result.Components)
                {
                    if (compInfo.Name.Equals("MasterVent"))
                    {
                        if (compInfo.OtherAttributes != null)
                        {
                            foreach (KeyValuePair<string, string> kvp in compInfo.OtherAttributes)
                            {
                                if (kvp.Key.Equals("datakey_type", StringComparison.OrdinalIgnoreCase))
                                {
                                    result.DeviceKeyType = kvp.Value;
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(compInfo.Revision) == false)
                        {
                            result.SoftwareVersion = compInfo.Revision;
                        }
                        if (string.IsNullOrEmpty(compInfo.PartNumber) == false)
                        {
                            result.SoftwarePartnumber = compInfo.PartNumber;
                        }
                        
                    }
                }
                foundData = true;
            }
            
            if  ( foundData )
            {
                return( result ) ;
            }
            else
            {
                return( null ) ;
            }
        }


        public string EncodeAsXml()
        {
            List<string> ExtraKeyValues = new List<string>(); 
            foreach( DeviceComponentInfo deviceComponentInfo in Components )
            {
                if  ( null != deviceComponentInfo.OtherAttributes )
                {
                    foreach( KeyValuePair<string,string> kvp in deviceComponentInfo.OtherAttributes )
                    {
                        if  ( !ExtraKeyValues.Contains( kvp.Key ) )
                            ExtraKeyValues.Add( kvp.Key ) ;
                    }
                }
            }


            StringBuilder sb = new StringBuilder();
            sb.Append( "<components>" ) ;
            foreach( DeviceComponentInfo deviceComponentInfo in Components )
            {
                sb.Append( string.Format(
@"<component type='{0}' >
<name>{1}</name>
<part_number>{2}</part_number>
<revision>{3}</revision>
<serial_number>{4}</serial_number>",
             deviceComponentInfo.Type, deviceComponentInfo.Name, deviceComponentInfo.PartNumber, deviceComponentInfo.Revision, deviceComponentInfo.SerialNumber));

                if  ( null != deviceComponentInfo.OtherAttributes )
                {
                    /* Do ordering below instead.  Keeping this in case we decide to have a flag to be ordered vs tight
                    foreach( KeyValuePair<string,string> kvp in deviceComponentInfo.OtherAttributes )
                    {
                        sb.Append( string.Format( "<{0}>{1}</{0}>", kvp.Key, kvp.Value ) ) ;
                    }
                    */

                    // Cause all items to be "ordered" so that XSLT puts things in the right columns
                    foreach( string key in ExtraKeyValues )
                    {
                        KeyValuePair<string,string> kvp = deviceComponentInfo.OtherAttributes.Find( item => item.Key == key ) ;
                        if  ( null == kvp.Key )
                        {
                            sb.Append( string.Format( "<{0}></{0}>", key ) ) ;
                        }
                        else
                        {
                            sb.Append( string.Format( "<{0}>{1}</{0}>", kvp.Key, kvp.Value ) ) ;
                        }
                    }
                }
                sb.Append( "</component>" ) ;
            }
            sb.Append( "</components>" ) ;

            return( sb.ToString() ) ;
        }
    }
}
