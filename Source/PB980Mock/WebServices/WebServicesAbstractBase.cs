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
using System;
using System.Collections.Generic;

namespace PB980Mock.WebServices
{
    internal class WebServicesAbstractBase : IWebService
    {
        private readonly Dictionary<string, IWebPage> m_pPageMap;
        private IWebPage m_errorPage;
        private readonly ILog m_Log;

        internal WebServicesAbstractBase(ILog log)
        {
            m_pPageMap = new Dictionary<string, IWebPage>();
            m_errorPage = null;
            m_Log = log;
        }

        internal Boolean addPage(IWebPage pPage)
        {
            if (pPage.PageName.Equals("96b64624-5e50-43f0-abbe-a2009a3ca36c"))
            {
                m_errorPage = pPage;
            }

            pPage.WebService = this;

            m_pPageMap.Add(pPage.PageName, pPage);

            return true;
        }

        public IWebPage getPage(string pszPageName)
        {
            try
            {
                return m_pPageMap[pszPageName];
            }
            catch (Exception)
            {
            }
            return m_errorPage;
        }

        public ILog getLogger()
        {
            return m_Log;
        }
    }
}