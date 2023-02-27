// ------------------------------------------------------------------------------
//                    Copyright (c) 2022 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------
namespace Covidien.CGRS.ESS
{
    using System.Diagnostics;
    using System.Windows.Forms;
    using Covidien.CGRS.PcAgentInterfaceBusiness;
    using Serilog;

    public class AuthenticationService
    {
        public enum CREDENTIAL_STATUS { APPROVED, DENIED, REQUIRED, UNKNOWN };

        private static AuthenticationService MInstance = null;
        private InterfaceDelegates.AuthenticationResults UIAuthenticationResultsDelegate;
        private Form LoginForm;

        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns></returns>
        public static AuthenticationService Instance() 
        {
            if( MInstance == null )
            {
                MInstance = new AuthenticationService();
            }
            return MInstance;
        }       

        /// <summary>
        /// AuthenticateUser()
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="loginForm"></param>
        /// <param name="callback"></param>
        public void AuthenticateUser(string userName, string password, Form loginForm, InterfaceDelegates.AuthenticationResults callback)
        {
            LoginForm = loginForm;
            UIAuthenticationResultsDelegate = new InterfaceDelegates.AuthenticationResults(callback);

            var Resp = BusinessServicesBridge.Instance.AuthorizeToServerUsingOfflinePasscode(userName, password);
            if (Resp != null && Resp.Result.Success)
            {
                ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.USER_AUTHENTICATED);
            }
            else
            {
                ProcessAuthenticationResults(false, Resp?.Result?.ErrMsg);
            }
        }

