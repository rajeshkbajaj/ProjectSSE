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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Net.NetworkInformation;
    using System.Threading;
    using System.Xml;
    using IPI_Core;
    using Serilog;

    public class ServerInterfaceServices
    {
        public const string FMT_DIRECTIONAL_MSG = "{0}:{1}\r\n{2}";
        public const string ORIGINATOR_MANAGER = "Mngr:";
        public const string ORIGINATOR_RECEIVE = "Recv:";
        public const string ORIGINATOR_SEND = "Send:";

        protected const int SOCKET_TIMEOUT = 0;   // 0 == infinite wait 30000;

        protected const int RECEIVE_BUFFER_SIZE = 2048;
        protected const int WAIT_TIME_MILLISECONDS = 1000 ; // only testing the 1-second -- normally set at 1/8th == 125;
        protected const int SERVER_MONITOR_TIME = 10000; // 1 minute
        protected const string MESSAGE_TERMINATOR = "</message>";

        public IPAddress ServerIpAddress { get; protected set; }
        public int PortNumber { get; protected set; }

        private bool MonitorServerEnabled = false;

        // The socket connection
        protected Socket ServerSocket;

        // The target socket
        protected IPEndPoint TargetSocket;

        // The two threads
        protected Thread ReceiveThread;
        protected Thread SendThread;
        protected Thread MonitorServerThread;

        // The receive buffer (used for incoming data) and mutex
        protected Mutex ReceiveBufferMutex;
        protected byte[] ReceiveBuffer { get; set; }

        protected Mutex SendBufferMutex;
        protected List<string> SendBuffer { get; set; }

        /// <summary>
        /// These callbacks (container actually) must be set before taking any other action
        /// Because of a mutual dependency (at least for now), it is not part of the ctor().
        /// </summary>
        public UserInterfaceServices UiCallbacks { get; set; }

        public ServerInterfaceServices()
        {
            // Create the buffer muti (there's a conjugation!) and allocated buffers
            ReceiveBufferMutex = new Mutex(false);
            ReceiveBuffer = null ;

            SendBufferMutex = new Mutex(false);
            SendBuffer = new List<string>();
        }


        /// <summary>
        /// Stores the IPAddress and PortNumber for the Server.
        /// MUST be called prior to connecting to the server!
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="portNumber"></param>
        public void IdentifyServer( IPAddress ipAddress, int portNumber )
        {
            ServerIpAddress = ipAddress;
            PortNumber = portNumber;
        }

        /// <summary>
        /// Handles connecting to the socket server
        /// </summary>
        public virtual bool ConnectToServer()
        {
            // Define the target socket using the IP address and port number text fields in connection settings tab
            ServerSocket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp ) ;

            // A web recommendation to use a BufferedStream -- will consider if the timeout issue fails.
            // this.datastream = new **BufferedStream**((Stream)new NetworkStream(socket), 1048510); 


            // Attempt to connect to the target ip and port
            try
            {
                TargetSocket = new IPEndPoint(ServerIpAddress, PortNumber ) ;

                // Set the socket configuration options
                ServerSocket.SendTimeout = SOCKET_TIMEOUT;
                ServerSocket.ReceiveTimeout = SOCKET_TIMEOUT;

                ServerSocket.SendBufferSize = 1048510;
                ServerSocket.ReceiveBufferSize = 1048510;

                ServerSocket.SetSocketOption( SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true ) ;
                ServerSocket.NoDelay = true;

                // This performs the actual connection and will throw a SocketException on failure
                ServerSocket.Connect( TargetSocket ) ;

                // Start the receive thread
                ReceiveThread = new Thread(ReceiveLoop)
                {
                    IsBackground = true
                };
                ReceiveThread.Start();

                // Start the send thread
                SendThread = new Thread(SendLoop)
                {
                    IsBackground = true
                };
                SendThread.Start();

                UiCallbacks.ConnectCallback?.Invoke(true);

                return ( true ) ;
            }
            catch( SocketException ex)
            {
                Log.Error($"ServerInterfaceServices::ConnectToServer Exception1:{ex.Message}");
                UiCallbacks.ConnectCallback?.Invoke(false);
                return (false);
            }
            catch (ObjectDisposedException ex)
            {
                Log.Error($"ServerInterfaceServices::ConnectToServer Exception2:{ex.Message}");
                UiCallbacks.ConnectCallback?.Invoke(false);
                return (false);
            }
            catch (Exception ex)
            {
                Log.Error($"ServerInterfaceServices::ConnectToServer Exception3:{ex.Message}");
                UiCallbacks.ConnectCallback?.Invoke(false);
                return (false);
            }
        }

        /// <summary>
        /// Called when user initiates a server disconnect
        /// </summary>
        public void DisconnectFromServer()
        {
            try
            {
                // Close and clear out the socket connection
                if (ReceiveThread != null)
                {
                    ReceiveThread.Abort();
                }
                // should probably clear out recv buffer
                ReceiveThread = null;

                if (SendThread != null)
                {
                    SendThread.Abort();
                }
                SendThread = null;

                if ((ServerSocket != null) &&
                    (ServerSocket.Connected == true))
                {
                    ServerSocket.Close();
                }

                MonitorServerEnabled = false;

                // considering allowing send buffer to continue to exist, so as to just pick-up where we left off,
                // the only problem is whether or not we might have lost a message or two in between disconnect/reconnect.
                // Most likely, it will be the message that was occurring when we recognized the disconnect.

                ReceiveThread = null;
                SendThread = null;
                ServerSocket = null;

                Log.Information(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_MANAGER, "DiconnectFromServer()"));
            }
            catch (Exception e)
            {
                Log.Error(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_MANAGER, "DiconnectFromServer(): " + e.ToString()));
            }

        }

        private void MonitorServerLoop()
        {
            // try pinging the server ip
            const int PING_TIME_OUT = 5000; //5 seconds
            bool stillExists = true;
            const short FAILURE_COUNT_MAX = 3; //3 successive failures
            short failureCount = 0;

            try
            {
                Ping pingSender = new Ping();
                PingReply reply;

                while (MonitorServerEnabled && (stillExists == true))
                {
                    //30 second time-out
                    reply = pingSender.Send(ServerIpAddress, PING_TIME_OUT);

                    if (reply.Status != IPStatus.Success)
                        failureCount++;
                    else
                    {
                        //reset failure count, need continuous failures
                        failureCount = 0;
                    }

                    if (failureCount == FAILURE_COUNT_MAX)
                    {
                        stillExists = false;
                    }
                    else
                    {
                        //sleep for 1 minute
                        Thread.Sleep(SERVER_MONITOR_TIME);
                    }
                }

                if (MonitorServerEnabled == true)
                {
                    // If the socket is disconnected, handle it
                    Log.Error(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_MANAGER, "MonitorServer()::Server Not Reachable"));
                    HandleSocketDisconnect();
                }
            }
            catch (Exception e)
            {
                stillExists = false;

                Log.Error(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_MANAGER, "MonitorServer()::Exception2:" + e.ToString()));

                if (MonitorServerEnabled == true)
                {
                    HandleSocketDisconnect();
                }
            }
        }

        public void StartServerMonitoring()
        {
            try
            {
                if (MonitorServerEnabled == true)
                {
                    return;
                }

                MonitorServerEnabled = true;
                // Start the send thread
                MonitorServerThread = new Thread(MonitorServerLoop)
                {
                    IsBackground = true
                };
                MonitorServerThread.Start();

                Log.Information(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_MANAGER, "MonitorServer()::Started:"));
            }
            catch (Exception e)
            {
                Log.Error(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_MANAGER, "MonitorServer()::Exception1:" + e.ToString()));
            }
        }

        private void HandleSocketDisconnect()
        {
            UiCallbacks.DisconnectCallback?.Invoke();

            DisconnectFromServer();
        }

        public void StopServerMonitoring()
        {
            MonitorServerEnabled = false;
            Log.Information(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_MANAGER, "MonitorServer()::Stopped"));
        }




        /// <summary>
        /// This loop is used by the receive thread and handles incoming data on the socket
        /// </summary>
        private void ReceiveLoop()
        {
            string exStr = "ERR 1";
            try
            {
                // Repeat until the socket connection fails
                while (ServerSocket.Connected)
                {
                    byte[] data = new byte[1024];

                    // Attempt to get incoming data
                    exStr = "ERR 1.1";
                    int receivedDataLength = ServerSocket.Receive(data);

                    exStr = "ERR 1.2";
                    byte[] tempArray = new byte[receivedDataLength];

                    exStr = "ERR 1.3";
                    for (int i = 0; i < receivedDataLength; i++)
                    {
                        tempArray[i] = data[i];
                    }
                    data = tempArray;

                    // If data was received
                    if (0 < receivedDataLength)
                    {
                        exStr = "ERR 2";
                        // Get the mutex on the receive buffer
                        ReceiveBufferMutex.WaitOne();

                        // Add the new data to the buffer
                        byte[] newArray;
                        if (null != ReceiveBuffer)
                        {
                            exStr = "ERR 3";
                            newArray = new byte[receivedDataLength + ReceiveBuffer.Length];
                            ReceiveBuffer.CopyTo(newArray, 0);
                            data.CopyTo(newArray, ReceiveBuffer.Length);
                            ReceiveBuffer = newArray;
                        }
                        else
                        {
                            ReceiveBuffer = tempArray;
                        }

                        exStr = "ERR 4";
                        ReceiveBufferMutex.ReleaseMutex();

                        // Note: ASCII decoding, maybe it should be UTF-8
                        // Better, how do we read the first part which might tell us the encoding, and then further decode properly
                        string str = System.Text.Encoding.ASCII.GetString(ReceiveBuffer, 0, ReceiveBuffer.Length);
                        if ((null != str) && (null != UiCallbacks.InternalMessageCallback))
                        {
                            UiCallbacks.InternalMessageCallback(ORIGINATOR_RECEIVE, "\r\n*****\r\n" + str + "\r\n*****\r\n");
                        }

                        exStr = "ERR 5";
                        int posn = str.IndexOf(MESSAGE_TERMINATOR);
                        while (0 < posn)
                        {
                            int len = posn + MESSAGE_TERMINATOR.Length;

                            string substr = str.Substring(0, len);
                            if (len == str.Length)
                            {
                                ReceiveBuffer = null;
                            }
                            else
                            {
                                exStr = "ERR 6";
                                newArray = new byte[str.Length - substr.Length];
                                Buffer.BlockCopy(ReceiveBuffer, substr.Length, newArray, 0, newArray.Length);
                                ReceiveBuffer = newArray;
                            }

                            exStr = "ERR 7";
                            //HandleMessageReceipt(substr);

                            exStr = "ERR 8";
                            str = str.Substring(len);
                            posn = str.IndexOf(MESSAGE_TERMINATOR);
                        }

                    }

                    Thread.Sleep(WAIT_TIME_MILLISECONDS);
                }

                exStr = "ERR 9";
                Console.WriteLine("receive loop disconnected");

                // If the socket is no longer connected, handle it
                Log.Error(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_MANAGER, "ReceiveLoop::Server Not Connected"));

                HandleSocketDisconnect();
            }
            catch (SocketException ex)
            {
                Log.Error(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_MANAGER, "ReceiveLoop::SocketException: " + exStr + "\r\n" + ex.ToString()));

                // If the socket is no longer connected, handle it
                HandleSocketDisconnect();
            }
            catch (ObjectDisposedException ex)
            {
                Log.Error(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_MANAGER, "ReceiveLoop::SocketException: " + exStr + "\r\n" + ex.ToString()));

                // If the socket is no longer connected, handle it
                HandleSocketDisconnect();
            }
        }

        /// <summary>
        /// Thread safe method to add to the send buffer
        /// </summary>
        /// <param name="sendItem"></param>
        public void AddToSendQueue(string sendItem)
        {
            SendBufferMutex.WaitOne();
            SendBuffer.Add(sendItem);
            SendBufferMutex.ReleaseMutex();
        }

        /// <summary>
        /// The loop used by the send thread to handle outgoing messages
        /// </summary>
        private void SendLoop()
        {
            string exStr = "ERR 1";
            try
            {
                // Loop continuously while the server is connected
                while(ServerSocket.Connected )
                {
                    //while server is connected, send items one by one
                    string outStr = null;
                    SendBufferMutex.WaitOne();
                    if (0 < SendBuffer.Count)
                    {
                        outStr = SendBuffer.ElementAt(0);
                        SendBuffer.RemoveAt(0);
                    }
                    SendBufferMutex.ReleaseMutex();

                    if (string.IsNullOrEmpty(outStr) == false)
                    {
                        exStr = "ERR 2";
                        // Strip out newline characters
                        // StripChars() is a custom extension in IPI_Core.StringExtensions
                        outStr = outStr.StripChars(new HashSet<char>(new[] { '\n', '\r' }));

                        exStr = "ERR 3";
                        string stringToBeWrittenIntoFile = outStr;
                        if (stringToBeWrittenIntoFile.Contains("password") == true)
                        {
                            try
                            {
                                //modify password innertext
                                XmlDocument xmlDoc = new XmlDocument();
                                xmlDoc.LoadXml(stringToBeWrittenIntoFile);
                                XmlNode passwordNode = xmlDoc.SelectSingleNode("//password");

                                // if found, begin manipulation    
                                if (passwordNode != null)
                                {
                                    // change contents for <password> node 
                                    passwordNode.InnerText = "*****";
                                }

                                using (var stringWriter = new StringWriter())
                                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                                {
                                    xmlDoc.WriteTo(xmlTextWriter);
                                    xmlTextWriter.Flush();
                                    stringToBeWrittenIntoFile = stringWriter.GetStringBuilder().ToString();
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error($"ServerInterfaceServices::SendLoop Exception:{ex.Message}");
                                //not xml format etc.
                                stringToBeWrittenIntoFile = outStr;
                            }

                        }

                        Log.Information(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_SEND, stringToBeWrittenIntoFile));

                        exStr = "ERR 3.1::" + outStr;
                        ServerSocket.Send(System.Text.Encoding.UTF8.GetBytes(outStr));

                        exStr = "ERR 4";
                        if ((null != outStr) && (null != UiCallbacks.InternalMessageCallback))
                        {
                            UiCallbacks.InternalMessageCallback(ORIGINATOR_SEND, outStr);    // + "\r\n--- vs ---\r\n" + outStr2 ) ;
                        }
                    }

                    //sleep after 1 send for stability reasons
                    Thread.Sleep(WAIT_TIME_MILLISECONDS);
                }

                Console.WriteLine( "send loop disconnected" ) ;

                // If the socket is disconnected, handle it
                Log.Error(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_MANAGER, "SendLoop::Server Not Connected"));

                HandleSocketDisconnect();
            }
            catch( SocketException ex )
            {
                Log.Error(string.Format(FMT_DIRECTIONAL_MSG, DateTime.Now, ORIGINATOR_MANAGER, "SEND SocketException: " + exStr + "\r\n" + ex.ToString()));

                // If the socket is disconnected, handle it
                HandleSocketDisconnect();
            }
            catch (ObjectDisposedException ex)
            {
                Log.Error($"ServerInterfaceServices::SendLoop ObjectDisposedException:{ex.Message}");
                if (ex.ObjectName.Equals("System.Net.Sockets.Socket"))
                {
                    //try and connect again
                    // If the socket is disconnected, handle it
                    HandleSocketDisconnect();
                }
            }
        }
    }
}
