//*
//*  File:            CoreConstants.cs
//*  Product:         Integrated Patient Intelligence
//*  Module:          InformaticsCore
//*
//*  Description:     Provides constant values used throughout the architecture
//*
//*  $Author: bill.jordan2 $
//*  $Revision: #4 $
//*  $Date: 2012/06/26 $
//*
//*  Copyright:       (c) 2011 - 2012 Nellcor Puritan Bennett LLC.
//*                   This document contains proprietary information to Nellcor Puritan Bennett LLC.
//*                   Transmittal, receipt or possession of this document does not express, license,
//*                   or imply any rights to use, design or manufacture from this information.
//*                   No reproduction, publication, or disclosure of this information, in whole or in part,
//*                   shall be made without prior written authorization from Nellcor Puritan Bennett LLC.


using System;

namespace Covidien.Ipi.InformaticsCore
{
    /// This class holds commonly used constants.  Mostly about directories and file names & extensions currently.  Use in conjunction with CodeCache
    /// </summary>
    public class CoreConstants
    {

        // "Key" prefix means to be able to find in the config or applications XML file
        public const string KEY_COVIDIEN_PATH = "Covidien_path";     // expected to be found in App.Config -- program files area
        public const string KEY_APPLICATION_PATH = "application_path";    // expected to be found in App.Config -- program files area for this app
        public const string KEY_USER_PATH = "user_path";             // expected to be found in App.Config -- "root" path for files saved for user
        public const string KEY_DATA_PATH = "data_path";             // expected to be found in App.Config -- for storing raw data files and the like
        public const string KEY_PORT_NAME = "PortName";
        public const string KEY_DEVICE_TYPE = "DeviceType";
        public const string KEY_PORT_TYPE = "PortType";
        public const string KEY_BAUD_RATE = "BaudRate";
        public const string KEY_DATA_BITS = "DataBits";
        public const string KEY_DTR_ENABLE = "DtrEnable";
        public const string KEY_PARITY = "Parity";
        public const string KEY_PROTOCOL = "Protocol";
        public const string KEY_STOP_BITS = "StopBits";
        public const string KEY_HANDSHAKE = "Handshake";
        public const string KEY_RTS_ENABLE = "RtsEnable";
        public const string KEY_READ_BUFFER_SIZE = "ReadBufferSize";
        public const string KEY_WRITE_BUFFER_SIZE = "WriteBufferSize";
        public const string KEY_HOST_NAME = "hostname";
        public const string KEY_PORT_NUMBER = "portnum";    // typically for the remote
        public const string KEY_LOCAL_PORT_NUMBER = "LocalPortnum";
        public const string KEY_CONNECTION_TIME = "ConnectionTime";
        public const string KEY_REMOTE_MAC_ADDRESS = "RemoteMacAddress";
        public const string KEY_PREFIX_PRIOR_CONNECTION = "pc";
        public const string SECTION_NAME_PRIOR_CONNECTIONS = "PriorConnections";

        public const string KEY_LISTENER_UID = "uid";
        public const string KEY_LISTENER_PORT_TYPE = "portType";
        public const string KEY_LISTENER_PORT_NUMBER = "portNum";
        public const string KEY_LISTENER_DEVICE_TYPE = "deviceType";
        public const string KEY_LISTENER_PROTOCOL = "protocol";
        public const string KEY_LISTENER_SAVE_RAW = "saveRaw";
        public const string KEY_LISTENER = "listener";

        public const string PORT_TYPE_COM = "COM";
        public const string PORT_TYPE_TCP_CLIENT = "TCPClient";
        public const string PORT_TYPE_TCP_LISTENER = "TCPListener";

        public const string PATH_EXTEND_COVIDIEN = "Covidien";   // because the user path (My documents) is being pulled out from the environment

        /// <summary>
        /// Path extensions based off our "source" path (aka Covidien Path)
        /// </summary>
        public const string PATH_EXTEND_DEVICES = "SourceDefinitions";  // "devices";
        public const string PATH_EXTEND_PROTOCOLS = "SourceDefinitions";    // "protocols";
        public const string PATH_EXTEND_CONNECTIONS = "connections";
        public const string PATH_EXTEND_LISTENERS = "SourceDefinitions";    // "listeners";
        public const string PATH_SOURCE_DEFINITIONS = "SourceDefinitions";
        public const string PATH_EXTEND_SOURCE_DATAMAPS = "datamaps";   // "source" only added to name to contrast with "generated" below
        public const string PATH_EXTEND_SYSTEM_DLLS = "libs";
        public const string PATH_EXTEND_PROTOCOL_DLLS = "ProtocolDlls";
        public const string PATH_EXTEND_CLINICAL_DLLS = "ClinicalPlugins";
        public const string PATH_EXTEND_PROCESS_DLLS = "ProcessPlugins";
        public const string PATH_EXTEND_THIRD_PARTY_LIBS = "ThirdPartyLibs";
        public const string PATH_EXTEND_CANNED_DATA = "cannedData";

