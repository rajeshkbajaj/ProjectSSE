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

namespace PB980Mock.WebServices
{
    internal class ErrorWebPage : WebPage
    {
        public ErrorWebPage()
            : base("96b64624-5e50-43f0-abbe-a2009a3ca36c")
        {
        }

        public override string ExecuteRequest(Dictionary<string, string> arguments)
        {
            return 
@"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Strict//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd'> 
<html xmlns='http://www.w3.org/1999/xhtml'> 
   <head>
      <title>PB840Mock 7.5 Detailed Error - 404.0 - Not Found</title> 
   </head> 
   <body> 
      <h1>OOPS</h1>
   </body> 
</html> ";

        }
    }
}