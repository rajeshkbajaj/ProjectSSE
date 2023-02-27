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
    internal class GetDeviceLogsAsXml : WebPage
    {
        public GetDeviceLogsAsXml(string pname)
            : base(pname)
        {
        }

        public override string ExecuteRequest( Dictionary<string, string> arguments )
        {
/*
            return string.Format(@"<?xml version='1.0' standalone='yes' ?>
<logs>
   <log name='SysDiag' uri='http://127.0.0.1:8080/GetDeviceLogAsXml?Log=5'/>
</logs>"
                );
*/
            return string.Format(@"<?xml version='1.0' standalone='yes' ?>
<logs>
   <log name='Alarm' uri='http://127.0.0.1:8080/GetDeviceLogAsXml?Log=0'/>
   <log name='GenEvent' uri='http://127.0.0.1:8080/GetDeviceLogAsXml?Log=1'/>
   <log name='PatientData' uri='http://127.0.0.1:8080/GetDeviceLogAsXml?Log=2'/>
   <log name='Settings' uri='http://127.0.0.1:8080/GetDeviceLogAsXml?Log=3'/>
   <log name='Service' uri='http://127.0.0.1:8080/GetDeviceLogAsXml?Log=4'/>
   <log name='SysDiag' uri='http://127.0.0.1:8080/GetDeviceLogAsXml?Log=5'/>
   <log name='SysComm' uri='http://127.0.0.1:8080/GetDeviceLogAsXml?Log=6'/>
   <log name='EstSst' uri='http://127.0.0.1:8080/GetDeviceLogAsXml?Log=7'/>
</logs>"
                );

        }
    }
}