        /// <summary>
        /// Path extensions based off the User Path
        /// </summary>
        public const string PATH_EXTEND_GENERATED = "gen";
        public const string PATH_EXTEND_GENERATED_DLLS = "gen/dlls";
        public const string PATH_EXTEND_GENERATED_DATAMAPS = "gen/datamaps";
        public const string PATH_EXTEND_GENERATED_DATA = "data";  // note: purposefully not prefixed with gen/

        public const string PATH_SEPARATOR_DOUBLE = "//";
        public const char PATH_SEPARATOR_STD = '/';
        public const char PATH_SEPARATOR_WIN = '\\';
        public const char FILE_NAME_SEPARATOR = '.';
        public const char COMMA_DELIMITER = ',';
        public const string PATH_WILDCARD = ".*";

        /// <summary>
        /// File extensions
        /// </summary>
        public const string FILE_EXTENSION_DEVICE = "dev.xml";
        public const string FILE_EXTENSION_PROTOCOL = "pcl.xml";
        public const string FILE_EXTENSION_CONNECTION = "cnct.xml";
        public const string FILE_EXTENSION_LISTENER = "lstn.xml";
        public const string FILE_EXTENSION_LANGUAGE = "lang.xml";
        public const string FILE_EXTENSION_RESOURCE = "rsrc.xml";
        public const string FILE_EXTENSION_DATAMAP = "map.xml";
        public const string FILE_EXTENSION_NHIBERNATE = "hbm.xml";
        public const string SELECT_ALL_FILE_START = "*.";
        public const string FILE_EXTENSION_DLL = "dll";
        public const string FILE_EXTENSION_SQL = "sql";
        public const string FILE_EXTENSION_XML = "xml";
        public const string FILE_EXTENSION_EXECUTABLE = "exe";
        public const string FILE_EXTENSION_CSHARP = "cs";
        public const string FILE_EXTENSION_RAW = "raw";    // will typically be prefixed with the protocol, for a result of .protocol.raw

        /// <summary>
        /// File name for the platform / application settings
        /// </summary>
        public const string FILE_NAME_SETTINGS = "settings.xml"; 
    
        /// <summary>
        /// escape string for comma for strings going to T-SQL
        /// </summary>
        public const string COMMA_ESCAPE_STRING = @"%2c";




        public const double HECTO = 100.0;
        public const double KILO = 1000.0;
        public const double MEGA = KILO * KILO ;

        public const double MM_TO_INCH = 0.03937007874;
        public const double INCH_TO_MM = 1.0 / MM_TO_INCH ;

        public const double MMHG_TO_TORR = 1.0;     // i.e., one torr is one mmHg
        public const double TORR_TO_MMHG = 1.0;

        public const double MMHG_TO_MMH2O = 13.5951;
        public const double MMH2O_TO_MMHG = 1.0 / MMHG_TO_MMH2O ;

        public const double ATM_TO_TORR = 760.0;    // at sea level, the barometric pressure is 760 mmHg.  Actually, I think it is 1.013
        public const double TORR_TO_ATM = 1.0 / ATM_TO_TORR;

        public const double TORR_TO_MBAR = 1.33322368;
        public const double MBAR_TO_TORR = 1.0 / TORR_TO_MBAR;

        public const double TORR_TO_PA = 133.322368421;
        public const double PA_TO_TORR = 1.0 / TORR_TO_PA;

        public const double TORR_TO_KPA = TORR_TO_PA / KILO;



        public const string ASSERT_MESSAGE_PARAM_WAS_NULL = "{0}::{1}() - param \"{2}\" cannot be null;";  // params should be {class}, {method}, {param name}
        public const string ASSERT_MESSAGE_PARAM_WRONG_TYPE = "{0}::{1}() - param \"{2}\" was type \"{3}\" but should have been type \"{4}\";";  // params should be {class}, {method}, {param name}, {param type}, {desired type}
        public const string ASSERT_MESSAGE_PROPERTY_NOT_SET = "{0}::{1}() - property \"{2}\" not properly set (was \"{3}\";";  // params should be {class}, {method}, {property name}, {property value}


        /// <summary>
        /// GetName is intended to return the name of a variable, when called as GetName( new { variable } ) hence avoiding hardcoding strings
        /// Calling differently will simply result in the name of the first property of the instance being returned, and it might blow chunks if there were no properties
        /// </summary>
        /// <typeparam name="T">Single variable type</typeparam>
        /// <param name="item">Instance of the single variable type</param>
        /// <returns>Name of the variable</returns>
        public static string GetName<T>(T item) where T : class
        {
            const string ASSERT_MESSAGE_BAD_CALL = "Bad call to GetName() using type {0}" ;  // params should be {type}

            IpiAssert.Fail(1 == typeof(T).GetProperties().Length, String.Format( ASSERT_MESSAGE_BAD_CALL, typeof(T).Name));
            string retval = typeof(T).GetProperties()[0].Name;
            return (retval);
        }

         public delegate bool ValidateStringEnumFunction( string str ) ;
    }
}
