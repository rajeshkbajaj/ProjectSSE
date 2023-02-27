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
    using System.Collections.Generic;

    public class RestrictedFunctionManager
    {
        private bool UserLoggedIn { get; set; }
        private readonly Hashtable Restrictions; // key indexed "bool?[][] RestrictedFunctionStates"

        public enum RestrictionAlleviation
        {
            NO_RESTRICTIONS,            // no (further) restrictions
            DEVICE_CHECK_REQUIRED,      // need to check the device as a minimum
            LOGIN_OVERRIDE_REQUIRED,    // if user login fails, or user denied access for the given device (via get-device-info), then will result in state of AccessDenied
            ACCESS_DENIED               // once in access-denied state, will have to logout to re-login with an appropriately credentialled user.
        };

        public enum RestrictedFunctions
        {
            FIRST,
            LOG_VIEW = FIRST,
            LOG_UPLOAD,
            BROWSE_TO_SELECT_DOWNLOAD_PACKAGE,
            SOFTWARE_DOWNLOAD,
            SET_PREVENTIVE_MAINTENANCE_DUE_HOURS,
            SET_OPERATIONAL_HOURS,
            SET_COMPRESSOR_SERIAL_NUMBER,
            OVERRIDE_EST_RESULT,
            SET_OPTIONS_KEY,
            EXTEND_ENHANCED_SERVICE_MODE,
            VIEW_DEVICE_KEY_TYPE,
            SET_EST_STATUS,
            SET_EST_PERFORMED_DATE,
            // additional functions go here.
            // Server Access is really only controlled by the login, so not here, must check property UserLoggedIn
            NUM_RESTRICTED_FUNCTIONS
        };

        public enum RestrictionSource
        {
            FIRST,
            DEVICE = FIRST,
            USER,
            // additional sources (if any) go here.
            NUM_RESTRICTED_SOURCES
        };
     
        /// <summary>
        /// Functions are resricted on a per device basis.  Hence, one manager instance per device
        /// </summary>
        public RestrictedFunctionManager()
        {
            Restrictions = new Hashtable();
        }

        public bool IsUserLoggedIn()
        {
            return UserLoggedIn;
        }

        public void SetUserLoggedIn(bool loggedIn)
        {
            UserLoggedIn = loggedIn;
        }

        public void InitDevice( string deviceId )
        {
            if  ( !Restrictions.ContainsKey( deviceId ) )
            {
                bool?[][] RestrictedFunctionStates = new bool?[(int)RestrictionSource.NUM_RESTRICTED_SOURCES][];
                for (RestrictionSource src = RestrictionSource.FIRST; src < RestrictionSource.NUM_RESTRICTED_SOURCES; src++)
                {
                    RestrictedFunctionStates[(int)src] = new bool?[(int)RestrictedFunctions.NUM_RESTRICTED_FUNCTIONS];
                }
                Restrictions.Add( deviceId, RestrictedFunctionStates ) ;
            }
            // else it has already been init.
        }


        /// <summary>
        /// Updates an individual right.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="src"></param>
        /// <param name="func"></param>
        /// <param name="isOK"></param>
        public void UpdateRight( string deviceId, RestrictionSource src, RestrictedFunctions func, bool ? isOK )
        {
            if  ( !Restrictions.ContainsKey( deviceId ) )
            {
                InitDevice( deviceId ) ;
            }
            bool?[][] RestrictedFunctionStates = (bool?[][])Restrictions[deviceId];
            RestrictedFunctionStates[(int)src][(int)func] = isOK;
        }


        /// <summary>
        /// Updates a list of rights from the same source
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="src"></param>
        /// <param name="funcs">KeyValuePair of function and right</param>
        public void UpdateRights( string deviceId, RestrictionSource src, List<KeyValuePair<RestrictedFunctions,bool?>> funcs )
        {
            foreach( KeyValuePair<RestrictedFunctions, bool?> funcInfo in funcs )
            {
                UpdateRight( deviceId, src, funcInfo.Key, funcInfo.Value ) ;
            }
        }


        /// <summary>
        /// Determines what level, if any, restriction alleviation is required.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="restrictedFuncId"></param>
        /// <returns></returns>
        public RestrictionAlleviation IsFunctionRestricted( string deviceId, RestrictedFunctions restrictedFuncId )
        {
            if (!Restrictions.ContainsKey(deviceId))
            {
                InitDevice(deviceId);
            }
            bool?[][] RestrictedFunctionStates = (bool?[][])Restrictions[deviceId];

            // If logged in with authorized access to the given function, then allow without further restrictions
            if (UserLoggedIn
                && (null != RestrictedFunctionStates[(int)RestrictionSource.USER][(int)restrictedFuncId])
                && (true == RestrictedFunctionStates[(int)RestrictionSource.USER][(int)restrictedFuncId]))
            {
                return (RestrictionAlleviation.NO_RESTRICTIONS);
            }

            // If (not logged in or not allowed access), then check for device providing allowance
            if (null == RestrictedFunctionStates[(int)RestrictionSource.DEVICE][(int)restrictedFuncId])
            {
                return (RestrictionAlleviation.DEVICE_CHECK_REQUIRED);
            }
            else if (true == RestrictedFunctionStates[(int)RestrictionSource.DEVICE][(int)restrictedFuncId])
            {
                return (RestrictionAlleviation.NO_RESTRICTIONS);
            }
            else if (UserLoggedIn)
            {
                return (RestrictionAlleviation.ACCESS_DENIED);
            }
            else
            {
                //return (RestrictionAlleviation.LOGIN_OVERRIDE_REQUIRED);
                return RestrictionAlleviation.ACCESS_DENIED;
            }
        }


        /// <summary>
        /// Determines whether or not a function is restricted based on the source.
        /// </summary>
        /// A specific UserLoggedIn would need to occur separately.
        /// <param name="deviceId"></param>
        /// <param name="src"></param>
        /// <param name="restrictedFuncId"></param>
        /// <returns>null if no data provided by source, false if denied, true if OK (sorry about the inversion)</returns>
        public bool ? IsFunctionSourceRestricted( string deviceId, RestrictionSource src, RestrictedFunctions restrictedFuncId )
        {
            if  ( !Restrictions.ContainsKey( deviceId ) )
            {
                InitDevice( deviceId ) ;
            }
            bool?[][] RestrictedFunctionStates = (bool?[][])Restrictions[deviceId];

            if  ( null != RestrictedFunctionStates[(int)src][(int)restrictedFuncId] )
            {
                return( !RestrictedFunctionStates[(int)src][(int)restrictedFuncId] ) ;
            }
            else
            {
                return( null ) ;
            }
        }
    }
}
