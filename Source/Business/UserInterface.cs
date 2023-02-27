using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PCAgentInterface.Messages;
using PCAgentInterface.Messages.Interfaces;

namespace Covidien.CGRS.PcAgentInterfaceBusiness
{
    public class UserInterface
    {
        // RSA related variables
        private string mSessionId = null;
        private string mUserId = null;
        private string mUserPswd = null;


        /// <summary>
        /// These calls (container actually) must be set before taking any other action
        /// Because of a mutual dependency (at least for now), it is not part of the ctor().
        /// </summary>
        public RsaInterface RsaIFace { get; set; }

        public UserInterface()
        {
            ConnectCallback = null;
            DisconnectCallback = null;
            InternalMessageCallback = null;
        }



        /// <summary>
        /// The delegate to be invoked when a connection has occurred.  Pass "null" to clear it.
        /// Recommend setting this AFTER the initial connection has been performed.  Its value
        /// is when we try to automatically reconnect after having encountered a disconnection.
        /// </summary>
        public UserInterfaceDelegates.ConnectCallback ConnectCallback { get; set; }

        /// <summary>
        /// The delegate to be invoked when a connection has occurred.  Pass "null" to clear it.
        /// </summary>
        public UserInterfaceDelegates.DisconnectCallback DisconnectCallback { get; set; }

        /// <summary>
        /// The delegate to be invoked when a connection has occurred.  Pass "null" to clear it.
        /// </summary>
        public UserInterfaceDelegates.InternalMessageCallback InternalMessageCallback { get; set; }



        private Hashtable mCallbacks = new Hashtable();
        private Hashtable mResponses = new Hashtable();



        public void HandleRsaResponse( IResponse response )
        {
            if  ( mCallbacks.ContainsKey( response.TransactionGuid ) )
            {
                object callback = mCallbacks[ response.TransactionGuid ] ;
                mCallbacks.Remove( response.TransactionGuid ) ;


                if  ( response.IsHeaders() )
                {
                    //
                }
                else if  ( response.IsNotifications() )
                {
                    
                }
                else  // if  ( response.IsParameters() )
                {
                    string typeStr = callback.GetType().ToString();
                    switch (typeStr)
                    {
                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+SessionCreateCallback" :
                            ((UserInterfaceDelegates.SessionCreateCallback)callback)( response.TransactionGuid, response.IsOk(), response.SessionGuid ) ;
                            break;
                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+SessionOpenCallback":
                            ((UserInterfaceDelegates.SessionOpenCallback)callback)( response.TransactionGuid, response.IsOk(), response.SessionGuid ) ;
                            break;
                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+SessionCloseCallback":
                            ((UserInterfaceDelegates.SessionCloseCallback)callback)( response.TransactionGuid, response.IsOk(), response.SessionGuid ) ;
                            break;

                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+LoginCallback":
                            ((UserInterfaceDelegates.LoginCallback)callback)( response.TransactionGuid, response.IsOk(), response.SessionGuid ) ;
                            break;
                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+LogoutCallback":
                            ((UserInterfaceDelegates.LogoutCallback)callback)( response.TransactionGuid, response.IsOk(), response.SessionGuid ) ;
                            break;

                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+GetDevicesCallback":
                            ((UserInterfaceDelegates.GetDevicesCallback)callback)( response.TransactionGuid, response.IsOk(), response.SessionGuid, null ) ;  // response.GetParameters() );
                            break;
                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+GetDeviceStatusCallback":
                            ((UserInterfaceDelegates.GetDeviceStatusCallback)callback)(response.TransactionGuid, response.IsOk(), response.SessionGuid, null ) ;  // response.GetParameters().Get( "status" ) ) ;
                            break;

                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+GetHeadersCallback":
                            ((UserInterfaceDelegates.GetHeadersCallback)callback)(response.TransactionGuid, response.IsOk(), response.SessionGuid, response.GetHeaders().NotificationsList ) ;
                            break;

                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+GetNotificationCallback":
                            ((UserInterfaceDelegates.GetNotificationCallback)callback)(response.TransactionGuid, response.IsOk(), response.SessionGuid, response.GetNotifications().NotificationsList ) ;
                            break;
                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+PostNotificationCallback":
                            ((UserInterfaceDelegates.PostNotificationCallback)callback)(response.TransactionGuid, response.IsOk(), response.SessionGuid);
                            break;
                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+DeleteNotificationCallback":
                            ((UserInterfaceDelegates.DeleteNotificationCallback)callback)(response.TransactionGuid, response.IsOk(), response.SessionGuid);
                            break;
                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+UndeleteNotificationCallback":
                            ((UserInterfaceDelegates.UndeleteNotificationCallback)callback)(response.TransactionGuid, response.IsOk(), response.SessionGuid);
                            break;
                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+UpdateNotificationCallback":
                            ((UserInterfaceDelegates.UpdateNotificationCallback)callback)(response.TransactionGuid, response.IsOk(), response.SessionGuid);
                            break;
                        case "Covidien.CGRS.PcAgentInterfaceBusiness.UserInterfaceDelegates+ExpungeNotificationCallback":
                            ((UserInterfaceDelegates.ExpungeNotificationCallback)callback)(response.TransactionGuid, response.IsOk(), response.SessionGuid);
                            break;

                        default:
                            InternalMessageCallback( "HandleRsaResponse", "UNHANDLED Message: " + response.Serialize() ) ;
                            break;
                    }
                }
            }
            else
            {
                mResponses.Add( response.TransactionGuid, response ) ;
            }
        }


