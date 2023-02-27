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
    using System.Collections;

    public class ModalConstants
    {
        private bool IsDebug { get; set; }

        // RTB - choosing to use underscore only to separate names, so can easily switch between display blanks and internal names by exchanging underscore and space.

        // Mailbox Classification
        public const string DEVICE_INBOX = "DEVICE_INBOX";
        public const string DEVICE_INBOX_GUID = "34caa93b-6ed3-4a93-9be7-4e31eb4b22d5";

        public const string DEVICE_OUTBOX = "DEVICE_OUTBOX";
        public const string DEVICE_OUTBOX_GUID = "3bd775ad-7250-42ff-adf4-188e0b691bff";

        public const string SYSTEM_INBOX = "SYSTEM_INBOX";
        public const string SYSTEM_INBOX_GUID = "b20af193-6353-4cf5-95cc-e79b84d88907";

        public const string SYSTEM_OUTBOX = "SYSTEM_OUTBOX";
        public const string SYSTEM_OUTBOX_GUID = "5b5fd529-4244-46be-88cd-1a1304c179df";


        // Device Classification
        public const string PB980_VENTILATOR = "PB980_VENTILATOR";
        public const string PB980_VENTILATOR_GUID = "7a85f0c9-531e-4754-ad68-04c77ed63657";

        public const string PB840_VENTILATOR = "PB840_VENTILATOR";
        public const string PB840_VENTILATOR_GUID = "a7a65225-ef2d-48e9-89c8-6975dd7dc054";


        // Type Classification
        public const string DEVICE_INFO = "DEVICE_INFO";
        public const string DEVICE_INFO_GUID = "532ffdda-6f38-41da-9a40-cb0801e6695f";

        public const string DEVICE_SOFTWARE_UPGRADE_PACKAGE = "DEVICE_SOFTWARE"; // note: for the vent, the firmware is packaged with the software as one big chunk.  will only use the S/W tag.
        public const string DEVICE_SOFTWARE_UPGRADE_PACKAGE_GUID = "24637560-edb3-4961-b17d-14e74f74457c";

        public const string DEVICE_SOFTWARE_DOWNLOAD_ACK = "DEVICE_SOFTWARE_DOWNLOAD_ACK";
        public const string DEVICE_SOFTWARE_DOWNLOAD_ACK_GUID = "91dec715-f256-421a-8560-e6b015dae5f9";

        public const string DEVICE_FIRMWARE_UPGRADE_PACKAGE = "DEVICE_FIRMWARE";
        public const string DEVICE_FIRMWARE_UPGRADE_PACKAGE_GUID = "a582e8a9-282a-4f49-93d1-f11547826a91";

        public const string DEVICE_FIRMWARE_DOWNLOAD_ACK = "DEVICE_FIRMWARE_DOWNLOAD_ACK";
        public const string DEVICE_FIRMWARE_DOWNLOAD_ACK_GUID = "2c0d006f-2282-4417-bf20-2096b56a9ff6";

        public const string DEVICE_HARDWARE = "DEVICE_HARDWARE";
        public const string DEVICE_HARDWARE_GUID = "fc1f3c2c-16df-4b52-a9ea-99409b131d31";


        public const string DEVICE_LIST = "DEVICE_LIST";
        public const string DEVICE_LIST_GUID = "06949c6d-5893-4579-b587-736373685683";

        public const string DEVICE_STAT = "DEVICE_STAT";
        public const string DEVICE_STAT_GUID = "4bee9772-adcb-4ec2-b944-3191492b3436";


        public const string DEVICE_RAW_LOG = "DEVICE_RAW_LOG";
        public const string DEVICE_RAW_LOG_GUID = "2f62d564-a162-440a-a5f6-ed16e7e632d5";

        public const string DEVICE_DECODED_LOG = "DEVICE_DECODED_LOG";
        public const string DEVICE_DECODED_LOG_GUID = "8e74db99-f0a3-4de4-aeed-f17afb6896fc";

        public const string RELEASE_NOTES = "RELEASE_NOTES";
        public const string RELEASE_NOTES_GUID = "02d7aa7b-25e0-423f-8f98-c15612e6a7c0";

        public const string SERVICE_MANUAL = "SERVICE_MANUAL";
        public const string SERVICE_MANUAL_GUID = "c250c8db-b532-4d18-8956-420c3d637a41";

        public const string OTHER_DOCUMENT = "OTHER_NOTES";
        public const string OTHER_DOCUMENT_GUID = "5789f84e-7f582-6e30-8289-692C57e59d63";

        public const string USER_GUIDE = "USER_GUIDE";
        public const string USER_GUIDE_GUID = "f520b9ce-c641-5e29-9167-531d4e748b52";

        public const string SYSTEMWIDE_NOTIFICATION = "SYSTEMWIDE_NOTIFICATION";
        public const string SYSTEMWIDE_NOTIFICATION_GUID = "44972a33-8fc4-4742-a1fc-940f2cc1cc92";

        /*
87b9a66c-a054-465a-80da-fb4ab1737171
14a5d387-0973-4be4-b4e6-2455977c7d8b
7a068c82-8855-43b3-a403-57259d1639fa
9bbcce58-b875-4c52-b53b-08b3356b8a2b
c7440d3f-77d6-46a4-8531-bc5f9f40d170
        */

        /// <summary>
        /// Provides mapping of key (constant) to guid
        /// </summary>
        private readonly Hashtable Guids;
        private readonly Hashtable Keys;

        /// <summary>
        /// The variable provides the locking object
        /// </summary>
        private static readonly object MSyncRoot = new object();

        /// <summary>
        /// mSingleton and Instance provides the private variable and public access to it - to provide the Singleton.
        /// The double-check method for lazy evaluation of the Singleton properly supports multi-threaded applications
        /// </summary>
        private static volatile ModalConstants MSingleton;
        public static ModalConstants Instance
        {
            get
            {
                if (null == MSingleton)
                {
                    lock (MSyncRoot)
                    {
                        if (null == MSingleton)
                        {
                            MSingleton = new ModalConstants();
                        }
                    }
                }
                return MSingleton;
            }
        }


        /// <summary>
        /// Private ctor() in support of singleton
        /// </summary>
        private ModalConstants()
        {
            IsDebug = false;    // clearly state we are in primary execution (by default)

            Guids = new Hashtable();
            Keys = new Hashtable();

            // Mailbox Classification
            Guids.Add( DEVICE_INBOX, DEVICE_INBOX_GUID ) ;
            Guids.Add( DEVICE_OUTBOX, DEVICE_OUTBOX_GUID ) ;
            Guids.Add( SYSTEM_INBOX, SYSTEM_INBOX_GUID ) ;
            Guids.Add( SYSTEM_OUTBOX, SYSTEM_OUTBOX_GUID ) ;

            // Device Classification
            Guids.Add( PB980_VENTILATOR, PB980_VENTILATOR_GUID ) ;
            Guids.Add( PB840_VENTILATOR, PB840_VENTILATOR_GUID ) ;

            // Type Classification
            Guids.Add( DEVICE_INFO, DEVICE_INFO_GUID ) ;
            Guids.Add( DEVICE_SOFTWARE_UPGRADE_PACKAGE, DEVICE_SOFTWARE_UPGRADE_PACKAGE_GUID ) ;
            Guids.Add( DEVICE_SOFTWARE_DOWNLOAD_ACK, DEVICE_SOFTWARE_DOWNLOAD_ACK_GUID ) ;
            Guids.Add( DEVICE_FIRMWARE_UPGRADE_PACKAGE, DEVICE_FIRMWARE_UPGRADE_PACKAGE_GUID ) ;
            Guids.Add( DEVICE_FIRMWARE_DOWNLOAD_ACK, DEVICE_FIRMWARE_DOWNLOAD_ACK_GUID ) ;
            Guids.Add( DEVICE_HARDWARE, DEVICE_HARDWARE_GUID ) ;
            Guids.Add( DEVICE_LIST, DEVICE_LIST_GUID ) ;
            Guids.Add( DEVICE_STAT, DEVICE_STAT_GUID ) ;

            Guids.Add( DEVICE_RAW_LOG, DEVICE_RAW_LOG_GUID ) ;
            Guids.Add( DEVICE_DECODED_LOG, DEVICE_DECODED_LOG_GUID ) ;

            Guids.Add( RELEASE_NOTES, RELEASE_NOTES_GUID ) ;
            Guids.Add( SERVICE_MANUAL, SERVICE_MANUAL_GUID ) ;
            Guids.Add(OTHER_DOCUMENT, OTHER_DOCUMENT_GUID);
            Guids.Add(USER_GUIDE, USER_GUIDE_GUID);

            Guids.Add( SYSTEMWIDE_NOTIFICATION, SYSTEMWIDE_NOTIFICATION_GUID ) ;


            foreach( string key in Guids.Keys )
            {
                Keys.Add( Guids[ key ], key ) ;
            }
        }


        /// <summary>
        /// Kind of a paradox to have a variable contant.  Suits me perfectly.
        /// </summary>
        /// <param name="key">Contant key</param>
        /// <returns>Appropriate contant, based on our mode (e.g., debug vs not-debug)</returns>
        public string GetConstant( string key )
        {
            if  ( !IsDebug )
            {
                string retstr = (string) Guids[ key ] ;
                if  ( string.IsNullOrEmpty( retstr ) )
                    return( key ) ;
                else return( retstr ) ;
            }
            else
            {
                return( key ) ;
            }
        }


        /// <summary>
        /// Converts the debug key into the runtime guid constant
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetRuntimeConstant( string key )
        {
            string retstr = (string) Guids[ key ] ;
            if  ( string.IsNullOrEmpty( retstr ) )
                return( key ) ;
            else return( retstr ) ;
        }


        /// <summary>
        /// Converts the runtime guid into the debug key constant
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public string GetDebugKey( string guid )
        {
            string retstr = (string) Keys[ guid ] ;
            if  ( string.IsNullOrEmpty( retstr ) )
                return( guid ) ;
            else return( retstr ) ;
        }


        /// <summary>
        /// Convert the key into a displayable string
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string KeyToDisplayString(string key)
        {
            return( key.Replace( '_', ' ' ) ) ;
        }

        /// <summary>
        /// Convert the display string back into a key
        /// </summary>
        /// <param name="dispStr"></param>
        /// <returns></returns>
        public static string DisplayStringToKey( string dispStr )
        {
            return( dispStr.Replace( ' ', '_' ) ) ;
        }
    }
}