        /// <summary>
        /// AuthenticateUser()
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="loginForm"></param>
        /// <param name="callback"></param>
        public void AuthorizeToServerUsingToken(string accessToken, string refreshToken, string username)
        {
            Log.Information($"AddDeviceForm::AuthorizeToServerUsingToken entry accessToken:{accessToken} refreshToken: {refreshToken} username: {username}");
            var Resp = BusinessServicesBridge.Instance.AuthorizeToServerUsingToken(accessToken, refreshToken, username);
            
            if(Resp != null && Resp.Result.Success)
            {
                var eventId = (Resp.IsPasscodeSet == false) ? ESS_Main.StateChangeEvent.REQUEST_OFFLINE_CODE : ESS_Main.StateChangeEvent.USER_AUTHENTICATED;
                ESS_Main.Instance.GenerateStateChangeEvent(eventId);
            }
            else
            {
                var Text = Resp?.Result?.ErrMsg != ""? Resp.Result.ErrMsg: Properties.Resources.AUTH_INVALID_OR_CANCEL;
                Log.Error($"AddDeviceForm::AuthorizeToServerUsingToken Either User Cancelled or Invalid Authorization accessToken:{accessToken} refreshToken: {refreshToken} username: {username}");
                MessageBox.Show(Text);
                ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.USER_AUTHENTICATION_FAILED);
            }
            Log.Information($"AddDeviceForm::AuthorizeToServerUsingToken Exit accessToken:{accessToken} refreshToken: {refreshToken} username: {username}");

        }

        /// <summary>
        /// sets device country code to the user specified valued
        /// </summary>
        /// <param name="countryCode"></param>
        public void SetDeviceCountryCode(string countryCode)
        {
            BusinessServicesBridge.Instance.SetDeviceCountryCode(countryCode);
            return;
        }

        /// <summary>
        /// CancelAuthenticationRequest()
        /// </summary>
        public void CancelAuthenticationRequest()
        {
        }

        /// <summary>
        /// ProcessAuthenticationResults()
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="result"></param>
        /// <param name="sessionId"></param>
        private void ProcessAuthenticationResults(bool result, string reason)
        {
            Log.Information($"AddDeviceForm::ProcessAuthenticationResults Entry InvokeRequired:{LoginForm.InvokeRequired}");

            if (LoginForm.InvokeRequired)
            {
                LoginForm.Invoke(UIAuthenticationResultsDelegate, result,reason);
            }
            else
            {
                UIAuthenticationResultsDelegate(result,reason);
            }
            Log.Information($"AddDeviceForm::ProcessAuthenticationResults Exit InvokeRequired:{LoginForm.InvokeRequired}");

        }

        public bool IsCovidienUser()
        {
            return BusinessServicesBridge.Instance.IsCovidienUser();
        }

        public string GetUserCountryCode()
        {
            return BusinessServicesBridge.Instance.GetUserCountryCode();
        }

        public CREDENTIAL_STATUS GetLogUploadCredentials()
        {
            RestrictedFunctionManager.RestrictionAlleviation status =
                BusinessServicesBridge.Instance.IsFunctionRestricted(RestrictedFunctionManager.RestrictedFunctions.LOG_UPLOAD);
            return TranslateRestrictionAlleviation(status);
        }

        public CREDENTIAL_STATUS GetLogAccessCredentials()
        {
            RestrictedFunctionManager.RestrictionAlleviation status = 
                BusinessServicesBridge.Instance.IsFunctionRestricted(RestrictedFunctionManager.RestrictedFunctions.LOG_VIEW);
            return TranslateRestrictionAlleviation(status);
        }

        public CREDENTIAL_STATUS GetSoftwareUploadCredentials()
        {
            RestrictedFunctionManager.RestrictionAlleviation status =
                BusinessServicesBridge.Instance.IsFunctionRestricted(RestrictedFunctionManager.RestrictedFunctions.SOFTWARE_DOWNLOAD);
            return TranslateRestrictionAlleviation(status);
        }

        public CREDENTIAL_STATUS GetBrowseToSelectSoftwareCredentials()
        {
            RestrictedFunctionManager.RestrictionAlleviation status =
                BusinessServicesBridge.Instance.IsFunctionRestricted(RestrictedFunctionManager.RestrictedFunctions.BROWSE_TO_SELECT_DOWNLOAD_PACKAGE);
            return TranslateRestrictionAlleviation(status);
        }

        public CREDENTIAL_STATUS GetCredentialsForFunction(RestrictedFunctionManager.RestrictedFunctions functionId)
        {
            RestrictedFunctionManager.RestrictionAlleviation status =
                BusinessServicesBridge.Instance.IsFunctionRestricted(functionId);
            return TranslateRestrictionAlleviation(status);
        }

        public CREDENTIAL_STATUS GetServerAccessCredentials()
        {
            CREDENTIAL_STATUS status;
            if (BusinessServicesBridge.Instance.IsUserLoggedIn() == false)
            {
                status = (LoginControl.GetAccess() == true) ? CREDENTIAL_STATUS.APPROVED : CREDENTIAL_STATUS.DENIED;
            }
            else
                status = CREDENTIAL_STATUS.APPROVED;

            return status;
        }

        private CREDENTIAL_STATUS TranslateRestrictionAlleviation(RestrictedFunctionManager.RestrictionAlleviation status)
        {
            CREDENTIAL_STATUS retVal = CREDENTIAL_STATUS.UNKNOWN; ;
            Log.Information($"AddDeviceForm::TranslateRestrictionAlleviation Entry status:{status}");

            switch ( status )
            {
                case RestrictedFunctionManager.RestrictionAlleviation.ACCESS_DENIED:
                    retVal = CREDENTIAL_STATUS.DENIED;
                    break;

                case RestrictedFunctionManager.RestrictionAlleviation.DEVICE_CHECK_REQUIRED:
                    retVal = CREDENTIAL_STATUS.UNKNOWN;
                    break;

                case RestrictedFunctionManager.RestrictionAlleviation.LOGIN_OVERRIDE_REQUIRED:
                        retVal = (LoginControl.GetAccess() == true) ? CREDENTIAL_STATUS.APPROVED : CREDENTIAL_STATUS.DENIED;
                    break;

                case RestrictedFunctionManager.RestrictionAlleviation.NO_RESTRICTIONS:
                    retVal = CREDENTIAL_STATUS.APPROVED;
                    break;

                default:
                    Debug.Assert(false);
                    break;
            }
            Log.Information($"AddDeviceForm::TranslateRestrictionAlleviation Exit status:{status}");
            return retVal;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AuthenticationService()
        {
        }
    }
}
