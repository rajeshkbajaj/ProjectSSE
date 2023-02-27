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
    using System.Diagnostics;
    using System.Reflection;

    public class VersionHelper
    {
        /// <summary>
        /// Gets the version of the assembly that is executing
        /// </summary>
        public static string ExecutingAssemblyVersion
        {
            get
            {
                var version = "Unknown";

                try
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                    version = fileVersionInfo.ProductVersion;
                }
                catch
                {
                    // Ignore
                }

                return version;
            }
        }
    }
}