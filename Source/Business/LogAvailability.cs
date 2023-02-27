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
    public class LogAvailability
    {
        public string Name { get; set; }
        public string Protocol { get; set; }
        public string UriSegment { get; set; }

        public LogAvailability( string name, string proto, string uri )
        {
            Name = name;

            if  ( string.IsNullOrEmpty( proto ) )
            {
                Protocol = "http";
            }
            else
            {
                Protocol = proto;
            }

            UriSegment = uri;
        }
    }
}
