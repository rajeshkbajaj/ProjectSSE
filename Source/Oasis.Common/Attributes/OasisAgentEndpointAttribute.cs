// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Oasis.Common.Attributes
{
    using System;

    public class OasisAgentEndpointAttribute : Attribute
    {
        public string Endpoint { get; }
        public OasisAgentEndpointMethodType OasisAgentEndpointMethodType { get; }
        public bool ClientCertificateRequired { get; }

        public OasisAgentEndpointAttribute(string endpoint, OasisAgentEndpointMethodType oasisAgentEndpointMethodType, bool clientCertificateRequired = false)
        {
            Endpoint = endpoint;
            OasisAgentEndpointMethodType = oasisAgentEndpointMethodType;
            ClientCertificateRequired = clientCertificateRequired;
        }
    }
}