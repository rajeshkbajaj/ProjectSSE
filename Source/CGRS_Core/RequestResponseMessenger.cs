using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Covidien.CGRS.CGRS_Core
{
    public class RequestResponseMessenger
    {
        /// <summary>
        /// The variable provides the locking object
        /// </summary>
        private static readonly object MSyncRoot = new object();

        /// <summary>
        /// This variable holds the information about all the managed devices - indexed by XXXXX.
        /// </summary>
        private readonly Hashtable ManagedMessages = new Hashtable();
        private readonly Mutex MutexManagedMessages = new Mutex();

        /// <summary>
        /// mSingleton and Instance provides the private variable and public access to it - to provide the Singleton.
        /// The double-check method for lazy evaluation of the Singleton properly supports multi-threaded applications
        /// </summary>
        private static volatile RequestResponseMessenger MSingleton;
        public static RequestResponseMessenger Instance
        {
            get
            {
                if (null == MSingleton)
                {
                    lock (MSyncRoot)
                    {
                        if (null == MSingleton)
                        {
                            MSingleton = new RequestResponseMessenger();
                        }
                    }
                }

                return MSingleton;
            }
        }

        /// <summary>
        /// The ctor() is private in support of the Singleton model.
        /// </summary>
        private RequestResponseMessenger()
        {
        }


        public void ManagerResponseCallback( string uid, string data )
        {
            // RequestResponseCallback callback = null;
            KeyValuePair<AsyncWebRequest, RequestResponseCallback> kvp ;
            lock( MutexManagedMessages )
            {
                kvp = (KeyValuePair<AsyncWebRequest, RequestResponseCallback>) ManagedMessages[ uid ] ;
                ManagedMessages.Remove( uid ) ;
            }
            kvp.Value?.Invoke(uid, data);
        }


        public void RequestData( string uri, string uid, RequestResponseCallback callback )
        {
            AsyncWebRequest awr = new AsyncWebRequest( uri, uid, ManagerResponseCallback ) ;

            lock( MutexManagedMessages )
            {
                KeyValuePair<AsyncWebRequest, RequestResponseCallback> kvp = new KeyValuePair<AsyncWebRequest, RequestResponseCallback>( awr, callback ) ;

                // Intentionally allowing the exception to occur on a duplicate UID
                ManagedMessages.Add( uid, kvp ) ;
            }

            awr.MakeRequest();
        }
    }
}
