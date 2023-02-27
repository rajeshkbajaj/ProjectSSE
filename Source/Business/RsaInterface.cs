using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using PCAgentInterface.Messages;
using PCAgentInterface.Messages.Interfaces;
using PCAgentInterface.Notification;
using PCAgentInterface.Notification.Interfaces;

namespace Covidien.CGRS.PcAgentInterfaceBusiness
{
    public class RsaInterface
    {
        private const string ORIGINATOR_MANAGER = "Mngr:";
        private const string ORIGINATOR_RECEIVE = "Recv:";
        private const string ORIGINATOR_SEND = "Send:";

        private const int RECEIVE_BUFFER_SIZE = 2048;
        private const int WAIT_TIME_MILLISECONDS = 125;
        private const string MESSAGE_TERMINATOR = "</message>";


        // The socket connection
        private Socket mServer;
        private IPAddress mIpAddress;
        private int mPortNumber;

        // The target socket
        private IPEndPoint mTargetSocket;

        // The two threads
        private Thread mReceiveThread;
        private Thread mSendThread;

        // The receive buffer (used for incoming data) and mutex
        private Mutex mReceiveBufferMutex;
        public byte[] ReceiveBuffer { get; set; }

        private Mutex mSendBufferMutex;
        public List<string> SendBuffer { get; set; }


        /// <summary>
        /// These callbacks (container actually) must be set before taking any other action
        /// Because of a mutual dependency (at least for now), it is not part of the ctor().
        /// </summary>
        public UserInterface UiCallbacks { get; set; }



        public RsaInterface()
        {
            // Create the buffer muti (there's a conjugation!) and allocated buffers
            mReceiveBufferMutex = new Mutex(false);
            ReceiveBuffer = null ;

            mSendBufferMutex = new Mutex(false);
            SendBuffer = new List<string>();
        }



        /// <summary>
        /// Handles connecting to the socket server
        /// </summary>
        public bool ConnectToServer( IPAddress ipAddress, int portNumber )
        {
            // Define the target socket using the IP address and port number text fields in connection settings tab
            mServer = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp ) ;

            mIpAddress = ipAddress;
            mPortNumber = portNumber;
            mTargetSocket = new IPEndPoint( ipAddress, portNumber ) ;

            // Attempt to connect to the target ip and port
            try
            {
                // This performs the actual connection and will throw a SocketException on failure
                mServer.Connect( mTargetSocket ) ;

                // Start the receive thread
                mReceiveThread = new Thread( ReceiveLoop ) ;
                mReceiveThread.IsBackground = true;
                mReceiveThread.Start();

                // Start the send thread
                mSendThread = new Thread( SendLoop ) ;
                mSendThread.IsBackground = true;
                mSendThread.Start();

                if  ( null != UiCallbacks.ConnectCallback )
                {
                    UiCallbacks.ConnectCallback();
                }

                return( true ) ;
            }
            catch( SocketException ex )
            {
                return( false ) ;
            }
        }


        private void HandleSocketDisconnect()
        {
            // Close and clear out the socket connection
            mServer.Close();
            mReceiveThread.Abort();
            // should probably clear out recv buffer
            mSendThread.Abort();
            // considering allowing send buffer to continue to exist, so as to just pick-up where we left off,
            // the only problem is whether or not we might have lost a message or two in between disconnect/reconnect.
            // Most likely, it will be the message that was occurring when we recognized the disconnect.

            if  ( null != UiCallbacks.DisconnectCallback )
            {
                UiCallbacks.DisconnectCallback();
            }

            // Try to auto-reconnect (once).
            ConnectToServer( mIpAddress, mPortNumber ) ;
        }


        private void HandleMessageReceipt( string str )
        {
            // UiCallbacks.InternalMessageCallback( ORIGINATOR_RECEIVE, "\r\n*** SUBSTR ***\r\n" + str + "\r\n*** END SUBSTR ***\r\n");

            IMessage msg = Message.Deserialize( str ) ;

            if  ( msg.IsRequest() )
            {
                IRequest request = msg.GetRequest();

                UiCallbacks.HandleRsaRequest( request ) ;
            }
            else // if  ( msg.IsResponse() )
            {
                IResponse response = msg.GetResponse();

                UiCallbacks.InternalMessageCallback(ORIGINATOR_RECEIVE, "\r\n***\r\nNEED TO HANDLE Response Message: " + str ) ;

                UiCallbacks.HandleRsaResponse( response ) ;
            }
        }


