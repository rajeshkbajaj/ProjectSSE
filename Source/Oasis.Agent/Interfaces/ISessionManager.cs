// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Oasis.Agent.Interfaces
{
    using System;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;

    public interface ISessionManager
    {
        IAgentInterface EstablishSession(OasisCredentialInformation oasisCredentialInformation,
                X509Certificate2 clientCertificate = null,
                Func<HttpRequestMessage, X509Certificate2, X509Chain, System.Net.Security.SslPolicyErrors, bool> serverCertificateCustomValidationCallback = null);
        
        void CloseSession();
        
        bool IsRSASessionOpened();
    }
}