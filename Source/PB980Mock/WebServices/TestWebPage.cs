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
using System.Text;

namespace PB980Mock.WebServices
{
    internal class TestWebPage : WebPage
    {
        public TestWebPage(string pname)
            : base(pname)
        {
        }

        public override string ExecuteRequest(Dictionary<string, string> arguments)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( "<?xml version=\"1.0\" encoding=\"ISO-8859-1\" ?>"); 

            sb.Append("<arguments>\n");
            foreach (var test in arguments)
            {
                sb.Append("   <param key='" + test.Key + "'>" + test.Value + "</param>\n");
            }

            sb.Append("</arguments>\n");
            return sb.ToString();
        }
    }
}