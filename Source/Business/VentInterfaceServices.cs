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
    using Serilog;
    using System;
    using System.Net;
    using System.Net.Sockets;

    public class VentInterfaceServices : ServerInterfaceServices
    {
        public VentInterfaceServices() : base()
        {
        }

        /// <summary>
        /// Handles connecting to the socket server
        /// </summary>
        public override bool ConnectToServer()
        {
            // Define the target socket using the IP address and port number text fields in connection settings tab
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


            // Attempt to connect to the target ip and port
            try
            {
                TargetSocket = new IPEndPoint(ServerIpAddress, PortNumber);

                // Set the socket configuration options
                ServerSocket.SendTimeout = SOCKET_TIMEOUT;
                ServerSocket.ReceiveTimeout = SOCKET_TIMEOUT;

                ServerSocket.SendBufferSize = 1048510;
                ServerSocket.ReceiveBufferSize = 1048510;

                ServerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                ServerSocket.NoDelay = true;

                // This performs the actual connection and will throw a SocketException on failure
                ServerSocket.Connect(TargetSocket);

                UiCallbacks.ConnectCallback?.Invoke(true);

                return (true);
            }
            catch (SocketException ex)
            {
                Log.Error($"VentInterfaceServices::ConnectToServer Socket Exception:{ex.Message}");
                UiCallbacks.ConnectCallback?.Invoke(false);
                return (false);
            }
            catch (ObjectDisposedException ex)
            {
                Log.Error($"VentInterfaceServices::ConnectToServer Obj Disposed Exception:{ex.Message}");
                UiCallbacks.ConnectCallback?.Invoke(false);
                return (false);
            }
            catch (Exception ex)
            {
                Log.Error($"VentInterfaceServices::ConnectToServer Exception:{ex.Message}");
                UiCallbacks.ConnectCallback?.Invoke(false);
                return (false);
            }
        }
    }
}
