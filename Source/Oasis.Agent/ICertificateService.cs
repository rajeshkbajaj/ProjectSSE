// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace CryptoServices
{
    using System.Security.Cryptography.X509Certificates;

    public interface ICertificateService
    {
        X509Certificate2 ReadCertificateFromStore(string certificateSubject);

        void StoreCertificate(X509Certificate2 x509Certificate);

        X509Certificate2 CreateCertificate(byte[] certificateData, byte[] privateKey, string privateKeyContainerName);

        bool IsCertificateValid(X509Certificate2 certificate);
    }
}