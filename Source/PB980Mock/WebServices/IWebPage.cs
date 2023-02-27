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

    internal interface IWebPage
    {
        string ExecuteRequest(Dictionary<string, string> arguments);

        IWebService WebService { get; set; }


        string PageName { get; }

        string DisplayName { get; }

        Boolean InTOC { get; }
    }
}