using System;
using System.Net;
using System.Threading;
using System.Text;
using System.IO;

namespace Covidien.CGRS.CGRS_Core
{
    // Declare a delegate type for processing a book:
    public delegate void RequestResponseCallback( string uid, string response ) ;


    // The RequestState class passes data across async calls.
    public class RequestState
    {
        const int BufferSize = 1024;
        public StringBuilder RequestData ;  // technically this is the _RESPONSE_ data
        public byte[] BufferRead;
        public WebRequest Request;
        public Stream ResponseStream;
        public string Uid;  // unique id by which the callback can know what this is in response to
        public RequestResponseCallback Callback;    // callback provided by the user

        // Create Decoder for appropriate enconding type.
        public Decoder StreamDecode = Encoding.UTF8.GetDecoder();

        public RequestState( WebRequest wreq, string uid, RequestResponseCallback callback )
        {
            BufferRead = new byte[BufferSize];
            RequestData = new StringBuilder(string.Empty);
            Request = wreq;
            ResponseStream = null;

            Uid = uid;
            Callback = callback;
        }
    }


    public class AsyncWebRequest
    {
        const int BUFFER_SIZE = 1024;

        public string mUri;  // URI desired
        public string mUid;  // unique id by which the callback can know what this is in response to
        public RequestResponseCallback mCallback;    // callback provided by the user


        // may want to have a singleton which generates new little pieces, in which case the singleton will own this part, I think
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        // Wait until the ManualResetEvent is set so that the application does not exit until after the callback is called.
//         allDone.WaitOne();


        public AsyncWebRequest( string uri, string uid, RequestResponseCallback callback )
        {
            mUri = uri;
            mUid = uid;
            mCallback = callback;
        }


        public void MakeRequest()
        {
            // Get the URI.
            Uri httpSite = new Uri( mUri ) ;

            // May need this line if we have trouble with certifications.  Leaving commented out for now.
            // ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, ssl) => true;           

            // Create the request object.
            WebRequest wreq = WebRequest.Create( httpSite ) ;
        
            // Create the state object and put the request into the state object so it can be passed around.
            RequestState rs = new RequestState( wreq, mUid, mCallback ) ;

            // Issue the async request.
            IAsyncResult r = (IAsyncResult) wreq.BeginGetResponse( new AsyncCallback( ResponseCallback ), rs ) ;
        }


        private static void ResponseCallback( IAsyncResult ar )
        {
            // Get the RequestState object from the async result.
            RequestState rs = (RequestState) ar.AsyncState;

            // Get the WebRequest from RequestState.
            WebRequest req = rs.Request;

            // Call EndGetResponse, which produces the WebResponse object that came from the request issued above.
            WebResponse resp = req.EndGetResponse(ar);         

            //  Start reading data from the response stream.
            Stream ResponseStream = resp.GetResponseStream();

            // Store the response stream in RequestState to read the stream asynchronously.
            rs.ResponseStream = ResponseStream;

            //  Pass rs.BufferRead to BeginRead. Read data into rs.BufferRead
            IAsyncResult iarRead = ResponseStream.BeginRead( rs.BufferRead, 0, BUFFER_SIZE, new AsyncCallback( ReadCallBack ), rs ) ; 
        }


        private static void ReadCallBack( IAsyncResult asyncResult )
        {
            // Get the RequestState object from AsyncResult.
            RequestState rs = (RequestState) asyncResult.AsyncState;

            // Retrieve the ResponseStream that was set in RespCallback. 
            Stream responseStream = rs.ResponseStream;

            // Read rs.BufferRead to verify that it contains data. 
            int read = responseStream.EndRead( asyncResult );
            if  ( read > 0 )
            {
                // Prepare a Char array buffer for converting to Unicode.
                char[] charBuffer = new char[BUFFER_SIZE];
         
                // Convert byte stream to Char array and then to String.
                // len contains the number of characters converted to Unicode.
                int len = rs.StreamDecode.GetChars(rs.BufferRead, 0, read, charBuffer, 0);

                string str = new string(charBuffer, 0, len);

                // Append the recently read data to the RequestData stringbuilder object contained in RequestState.
                rs.RequestData.Append( Encoding.ASCII.GetString( rs.BufferRead, 0, read ) ) ;

                // Continue reading data until responseStream.EndRead returns –1.
                IAsyncResult ar = responseStream.BeginRead( rs.BufferRead, 0, BUFFER_SIZE, new AsyncCallback( ReadCallBack ), rs ) ;
            }
            else
            {
                if  ( rs.RequestData.Length > 0 )
                {
                    //  Display data to the console.
                    string strContent = rs.RequestData.ToString();

                    rs.Callback?.Invoke(rs.Uid, strContent);
                    // else msg was created as fire-and-forget (or maybe our connection died out somehow)
                }

                // Close down the response stream.
                responseStream.Close();

                // Set the ManualResetEvent so the main thread can exit.
//                allDone.Set();                           
            }
            return;
        }    
    }
}