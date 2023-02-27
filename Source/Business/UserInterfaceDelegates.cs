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
    using System.Collections.Generic;
    using Oasis.Agent;
    using Oasis.Agent.Models;

    public enum RESPONSE_STATUS
    {
        RESPONSE_OK,
        RESPONSE_BAD
    };

    public class UserInterfaceDelegates
    {
        /// <summary>
        /// Callback delegate invoked in response to a successful connection
        /// </summary>
        public delegate void ConnectCallback(bool actionSuccessful) ;

        /// <summary>
        ///  Callback delegate invoked in response to a disconnect
        /// </summary>
        public delegate void DisconnectCallback();

        /// <summary>
        /// (Internal) callback delegate invoked in response to messages we might generate (during initial debugging mainly/only)
        /// </summary>
        /// <param name="originator"></param>
        /// <param name="msg"></param>
        public delegate void InternalMessageCallback( string originator, string msg ) ;



        /// <summary>
        /// (Internal) callback delegate invoked to provide status to the user (re the RSA)
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        public delegate void AddServerStatusMessage( string tag, string message ) ;

        /// <summary>
        /// (Internal) callback delegate invoked in remove a status previously provided
        /// </summary>
        /// <param name="tag"></param>
        public delegate void RemoveServerStatusMessage( string tag ) ;

        /// <summary>
        /// (Internal) callback delegate invoked to provide status to the user (re the device)
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="message"></param>
        public delegate void AddDeviceStatusMessage( string tag, string message ) ;

        /// <summary>
        /// (Internal) callback delegate invoked in remove a status previously provided
        /// </summary>
        /// <param name="tag"></param>
        public delegate void RemoveDeviceStatusMessage( string tag ) ;




        /// <summary>
        /// Callback delegate invoked in response to an overloaded CreateSession() request
        /// </summary>
        /// <param name="actionSuccessful"></param>
        public delegate void SessionCreateCallback( string transactionId, bool actionSuccessful, string sessionId ) ;

        /// <summary>
        /// Callback delegate invoked in response to an overloaded OpenSession() request
        /// </summary>
        /// <param name="actionSuccessful"></param>
        public delegate void SessionOpenCallback( string transactionId, bool actionSuccessful, string sessionId ) ;

        /// <summary>
        /// Callback delegate invoked in response to an overloaded CloseSession() request
        /// </summary>
        /// <param name="actionSuccessful"></param>
        public delegate void SessionCloseCallback( string transactionId, bool actionSuccessful, string sessionId ) ;

        /// <summary>
        /// Callback delegate invoked in response to an overloaded GetDeviceStatus() request
        /// </summary>
        /// <param name="response">The response status</param>
        /// <param name="deviceStatus">Device Status information in form of key,value pairs</param>
        public delegate void GetDeviceStatusCallback( string transactionId, bool actionSuccessful, string sessionId, DeviceInformation deviceInformation ) ;


        /// <summary>
        /// Callback delegate invoked in response to an overloaded GetDevices() request
        /// </summary>
        /// <param name="response">The response status</param>
        /// <param name="status">List of devices</param>
        public delegate void GetRsaStatusCallback( RsaStatus status ) ;


        /// <summary>
        /// Callback delegate invoked in response to an overloaded GetDevices() request
        /// </summary>
        /// <param name="response">The response status</param>
        /// <param name="status">List of devices</param>
        public delegate void CreateDeviceCallback( string transactionId, bool actionSuccessful, string sessionId ) ;

        /// <summary>
        /// Callback delegate invoked in response to an overloaded PostNotification() request
        /// </summary>
        /// <param name="actionSuccessful">The result status</param>
        public delegate void PostNotificationCallback( string transactionId, bool actionSuccessful, string sessionId ) ;

        /// <summary>
        /// Callback delegate invoked in response to an overloaded GetNotification() request
        /// </summary>
        /// <param name="actionSuccessful">The result status</param>
        public delegate void DeleteNotificationCallback( string transactionId, bool actionSuccessful, string sessionId ) ;

        /// <summary>
        /// Callback delegate invoked in response to an overloaded GetNotification() request
        /// </summary>
        /// <param name="actionSuccessful">The result status</param>
        public delegate void UndeleteNotificationCallback( string transactionId, bool actionSuccessful, string sessionId ) ;

        /// <summary>
        /// Callback delegate invoked in response to an overloaded UpdateNotification() request
        /// </summary>
        /// <param name="actionSuccessful">The result status</param>
        public delegate void UpdateNotificationCallback( string transactionId, bool actionSuccessful, string sessionId ) ;

        /// <summary>
        /// Callback delegate invoked in response to an overloaded GetNotification() request
        /// </summary>
        /// <param name="actionSuccessful">The result status</param>
        public delegate void ExpungeNotificationCallback( string transactionId, bool actionSuccessful, string sessionId ) ;



        /// <summary>
        /// Callback delegate invoked in response to an overloaded GetDeviceInfo() request --- identify the device info
        /// </summary>
        /// <param name="transactionId">Transaction guid</param>
        /// <param name="actionSuccessful">Success or failure of attempt</param>
        /// <param name="deviceComponentInfos">List of information about the device components</param>
        public delegate void GetDeviceInfoCallback( string transactionId, bool actionSuccessful, DeviceInformation deviceInfo ) ;

        /// <summary>
        /// Callback delegate invoked in response to an overloaded SetDeviceHours() request
        /// </summary>
        /// <param name="transactionId">Transaction guid</param>
        /// <param name="actionSuccessful">Success or failure of attempt</param>
        /// <param name="result">SUCCESS or ERROR(code)</param>
        public delegate void SetDeviceHoursCallback( string transactionId, bool actionSuccessful, string result ) ;

        /// <summary>
        /// Callback delegate invoked in response to an overloaded SetDeviceDataKey() request
        /// </summary>
        /// <param name="transactionId">Transaction guid</param>
        /// <param name="actionSuccessful">Success or failure of attempt</param>
        /// <param name="result">SUCCESS or ERROR(code)</param>
        public delegate void SetDeviceDataKeyCallback( string transactionId, bool actionSuccessful, string result ) ;

        /// <summary>
        /// Callback delegate invoked in response to an overloaded SetDeviceDataKey() request
        /// </summary>
        /// <param name="transactionId">Transaction guid</param>
        /// <param name="actionSuccessful">Success or failure of attempt</param>
        /// <param name="result">SUCCESS or ERROR(code)</param>
        public delegate void ClearDeviceLogsCallback( string transactionId, bool actionSuccessful, string result ) ;


        /// <summary>
        /// Callback delegate invoked in response to device exists event (stat/creation) response from RSA
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="actionSuccessful"></param>
        public delegate void DeviceExistsEventCallback(bool actionSuccessful);

        /// <summary>
        /// Callback delegate invoked in response to an overloaded GetDeviceLogList() request --- identify the logs available on the device
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="actionSuccessful"></param>
        /// <param name="deviceLogIds"></param>
        public delegate void GetDeviceLogsListCallback( string transactionId, bool actionSuccessful, List<string> deviceLogIds ) ;

        /// <summary>
        /// Callback delegate invoked in response to an overloaded GetDeviceLog() request --- device log(s) in xml wrapping to handle head and body info
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="actionSuccessful"></param>
        /// <param name="xmlStr"></param>
        public delegate void GetDeviceLogCallback( string transactionId, bool actionSuccessful, LogCargoContainer logInfo ) ;
        public delegate void DeviceLogLoadedCallback( string transactionId, bool actionSuccessful, string logName ) ;

        /// <summary>
        /// Callback to notify all logs are uploaded event
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="actionSuccessful"></param>
        public delegate void AllLogsUploadedToServerCallback(bool actionSuccessful);

        /// <summary>
        /// call back to notify an individual log uploaded event
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="actionSuccessful"></param>
        /// <param name="logName"></param>
        public delegate void LogUploadedToServerCallback(bool actionSuccessful, string logName);

        /// <summary>
        /// Callback delegate invoked in response to a start event of the download process
        /// </summary>
        /// <param name="cpu"></param>
        public delegate void FlashProcessStartedCallback(string cpu);

        /// <summary>
        /// Callback delegate invoked in response to a download software package status change from the device
        /// </summary>
        /// <param name="cpu"></param>
        /// <param name="component"></param>
        /// <param name="message"></param>
        public delegate void FlashProgressCallback( string cpu, string component, string message) ;

        /// <summary>
        /// Callback delegate invoked in response to othersoftware download completion 
        /// </summary>
        /// <param name="actionSuccessful"></param>
        public delegate void OtherPackageDownloadCallback(bool actionSuccessful);

        /// <summary>
        /// Callback delegate invoked in response to a download software package completion status from the device
        /// </summary>
        /// <param name="?"></param>
        public delegate void FlashProcessCompleteCallback();

        /// <summary>
        /// Callback delegate invoked in response to a download software package failure status from the device
        /// </summary>
        /// <param name="errMsg"></param>
        public delegate void DeviceFlashFailedCallback(string errMsg);


        // NON-USER-INITIATED Callbacks

        /// <summary>
        /// Callback delegate invoked in response to a Software Update notification being parsed
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="actionSuccessful"></param>
        /// <param name="softwareUpdateInfo"></param>
        public delegate void AvailSoftwareUpdateCallback( string transactionId, bool actionSuccessful, SoftwareUpdateInfo softwareUpdateInfo ) ;

        /// <summary>
        /// Callback delegate invoked in response to a Log Download completed
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="actionSuccessful"></param>
        /// <param name="logName"></param>
        public delegate void LogDownloadedCallback( string transactionId, bool actionSuccessful, string logName ) ;

        /// <summary>
        /// callback event informing the number of notifications
        /// </summary>
        /// <param name="nNumberOfNotifications"></param>
        public delegate void NotificationsCallback(int nNumberOfNotifications);

        /// <summary>
        /// callback event which signals processing of a notification
        /// </summary>
        public delegate void NotificationProcessedCallback();

        /// <summary>
        /// Callback event which is called when all the notification are processed
        /// </summary>
        public delegate void AllNotificationsProcessedCallback();

        /// <summary>
        /// callback event when a document is successfully unpacked
        /// </summary>
        /// <param name="actionSuccessful"></param>
        /// <param name="filePath"></param>
        public delegate void OpenDocumentCallback(bool actionSuccessful, string filePath);

        /// <summary>
        ///     Callback to get software package/package list from server.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="actionSuccessful"></param>
        public delegate void SoftwareListRetrievalCompleteCallback(List<Software> softwarePkgList);
    }
}
