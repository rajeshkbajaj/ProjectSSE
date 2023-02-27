// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Utilties
{
    using System.Management;
    using System.Net;
    using System.Net.NetworkInformation;

    public static class IdentificationHelper
    {
        public static string GetHostName()
        {
            var domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            var hostName = Dns.GetHostName();

            if (string.IsNullOrEmpty(domainName) == false)
            {
                domainName = "." + domainName;
                if (!hostName.EndsWith(domainName))
                {
                    hostName += domainName;
                }
            }

            return hostName;
        }

        // ARON: What is this method supposed to return?  It says "HostId" but that could be accomplished without using a Windows specific call.
        // The need for this call should be evaluated- the way it's working now is not compatible with anything other than Windows.
        // TODO: Refactor this method to use non-windows specific calls if possible
        public static string GetHostId()
        {
            var biosNumber = "";
#pragma warning disable CA1416 // Validate platform compatibility
            var mSearcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard");

            var collection = mSearcher.Get();
            if (collection.Count > 0)
            {
                foreach (ManagementObject obj in collection)
                {
                    biosNumber = (string) obj["SerialNumber"];
                    break;
                }
            }
#pragma warning restore CA1416 // Validate platform compatibility

            return biosNumber;
        }
    }
}