// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Oasis.Common
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;

    using Oasis.Common.Attributes;

    public static class OasisAgentRestEndpointExtensions
    {
        public static Uri GetAgentRestUri(this OasisAgentRestEndpoint oasisAgentRestEndpoint, string oasisAgentHostName, int oasisAgentPort)
        {
            return new Uri($"https://{oasisAgentHostName}:{oasisAgentPort}{oasisAgentRestEndpoint.GetAgentEndpointString()}");
        }

        public static string GetAgentEndpointString(this OasisAgentRestEndpoint oasisAgentRestEndpoint)
        {
            return oasisAgentRestEndpoint.GetType()
                      .GetMember(oasisAgentRestEndpoint.ToString())
                      .Single()
                      .GetCustomAttribute<OasisAgentEndpointAttribute>()?
                      .Endpoint ?? string.Empty;
        }

        public static bool? IsClientCertificateRequired(this OasisAgentRestEndpoint oasisAgentRestEndpoint)
        {
            return oasisAgentRestEndpoint.GetType()
                      .GetMember(oasisAgentRestEndpoint.ToString())
                      .Single()
                      .GetCustomAttribute<OasisAgentEndpointAttribute>()?
                      .ClientCertificateRequired;
        }

        public static HttpMethod GetAgentEndpointHttpMethod(this OasisAgentRestEndpoint oasisAgentRestEndpoint)
        {
            var epVerb = oasisAgentRestEndpoint.GetType()
                      .GetMember(oasisAgentRestEndpoint.ToString())
                      .Single()
                      .GetCustomAttribute<OasisAgentEndpointAttribute>()?
                      .OasisAgentEndpointMethodType ?? OasisAgentEndpointMethodType.Post;

            return new HttpMethod(epVerb.ToString());
        }
    }
}