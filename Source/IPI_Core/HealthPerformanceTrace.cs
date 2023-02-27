//*
//*  File:            HealthPerformanceTrace.cs
//*  Product:         Integrated Patient Intelligence
//*  Module:          InformaticsCore
//*
//*  Description:
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
using System.Diagnostics;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Covidien.Ipi.InformaticsCore
{
    /// <summary>
    /// HealthPerformanceTrace - a singleton object used to log all data
    /// </summary>
    public class HealthPerformanceTrace
    {
        public const string TRACE_LISTENER_BASE = 
            "IpiTracing/IpiTraceListeners";

        public const string TRACE_LISTENER_ASSEMBLY_KEY =
            TRACE_LISTENER_BASE+"/{0}/IpiTraceListenerAssembly";

        public const string TRACE_LISTENER_CLASS_KEY =
            TRACE_LISTENER_BASE+"/{0}/IpiTraceListener";

        public const string TRACE_SOURCE = "IPIHealthPerformanceTrace";
        public const string SOURCE_SWITCH = "SourceSwitch";
        public const string VERBOSE = "Verbose";
        public const string TRACE_LISTENER_ASSEM = "traceListenerAssem";
        public const string TRACE_LISTENER_CLASS = "traceListenerClass";
        public const string MY_LISTENER = "myListener";

        /// <summary>
        /// enumeration of the criticality levels for TraceSource
        /// </summary>
        public enum Level
        {
            Critical = 10000000,
            Error = 11000000,
            Warning = 12000000,
            Information = 13000000,
            Activity = 14000000,
            Verbose = 15000000
        }

        private TraceSource mTraceSource = null;

        private static volatile HealthPerformanceTrace mHPTrace = null;

        /// <summary>
        /// synchronization object for thread safety.
        /// </summary>
        private static object MSynchronizeRoot = new Object();

        /// <summary>
        /// public static Instance of HealthPerformanceTrace makes this a sigleton.
        /// </summary>
        public static HealthPerformanceTrace Instance 
        {
            get
            {
                // We don't want the overhead of locking
                // each time we try to get the object.
                // Hence check for null first, if not null
                // then just return the object.
                if (null == mHPTrace)
                {
                    // Use a MSynchronizeRoot instance to lock on, 
                    // rather than locking on the type 
                    // itself, to avoid deadlocks
                    lock (MSynchronizeRoot)
                    {
                        if (null == mHPTrace)
                        {
                            mHPTrace = new HealthPerformanceTrace();

                            // Initialization code separated to avoid infinite loop that
                            // occurs if ConfigXml calls fail
                            mHPTrace.Initialize();
                        }
                    }
                }
                return mHPTrace;
            }
        }

        /// <summary>
        /// The native TraceSource object wrapped by the instance of TraceSource.
        /// </summary>
        public TraceSource TraceSource
        {
            get { return mTraceSource; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private HealthPerformanceTrace()
        {
            mTraceSource = new TraceSource(TRACE_SOURCE);
            SourceSwitch sourceSwitch = new SourceSwitch(SOURCE_SWITCH, VERBOSE);
            mTraceSource.Switch = sourceSwitch;

            mTraceSource.Listeners.Clear();
        }

        /// <summary>
        /// Handles initialization after constructor is finished.  If the ConfigXml calls
        /// are in the constructor and fail, they call ErrorTrace and create an infinite loop.
        /// </summary>
        private void Initialize()
        {
            //Pull Configuration Settings For TraceListeners
            // RTB - not what we want -- List<string> listenerList = ConfigXml.Instance.GetAsStringList(TRACE_LISTENER_BASE);
            List<string> listenerList = ConfigXml.Instance.GetAllChildNodeNames(TRACE_LISTENER_BASE);
            foreach(string listener in listenerList)
            {
                try
                {
                    string traceListenerAssem = ConfigXml.Instance.GetAsString(String.Format(TRACE_LISTENER_ASSEMBLY_KEY, listener));
                    IpiAssert.IsNotNull(traceListenerAssem, TRACE_LISTENER_ASSEM);

                    string traceListenerClass = ConfigXml.Instance.GetAsString(String.Format(TRACE_LISTENER_CLASS_KEY, listener));
                    IpiAssert.IsNotNull(traceListenerClass, TRACE_LISTENER_CLASS);

                    TraceListener myListener = HealthPerformanceTraceListenerFactory.CreateListener(traceListenerAssem, traceListenerClass);
                    IpiAssert.IsNotNull(myListener, MY_LISTENER);

                    mTraceSource.Listeners.Add(myListener);
                }
                catch (Exception ex)
                {
                    EventLog exceptionLog = new EventLog();
                    exceptionLog.Source = "InformaticsWeb";
                    exceptionLog.WriteEntry(string.Format("Exception in HealthPerformanceTrace.Initialize(): {0} {1} {2}", ex.Message, ex.InnerException, ex.StackTrace));
                }
            }
        }

        /// <summary>
        /// The native TraceSource object wrapped by the instance of TraceSource.
        /// </summary>
        /// <param name="healthPerformanceTraceBase">any Trace object derived from HealthPerformanceTraceBase</param>
        /// <returns>the comma delimited string of the values in the Trace object</returns>
        public void TraceData(HealthPerformanceTraceBase healthPerformanceTraceBase)
        {
            string traceData;
            StringBuilder sb = new StringBuilder();

            try
            {
                TraceEventType traceEventType;
                int level;

                switch (healthPerformanceTraceBase.EventStatus)
                {
                    case  EventStatusType.Info :
                        traceEventType = TraceEventType.Information;
                        level = (int) Level.Information;
                        break;
                    case  EventStatusType.Warning :
                        traceEventType = TraceEventType.Warning;
                        level = (int) Level.Warning;
                        break;
                    case  EventStatusType.Error :
                        traceEventType = TraceEventType.Error;
                        level = (int) Level.Error;
                        break;
                    case  EventStatusType.Critical :
                        traceEventType = TraceEventType.Critical;
                        level = (int) Level.Critical;
                        break;
                    default :
                        traceEventType = TraceEventType.Information;
                        level = (int) Level.Information;
                        break;
                }

                mTraceSource.TraceData(traceEventType, level, healthPerformanceTraceBase);
            }
            catch (Exception ex)
            {
                traceData = ex.Message;
                EventLog exceptionLog = new EventLog();
                exceptionLog.Source = "InformaticsWeb";
                exceptionLog.WriteEntry(string.Format("Exception in HealthPerformanceTrace.TraceData(): {0} {1} {2}", ex.Message, ex.InnerException, ex.StackTrace));
            }

        }

    }
}
