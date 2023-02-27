//*
//*  File:            IpiAssert.cs
//*  Product:         Integrated Patient Intelligence
//*  Module:          InformaticsCore
//*
//*  Description:     Provides a wrapper around Debug.Assert
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


using System.Diagnostics;

namespace Covidien.Ipi.InformaticsCore
{

    // The only thing this class has is a static method.
	public sealed class IpiAssert
	{
        /// <summary>
        /// Private constructor.
        /// </summary>
        private IpiAssert() { }
        
        /// <summary>
        /// This method logs the error and then calls Debug.Assert().
        /// </summary>
        /// <param name="bCondition"></param>
        /// <param name="sErrorMessage"></param>
        [Conditional("DEBUG")]
        public static void Fail(bool bCondition, string sErrorMessage)
        {
            if (!bCondition)
            {
                ErrorTrace.Trace(IpiEventCode.AssertFailError, sErrorMessage,
                                 EventStatusType.Error);
                // Call C# Assert
                Debug.Assert(bCondition, sErrorMessage);
            }
        }

        /// <summary>
        /// This method logs the error and then calls Debug.Assert().
        /// </summary>
        /// <param name="bCondition"></param>
        /// <param name="pErrorId"></param>
        [Conditional("DEBUG")]
        public static void Fail(bool bCondition, int pErrorId)
        {
            if (!bCondition)
            {
                ErrorTrace.Trace(IpiEventCode.AssertFailError, "Variable is false",
                                 EventStatusType.Error);
                // Call C# Assert
                Debug.Assert(bCondition, "");
            }
        }

        /// <summary>
        /// This method logs the error and then calls Debug.Assert().
        /// </summary>
        /// <param name="bCondition"></param>
        /// <param name="pErrorId"></param>
        /// <param name="pMsgParamater"></param>
        [Conditional("DEBUG")]
        public static void Fail(bool bCondition, int pErrorId, string pMsgParamater)
        {
            if (!bCondition)
            {
                ErrorTrace.Trace(IpiEventCode.AssertFailError, "Variable is false: " + pMsgParamater,
                                 EventStatusType.Error);
                // Call C# Assert
                Debug.Assert(bCondition, "");
            }
        }

        /// <summary>
        /// This method logs the error and then calls Debug.Assert().
        /// </summary>
        /// <param name="pObject"></param>
        /// <param name="pObjectName"></param>
        [Conditional("DEBUG")]
        public static void IsNotNull(object pObject, string pObjectName)
        {
            if (null == pObject)
            {
                ErrorTrace.Trace(IpiEventCode.AssertNotNullError, "Variable is null: " + pObjectName,
                                 EventStatusType.Error);
                // Call C# Assert
                Debug.Assert(null != pObject, "");
            }
        }

    }
}
