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
    using System;
    using System.Linq;
    using IdentityModel.Client;
    using IdentityModel.OidcClient;
    using Oasis.Agent.Models;
    using Serilog;
    using System.Windows.Forms;

    public class OAuth2LoginManager : Form
    {
        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns>Instance</returns>
        public static OAuth2LoginManager Instance()
        {
            if (MInstance == null)
            {
                MInstance = new OAuth2LoginManager();
            }
            return MInstance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticationServerInfo"></param>
        /// <param name="isServerAvialable"></param>
        /// <param name="userType"></param>
        public void Login(AuthenticationServerInfo authenticationServerInfo, bool isServerAvialable, ESS_Main.UserType userType)
        {
            if (isServerAvialable)
            {
                if (userType == ESS_Main.UserType.USER_TYPE_MEDTRONIC)
                {
                    var clientSecret = "";// "IPg8Q~L-83W7v7tNdlFRd6yJGiaBMtjp-gyoAc0Y";
                    Log.Information($"OAuth2LoginManager::Login Online Employee URL:{authenticationServerInfo.IAMServerUrl}, Id:{authenticationServerInfo.IAMClientId}, cb:{authenticationServerInfo.CallbackUrl}, sec:{clientSecret}");
                    UserLoginOnlineMode(authenticationServerInfo.IAMServerUrl,
                                    authenticationServerInfo.IAMClientId,
                                    clientSecret,
                                    authenticationServerInfo.CallbackUrl);
                }
                else if (userType == ESS_Main.UserType.USER_TYPE_NONMEDTRONIC)
                {
                    var clientSecret = "";
                    Log.Information($"OAuth2LoginManager::Login Non-Employee Online URL:{authenticationServerInfo.CIAMServerUrl}, Id:{authenticationServerInfo.CIAMClientId}, cb:{authenticationServerInfo.CallbackUrl}, sec:{clientSecret}");
                    UserLoginOnlineMode(authenticationServerInfo.CIAMServerUrl,
                                    authenticationServerInfo.CIAMClientId,
                                    clientSecret,
                                    authenticationServerInfo.CallbackUrl);
                }
                else
                {
                    // Not a valid state..
                    Log.Error($"OAuth2LoginManager::Login Invalid Usertype:{userType}");
                    ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.USER_AUTHENTICATION_FAILED);
                }
            }
            else
            {
                // Not a valid state..
                Log.Error($"OAuth2LoginManager::Login Server Not Available");
                ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.USER_AUTHENTICATION_FAILED);
            }
        }


        /// <summary>
        /// UserLoginOnlineMode
        /// </summary>
        private async void UserLoginOnlineMode(string loginUrl, string clientId, string clientSecret, string redirectUri)
        {
            var options = new OidcClientOptions()
            {
                Authority = loginUrl, 
                ClientId = clientId,
                Scope = "openid profile email offline_access",
                RedirectUri = redirectUri,
                ClientSecret = clientSecret,
                Browser = new WpfEmbeddedBrowser(),
                Policy = new Policy
                {
                    RequireIdentityTokenSignature = false,
                    Discovery = new DiscoveryPolicy() { ValidateEndpoints = false }
                }
            };

            try
            {
                var _oidcClient = new OidcClient(options);
                var loginResult = await _oidcClient.LoginAsync();                
                if (loginResult.IsError)
                {
                    Log.Error("OAuth2LoginManager::UserLoginOnlineMode, Error in Login");
                    //if (loginResult.Error != ESS.Properties.Resources.AUTH_RESPONSE_USER_CANCEL)
                    //{ MessageBox.Show(ESS.Properties.Resources.LOGIN_FAILED_RELOGIN); }

                    ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.USER_AUTHENTICATION_FAILED);
                }
                else
                {
                    var userEmail = loginResult.User.Claims.Count() > 0 ? loginResult.User.Claims.FirstOrDefault(x => x.Type == "email")?.Value : "";

                    Log.Information($"OAuth2LoginManager::UserLoginOnlineMode userEmail: {userEmail}");
                    AuthenticationService.Instance().AuthorizeToServerUsingToken(loginResult.AccessToken,
                                                                    loginResult.RefreshToken, userEmail);
                }
            }
            catch (Exception ex)
            {
                Log.Error( $"OAuth2LoginManager::UserLoginOnlineMode Unexpected Error Exception: {ex.Message}");
                //MessageBox.Show(ESS.Properties.Resources.LOGIN_FAILED_RELOGIN);
                ESS_Main.Instance.GenerateStateChangeEvent(ESS_Main.StateChangeEvent.USER_AUTHENTICATION_FAILED);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private OAuth2LoginManager() { }
        private static OAuth2LoginManager MInstance;  
    }
}