        public void HandleRsaRequest( IRequest request )
        {
            // No current expectation that we will be getting requests from the RSA, but have put placeholder code at the ready.

            InternalMessageCallback( "HandleRsaRequest", "UNHANDLED Message: " + request.ToString() ) ;

        }



        public string CreateSession()
        {
            IRequestSessionCreate nRequest = new RequestSessionCreate();

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            return( transactionGuid ) ;
        }

        public string CreateSession( Delegate sessionCreateCallback )
        {
            IRequestSessionCreate nRequest = new RequestSessionCreate();

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            mCallbacks[transactionGuid] = sessionCreateCallback;

            return( transactionGuid ) ;
        }

        public string GetCreatedSessionResult( string transactionId )
        {
            if  ( mResponses.ContainsKey( transactionId ) )
            {
                string retval = (string) mResponses[ transactionId ] ;
                mResponses.Remove( transactionId ) ;
                return( retval ) ;
            }
            else
            {
                return( null ) ;
            }
        }


        public string OpenSession( string sessionId )
        {
            IRequestSessionOpen nRequest = new RequestSessionOpen( sessionId ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            return( transactionGuid ) ;
        }

        public string OpenSession( string sessionId, Delegate sessionOpenCallback )
        {
            IRequestSessionCreate nRequest = new RequestSessionCreate();

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            mCallbacks[transactionGuid] = sessionOpenCallback;

            return( transactionGuid ) ;
        }

        public string GetOpenedSessionResult( string transactionId )
        {
            if  ( mResponses.ContainsKey( transactionId ) )
            {
                string retval = (string) mResponses[ transactionId ] ;
                mResponses.Remove( transactionId ) ;
                return( retval ) ;
            }
            else
            {
                return( null ) ;
            }
        }


        public string CloseSession( string sessionId )
        {
            IRequestSessionOpen nRequest = new RequestSessionOpen( sessionId ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            return (transactionGuid);
        }

        public string CloseSession( string sessionId, Delegate sessionCloseCallback )
        {
            IRequestSessionCreate nRequest = new RequestSessionCreate();

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            mCallbacks[transactionGuid] = sessionCloseCallback;

            return( transactionGuid ) ;
        }

        public string GetClosedSessionResult( string transactionId )
        {
            if  ( mResponses.ContainsKey( transactionId ) )
            {
                string retval = (string) mResponses[ transactionId ] ;
                mResponses.Remove( transactionId ) ;
                return( retval ) ;
            }
            else
            {
                return( null ) ;
            }
        }



        /// <summary>
        /// Call from User Interface making a login request.
        /// </summary>
        /// <param name="userId">The User Login Id</param>
        /// <param name="password">The associated password</param>
        /// <returns>Transaction ID by which to later poll for a response.</returns>
        public string Login( string userId, string password )
        {
            mUserId = userId ;
            mUserPswd = password ;
            IRequestLogOn nRequest = new RequestLogOn( mSessionId, mUserId, mUserPswd ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            return( transactionGuid ) ;
        }

        /// <summary>
        /// Call from User Interface making a login request.
        /// </summary>
        /// <param name="userId">The User Login Id</param>
        /// <param name="password">The associated password</param>
        /// <returns>Transaction ID by which to later poll for a response.</returns>
        public string Login( string userId, string password, Delegate loginCallback )
        {
            mUserId = userId ;
            mUserPswd = password ;
            IRequestLogOn nRequest = new RequestLogOn( mSessionId, mUserId, mUserPswd ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            mCallbacks[transactionGuid] = loginCallback;

            return( nRequest.TransactionGuid ) ;
        }

        /// <summary>
        /// Gets the result of the login if no callback was given.  Returns null if no result yet, or result already read.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public string GetLoginResult( string transactionId )
        {
            if  ( mResponses.ContainsKey( transactionId ) )
            {
                string retval = (string) mResponses[ transactionId ] ;
                mResponses.Remove( transactionId ) ;
                return( retval ) ;
            }
            else
            {
                return( null ) ;
            }
        }




        /// <summary>
        /// Call from User Interface making a request to get devices.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns>Transaction ID by which to later poll for a response.</returns>
        public string GetDevices( List<string> filters )
        {
            IRequestDeviceListGet nRequest = new RequestDeviceListGet( mSessionId ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            return( transactionGuid ) ;
        }

        public string GetDevices( List<string> filters, Delegate getDevicesCallback )
        {
            IRequestDeviceListGet nRequest = new RequestDeviceListGet( mSessionId ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            mCallbacks[transactionGuid] = getDevicesCallback;

            return( nRequest.TransactionGuid ) ;
        }

        /// <summary>
        /// Call from User Interface polling to determine if a response is ready to prior request to GetDevices().
        /// </summary>
        /// <param name="transactionId">Transaction ID returned from an earlier call to overloaded GetDevices()</param>
        /// <returns></returns>
        public List<string> GetGetDevicesResult( string transactionId )
        {
            if  ( mResponses.ContainsKey( transactionId ) )
            {
                List<string> retval = (List<string>) mResponses[ transactionId ] ;
                mResponses.Remove( transactionId ) ;
                return( retval ) ;
            }
            else
            {
                return( null ) ;
            }
        }






        /// <summary>
        /// Call from User Interface making a request to get devices.
        /// </summary>
        /// <param name="filters"></param>
        /// <returns>Transaction ID by which to later poll for a response.</returns>
        public string GetHeaders( List<string> filters )
        {
            return( GetHeaders( filters, null ) ) ;
        }

        public string GetHeaders( List<string> filters, Delegate getHeadersCallback )
        {
            IRequestHeadersGet nRequest = new RequestHeadersGet( mSessionId, "Ventilator", null, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            if  ( null != getHeadersCallback )
            {
                mCallbacks[transactionGuid] = getHeadersCallback;
            }
            return( nRequest.TransactionGuid ) ;
        }

        public List<string> GetGetHeadersResult( string transactionId )
        {
            if  ( mResponses.ContainsKey( transactionId ) )
            {
                List<string> retval = (List<string>) mResponses[ transactionId ] ;
                mResponses.Remove( transactionId ) ;
                return( retval ) ;
            }
            else
            {
                return( null ) ;
            }
        }



        /// <summary>
        /// Call from User Interface polling to determine if a response is ready to prior request to GetDevices().
        /// </summary>
        /// <param name="transactionId">Transaction ID returned from an earlier call to overloaded GetDevices()</param>
        /// <returns></returns>
        public string GetNotification( string transactionId )
        {
            IRequestNotification nRequest = new RequestNotificationGet( mSessionId, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            return( transactionGuid ) ;
        }

        public string GetNotification( string transactionId, Delegate getNotificationCallback )
        {
            IRequestNotification nRequest = new RequestNotificationGet( mSessionId, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            mCallbacks[transactionGuid] = getNotificationCallback;

            return( nRequest.TransactionGuid ) ;
        }

        public List<string> GetGetNotificationResult( string transactionId )
        {
            if  ( mResponses.ContainsKey( transactionId ) )
            {
                List<string> retval = (List<string>) mResponses[ transactionId ] ;
                mResponses.Remove( transactionId ) ;
                return( retval ) ;
            }
            else
            {
                return( null ) ;
            }
        }

        /*
        public string PostNotification( string transactionId )
        {
            IRequestNotificationPost nRequest = new RequestNotificationPost( mSessionId, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            return( transactionGuid ) ;
        }

        public string PostNotification( string transactionId, Delegate postNotificationCallback )
        {
            IRequestNotificationPost nRequest = new RequestNotificationPost( mSessionId, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            mCallbacks[transactionGuid] = postNotificationCallback;

            return( nRequest.TransactionGuid ) ;
        }

        public List<string> GetPostNotificationResult( string transactionId )
        {
            if  ( mResponses.ContainsKey( transactionId ) )
            {
                List<string> retval = (List<string>) mResponses[ transactionId ] ;
                mResponses.Remove( transactionId ) ;
                return( retval ) ;
            }
            else
            {
                return( null ) ;
            }
        }
        */


        public string DeleteNotification( string transactionId )
        {
            IRequestNotificationDelete nRequest = new RequestNotificationDelete( mSessionId, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            return( transactionGuid ) ;
        }

        public string DeleteNotification( string transactionId, Delegate deleteNotificationCallback )
        {
            IRequestNotificationDelete nRequest = new RequestNotificationDelete( mSessionId, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            mCallbacks[transactionGuid] = deleteNotificationCallback;

            return( nRequest.TransactionGuid ) ;
        }

        public List<string> GetDeleteNotificationResult( string transactionId )
        {
            if  ( mResponses.ContainsKey( transactionId ) )
            {
                List<string> retval = (List<string>) mResponses[ transactionId ] ;
                mResponses.Remove( transactionId ) ;
                return( retval ) ;
            }
            else
            {
                return( null ) ;
            }
        }



        public string UndeleteNotification( string transactionId )
        {
            IRequestNotificationUndelete nRequest = new RequestNotificationUndelete( mSessionId, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            return( transactionGuid ) ;
        }

        public string UndeleteNotification( string transactionId, Delegate undeleteNotificationCallback )
        {
            IRequestNotificationUndelete nRequest = new RequestNotificationUndelete( mSessionId, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            mCallbacks[transactionGuid] = undeleteNotificationCallback;

            return( nRequest.TransactionGuid ) ;
        }

        public List<string> GetUndeleteNotificationResult( string transactionId )
        {
            if  ( mResponses.ContainsKey( transactionId ) )
            {
                List<string> retval = (List<string>) mResponses[ transactionId ] ;
                mResponses.Remove( transactionId ) ;
                return( retval ) ;
            }
            else
            {
                return( null ) ;
            }
        }




        public string UpdateNotification( string transactionId )
        {
            IRequestNotificationUpdate nRequest = new RequestNotificationUpdate( mSessionId, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            return( transactionGuid ) ;
        }

        public string UpdateNotification( string transactionId, Delegate updateNotificationCallback )
        {
            IRequestNotificationUpdate nRequest = new RequestNotificationUpdate( mSessionId, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            mCallbacks[transactionGuid] = updateNotificationCallback;

            return( nRequest.TransactionGuid ) ;
        }

        public List<string> GetUpdateNotificationResult( string transactionId )
        {
            if  ( mResponses.ContainsKey( transactionId ) )
            {
                List<string> retval = (List<string>) mResponses[ transactionId ] ;
                mResponses.Remove( transactionId ) ;
                return( retval ) ;
            }
            else
            {
                return( null ) ;
            }
        }



        public string ExpungeNotification( string transactionId )
        {
            IRequestNotificationExpunge nRequest = new RequestNotificationExpunge( mSessionId, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            return( transactionGuid ) ;
        }

        public string ExpungeNotification( string transactionId, Delegate expungeNotificationCallback )
        {
            IRequestNotificationExpunge nRequest = new RequestNotificationExpunge( mSessionId, null ) ;

            string inStr = nRequest.Serialize();

            RsaIFace.SendBuffer.Add(inStr);

            string transactionGuid = nRequest.TransactionGuid;
            mCallbacks[transactionGuid] = expungeNotificationCallback;

            return( nRequest.TransactionGuid ) ;
        }

        public List<string> GetExpungeNotificationResult( string transactionId )
        {
            if  ( mResponses.ContainsKey( transactionId ) )
            {
                List<string> retval = (List<string>) mResponses[ transactionId ] ;
                mResponses.Remove( transactionId ) ;
                return( retval ) ;
            }
            else
            {
                return( null ) ;
            }
        }
    }
}
