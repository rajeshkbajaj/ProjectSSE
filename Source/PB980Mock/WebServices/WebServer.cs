//----------------------------------------------------------------------------
//            Copyright (c) 2012 Covidien, Inc.
//
// This software is copyrighted by and is the sole property of Covidien. This
// is a proprietary work to which Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Covidien.
//----------------------------------------------------------------------------
using System.Net;
using System.Threading;

namespace PB980Mock.WebServices
{
    public class WebServer
    {
        private ILog m_log;

        private HttpListener listener;

        public WebServer(ILog log)
        {
            m_log = log;

            Thread thread = new Thread(new ThreadStart(WorkThreadFunction));

            thread.Start();
        }


        public void Close()
        {
            listener.Stop();
        }

        private void WorkThreadFunction()
        {



            listener = new HttpListener();

            listener.Prefixes.Add("http://*:8080/");
            listener.Start();

            IWebService webservices = new Line1WebServicesMain(m_log);

            m_log.LogMessage("HTTP WebService started.");

            try
            {
                for (;;)
                {
                    HttpListenerContext ctx = listener.GetContext();
                    new Thread(new Worker(webservices, ctx).ExecuteRequest).Start();
                }
            }
            catch(HttpListenerException)
            {
         
            }

            m_log.LogMessage("HTTP WebService stopped.");
        }
       
    }


}