        /// <summary>
        /// This loop is used by the receive thread and handles incoming data on the socket
        /// </summary>
        private void ReceiveLoop()
        {
            try
            {
                // Repeat until the socket connection fails
                while( mServer.Connected )
                {
                    byte[] data = new byte[1024];

                    // Attempt to get incoming data
                    int receivedDataLength = mServer.Receive(data);

                    byte[] tempArray = new byte[receivedDataLength];

                    for( int i = 0 ;   i < receivedDataLength ;   i++ )
                    {
                        tempArray[i] = data[i];
                    }
                    data = tempArray;

                    // If data was received
                    if  ( 0 < receivedDataLength )
                    {
                        // Get the mutex on the receive buffer
                        mReceiveBufferMutex.WaitOne();

                        // Add the new data to the buffer
                        byte[] newArray ;
                        if  ( null != ReceiveBuffer  )
                        {
                            newArray = new byte[ receivedDataLength + ReceiveBuffer.Length ] ;
                            ReceiveBuffer.CopyTo(newArray, 0);
                            data.CopyTo(newArray, ReceiveBuffer.Length);
                            ReceiveBuffer = newArray;
                        }
                        else
                        {
                            ReceiveBuffer = tempArray;
                        }

                        mReceiveBufferMutex.ReleaseMutex();

                        string str = System.Text.Encoding.ASCII.GetString( ReceiveBuffer, 0, ReceiveBuffer.Length ) ;
                        if  ( ( null != str )  &&  ( null != UiCallbacks.InternalMessageCallback ) )
                        {
                            UiCallbacks.InternalMessageCallback( ORIGINATOR_RECEIVE, "\r\n*****\r\n" + str + "\r\n*****\r\n" ) ;
                        }

                        int posn = str.IndexOf( MESSAGE_TERMINATOR ) ;
                        while( 0 < posn )
                        {
                            int len = posn + MESSAGE_TERMINATOR.Length;

                            string substr = str.Substring(0, len);
                            if  ( len == str.Length )
                            {
                                ReceiveBuffer = null;
                            }
                            else
                            {
                                newArray = new byte[str.Length - substr.Length];
                                Buffer.BlockCopy( ReceiveBuffer, substr.Length, newArray, 0, newArray.Length ) ;
                                ReceiveBuffer = newArray;
                            }
                            
                            HandleMessageReceipt( substr ) ;

                            str = str.Substring( len ) ;
                            posn = str.IndexOf( MESSAGE_TERMINATOR ) ;
                        }

                    }

                    Thread.Sleep( WAIT_TIME_MILLISECONDS ) ;
                }

Console.WriteLine( "receive loop disconnected" ) ;

                // If the socket is no longer connected, handle it
                HandleSocketDisconnect();
            }
            catch (SocketException)
            {
                // If the socket is no longer connected, handle it
                HandleSocketDisconnect();
            }
        }


        /// <summary>
        /// The loop used by the send thread to handle outgoing messages
        /// </summary>
        private void SendLoop()
        {
            try
            {
                // Loop continuously while the server is connected
                while( mServer.Connected )
                {
//Console.Write( "s" ) ;
                    string outStr = null;
                    while( 0 < SendBuffer.Count )
                    {
Console.Write( "(" + SendBuffer.Count + ")" ) ;
                        // Get the mutex for the send buffer
                        mSendBufferMutex.WaitOne();
                        outStr = SendBuffer.ElementAt( 0 ) ;
                        SendBuffer.RemoveAt(0);
                        mServer.Send(System.Text.Encoding.ASCII.GetBytes(outStr));
                        mSendBufferMutex.ReleaseMutex();

                        if  ( ( null != outStr )  &&  ( null != UiCallbacks.InternalMessageCallback ) )
                        {
                            UiCallbacks.InternalMessageCallback( ORIGINATOR_SEND, outStr ) ;
                        }
                    }

                    Thread.Sleep( WAIT_TIME_MILLISECONDS ) ;
                }

Console.WriteLine( "send loop disconnected" ) ;

                // If the socket is disconnected, handle it
                HandleSocketDisconnect();
            }
            catch (SocketException)
            {
                // If the socket is disconnected, handle it
                HandleSocketDisconnect();
            }
        }

    }
}
