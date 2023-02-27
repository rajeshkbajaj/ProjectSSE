//*
//*  File:            HealthPerformanceTraceListenerFactory.cs
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
using System.Text;
using System.Reflection;

namespace Covidien.Ipi.InformaticsCore
{
    class HealthPerformanceTraceListenerFactory
    {
        /// <summary>
        /// Builds the selected log writers.
        /// </summary>
        /// <param name="pDllName">DLL to load dynamically</param>
        /// <param name="pClassName">class name of the Listener</param>
        /// <returns></returns>
        public static TraceListener CreateListener(string pDllName, string pClassName)
        {
            TraceListener listener = null;
            // dynamically load assembly from file HardwareSimulators.dll
            try
            {
                AssemblyName assemblyName = new AssemblyName();
                assemblyName.Name = pDllName;

                // Load the named assembly and get the types it supports.
                Assembly traceListenerAssembly = Assembly.Load(assemblyName.FullName);
                IpiAssert.IsNotNull(traceListenerAssembly, "traceListenerAssembly");

                // Create the Log Writer object using reflection.
                listener = (TraceListener)traceListenerAssembly.CreateInstance(pClassName);
            }
            catch (Exception)
            {
                System.Console.WriteLine("Unable to create trace listener.  Logging may be disabled.");
                throw ;
            }

            return listener;
        }
    }
}
