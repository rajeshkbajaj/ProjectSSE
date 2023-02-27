//*
//*  File:            IpiException.cs
//*  Product:         Integrated Patient Intelligence
//*  Module:          InformaticsCore
//*
//*  Description:     Provides a wrapper around exception
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

namespace Covidien.Ipi.InformaticsCore
{
    public class IpiException : Exception
    {
        // If you aren't going to record the exception (or at least be subject to our potentially recording it), then just create a regular exception
        //
        // RH: This implementation is left to allow code using it to continue to compile. If an Ipi exception is still required where this
        // implementation is currently used, then a specific exception (deriving from IpiApplicationException) should be implemented
        // in the appropriate namespace and this implementation removed.
        public IpiException( string msg )
            : base(msg)
        {
//            Console.WriteLine("IPI Exception: {0} to be replace by H&P Logger", msg);
            ErrorTrace.Trace(IpiEventCode.IpiBaseException, msg, EventStatusType.Error, this);
        }

        // As above, but expected to be called with traceIt=false as a means of avoiding duplicate trace messages
        // if the derived class is in fact creating its own trace
        public IpiException(string msg, bool traceIt )
            : base(msg)
        {
            if  ( traceIt )
                ErrorTrace.Trace(IpiEventCode.IpiBaseException, msg, EventStatusType.Error, this);
        }
    }

    /// <summary>
    /// Base class from which to derive specific Ipi exceptions. Whenever possible, create the
    /// derived exception in the namespace to which it relates. 
    /// For example, Ipi.DeviceManagement.DeviceNotFoundException
    /// </summary>
    [Serializable()]
    public abstract class IpiApplicationException : System.Exception
    {
        protected IpiApplicationException() { }
        protected IpiApplicationException(string message) : base(message) { }
        protected IpiApplicationException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization when exception propagates from a remoting server to the client.
        protected IpiApplicationException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Indicates that a duplicate definition is found in an IPI configuration.
    /// </summary>
    public class IpiDuplicateDefinitionException : IpiApplicationException
    {
        public IpiDuplicateDefinitionException() { }
        public IpiDuplicateDefinitionException(string message) : base(message) { }
        public IpiDuplicateDefinitionException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization when exception propagates from a remoting server to the client.
        protected IpiDuplicateDefinitionException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Indicates a failure in the creation of a device listener.
    /// </summary>
    public class IpiListenerCreationException : IpiApplicationException
    {
        public IpiListenerCreationException() { }
        public IpiListenerCreationException(string message) : base(message) { }
        public IpiListenerCreationException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization when exception propagates from a remoting server to the client.
        protected IpiListenerCreationException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Indicates that a specified port type could not be properly handled.
    /// </summary>
    public class IpiUnhandledPortTypeException : IpiApplicationException
    {
        public IpiUnhandledPortTypeException() { }
        public IpiUnhandledPortTypeException(string message) : base(message) { }
        public IpiUnhandledPortTypeException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization when exception propagates from a remoting server to the client.
        protected IpiUnhandledPortTypeException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }



    /// <summary>
    /// Indicates that configuration reading is not properly handled.
    /// </summary>
    public class IpiConfigurationEvaluationException : IpiApplicationException
    {
        public IpiConfigurationEvaluationException() { }
        public IpiConfigurationEvaluationException(string message) : base(message) { }
        public IpiConfigurationEvaluationException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization when exception propagates from a remoting server to the client.
        protected IpiConfigurationEvaluationException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


    /// <summary>
    /// Indicates that Protocol definition file is not found.
    /// </summary>
    public class IpiProtocolDefinitionNotFoundException : IpiApplicationException
    {
        public IpiProtocolDefinitionNotFoundException() { }
        public IpiProtocolDefinitionNotFoundException(string message) : base(message) { }
        public IpiProtocolDefinitionNotFoundException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization when exception propagates from a remoting server to the client.
        protected IpiProtocolDefinitionNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }



    /// <summary>
    /// Indicates that Protocol handler could not be created successfully
    /// </summary>
    public class IpiInvalidProtocolHandlerException : IpiApplicationException
    {
        public IpiInvalidProtocolHandlerException() { }
        public IpiInvalidProtocolHandlerException(string message) : base(message) { }
        public IpiInvalidProtocolHandlerException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization when exception propagates from a remoting server to the client.
        protected IpiInvalidProtocolHandlerException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


    /// <summary>
    /// Indicates that PortStream could not be created
    /// </summary>
    public class IpiPortStreamErrorException : IpiApplicationException
    {
        public IpiPortStreamErrorException() { }
        public IpiPortStreamErrorException(string message) : base(message) { }
        public IpiPortStreamErrorException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization when exception propagates from a remoting server to the client.
        protected IpiPortStreamErrorException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


   
    /// <summary>
    /// Indicates that there is an error in the manual device creator
    /// </summary>
    public class IpiManualDeviceCreatorErrorException : IpiApplicationException
    {
        public IpiManualDeviceCreatorErrorException() { }
        public IpiManualDeviceCreatorErrorException(string message) : base(message) { }
        public IpiManualDeviceCreatorErrorException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization when exception propagates from a remoting server to the client.
        protected IpiManualDeviceCreatorErrorException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }



    /// <summary>
    /// Indicates that there is an error in the manual device creator
    /// </summary>
    public class IpiManualDeviceShutdownException : IpiApplicationException
    {
        public IpiManualDeviceShutdownException() { }
        public IpiManualDeviceShutdownException(string message) : base(message) { }
        public IpiManualDeviceShutdownException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization when exception propagates from a remoting server to the client.
        protected IpiManualDeviceShutdownException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }



    /// <summary>
    /// Indicates that there is an error in the manual device creator
    /// </summary>
    public class IpiDeviceMonitoringSessionException : IpiApplicationException
    {
        public IpiDeviceMonitoringSessionException() { }
        public IpiDeviceMonitoringSessionException(string message) : base(message) { }
        public IpiDeviceMonitoringSessionException(string message, System.Exception inner) : base(message, inner) { }

        // Constructor needed for serialization when exception propagates from a remoting server to the client.
        protected IpiDeviceMonitoringSessionException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }





    //// SAMPLE OF HOW TO CREATE A NEW EXCEPTION
    //public class IpiDerivedException : IpiApplicationException
    //{
    //    public IpiDerivedException() { }
    //    public IpiDerivedException(string message) : base(message) { }
    //    public IpiDerivedException(string message, System.Exception inner) : base(message, inner) { }

    //    // Constructor needed for serialization when exception propagates from a remoting server to the client.
    //    protected IpiDerivedException(System.Runtime.Serialization.SerializationInfo info,
    //        System.Runtime.Serialization.StreamingContext context): base(info, context)  { }
    //}
}
