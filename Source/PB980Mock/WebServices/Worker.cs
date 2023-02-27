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
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PB980Mock.WebServices
{
    internal class Worker
    {
        private HttpListenerContext context;
        private IWebService m_webservices;

        internal Worker(IWebService webservices, HttpListenerContext context)
        {
            this.context = context;
            m_webservices = webservices;
        }

        internal void ExecuteRequest()
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();


            StringBuilder sb = new StringBuilder();

            sb.Append("Request: " + context.Request.Url.AbsoluteUri+"  ");


            sb.Append("{");
            foreach (var header in context.Request.QueryString)
            {
                temp[header.ToString()] = context.Request.QueryString[header.ToString()];
                
                sb.Append(header.ToString()+"="+context.Request.QueryString[header.ToString()]+",");
            }
            sb.Append("}");
            m_webservices.getLogger().LogMessage(sb.ToString());

            IWebPage page = m_webservices.getPage(context.Request.Url.AbsolutePath);



            byte[] b = Encoding.UTF8.GetBytes(page.ExecuteRequest(temp));
            context.Response.ContentLength64 = b.Length;
            context.Response.OutputStream.Write(b, 0, b.Length);
            context.Response.OutputStream.Close();

            if (page.PageName.Equals("96b64624-5e50-43f0-abbe-a2009a3ca36c"))
            {
                m_webservices.getLogger().LogMessage("Error:Page not found " + context.Request.Url.AbsoluteUri);
            }
            else
            {
                m_webservices.getLogger().LogMessage("Response:length="+b.Length );
            }
        }
    }
}