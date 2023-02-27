//*
//*  File:            ConfigXml.cs
//*  Product:         Integrated Patient Intelligence
//*  Module:          InformaticsCore
//*
//*  Description:     Provides singleton access to configuration file (app.config)
//*
//*  $Author: bill.jordan2 $
//*  $Revision: #2 $
//*  $Date: 2012/06/26 $
//*
//*  Copyright:       (c) 2011 - 2012 Nellcor Puritan Bennett LLC.
//*                   This document contains proprietary information to Nellcor Puritan Bennett LLC.
//*                   Transmittal, receipt or possession of this document does not express, license,
//*                   or imply any rights to use, design or manufacture from this information.
//*                   No reproduction, publication, or disclosure of this information, in whole or in part,
//*                   shall be made without prior written authorization from Nellcor Puritan Bennett LLC.


using System;
using System.Configuration;
using System.Xml;

namespace Covidien.Ipi.InformaticsCore
{
    public class ConfigXml : KeyValueXml
    {
        /// <summary>
        /// Reference to the singleton ConfigXml object.
        /// </summary>
        private static volatile ConfigXml MConfig;

        /// <summary>
        /// synchronization object for thread safety.
        /// </summary>
        private static readonly object MSynchronizeRoot = new object();


        /// <summary>
        /// We do not want to expose the constructor
        /// as we want to prevent anyone from creating
        /// the object without requesting the singleton.
        /// </summary>
        /// <returns></returns>
        protected ConfigXml()
        {
            mXmlDoc = new XmlDocument();
            try
            {
                mXmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            }
            catch (Exception)
            {
                //string logMessage = string.Format("ConfigXml::ConfigXml - failed to load XML file {0};", AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                //ErrorTrace.Trace(IpiEventCode.LoadXMLFileConfig, logMessage, EventStatusType.Critical, e);

                throw;
            }
        }

        /// <summary>
        /// This method returns the singleton object.
        /// </summary>
        public static ConfigXml Instance
        {
            get
            {
                /// We don't want the overhead of locking
                /// each time we try to get the object.
                /// Hence check for null first, if not null
                /// then just return the object.
                if (null == MConfig)
                {
                    /// Use a MSynchronizeRoot instance to lock on, 
                    /// rather than locking on the type 
                    /// itself, to avoid deadlocks
                    lock (MSynchronizeRoot)
                    {
                        if (null == MConfig)
                        {
                            MConfig = new ConfigXml();
                        }
                    }
                }
                return MConfig;
            }
        }

        /// <summary>
        /// This method saves the data and refreshes the section being edited
        /// </summary>
        public override void Save()
        {
            // Code to write to the actual file
            // mXmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            
            // Tell the section handlers that new data is available
            ConfigurationManager.RefreshSection("system-configuration");
            ConfigurationManager.RefreshSection("hibernate-configuration");
            ConfigurationManager.RefreshSection("debug-configuration");
        }
    }
}


