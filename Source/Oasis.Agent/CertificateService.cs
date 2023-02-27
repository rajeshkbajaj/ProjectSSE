// ------------------------------------------------------------------------------
//                    Copyright (c) 2022 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------
namespace CryptoServices
{
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    using Utilties;

    using Serilog;
    using System.IO;
    using System;

#pragma warning disable CA1416 // Validate platform compatibility

    public class CertificateService : ICertificateService
    {
        private readonly EssSettings settings;

        public CertificateService(EssSettings options)
        {
            settings = options;
        }

        public X509Certificate2 CreateCertificate(byte[] certificateData, byte[] privateKey, string privateKeyContainerName)
        {
            var byteKey = DecodePrivateKey(privateKey);

            if (byteKey != null && byteKey.Length > 0)
            {
                var rsaParam = DecodeRSAPrivateKeyToRSAParam(byteKey);

                var cspParams = new CspParameters
                {
                    ProviderType = 1,
                    Flags = CspProviderFlags.UseMachineKeyStore,
                    KeyContainerName = privateKeyContainerName
                };

                using (var rsaProvider = new RSACryptoServiceProvider(cspParams))
                {
                    rsaProvider.ImportParameters(rsaParam);
                    rsaProvider.PersistKeyInCsp = true;

                    var certificate = new X509Certificate2(certificateData)
                        .CopyWithPrivateKey(rsaProvider);

                    return certificate;
                }
            }
            else
            {
                Log.Error("InstallCertificate status: Failed, reason: Invalid/Null Data");
            }

            return null;
        }

        public void StoreCertificate(X509Certificate2 x509Certificate)
        {
            using (var store = new X509Store(settings.CertificateStoreName, settings.CertificateStoreLocation))
            {
                store.Open(OpenFlags.ReadWrite);

                if (!store.Certificates.Contains(x509Certificate))
                {
                    store.Add(x509Certificate);
                    Log.Information("Successfully store the certificate in the store");
                }
                else
                {
                    Log.Information("Specified certificate is already in the store, skipping storage.");
                }
            }
        }

        public X509Certificate2 ReadCertificateFromStore(string certificateSubject)
        {
            using (var store = new X509Store(settings.CertificateStoreName, settings.CertificateStoreLocation))
            {
                store.Open(OpenFlags.ReadOnly);

                var certCollection = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, certificateSubject, false);

                if (certCollection.Count > 0)
                {
                    // ARON: This will always return the last certificate in the collection of certificates with the same subject name
                    // TODO: Verify that this is what we want...
                    // TODO: A better approach would be to Find by Thumbprint above
                    var certificate = certCollection[certCollection.Count - 1];

                    return certificate;
                }
            }

            return null;
        }

        /// <summary>
        /// Verifies the certificate using configured parameters
        /// </summary>
        /// <param name="certificate">Certificate to verify</param>
        /// <returns>True if the certificate is valid, otherwise false</returns>
        public bool IsCertificateValid(X509Certificate2 certificate)
        {
            // Right now this is only returning the results of the .Verify call, but we should do more validation on this
            // TODO: Validate the issuer, subject name (should match our computer), and any other parameters we want
            //return certificate.Verify();
            return certificate.Subject == settings.CertificateSubject;
        }

        /// <summary>
        ///     DecodePrivateKey()
        ///     This method decodes the certificate parameters from cleint certificate.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>byte[].</returns>
        private static byte[] DecodePrivateKey(byte[] key)
        {
            const string RSA_HEADER = "-----BEGIN RSA PRIVATE KEY-----";
            const string RSA_FOOTER = "-----END RSA PRIVATE KEY-----";

            var decodedResultString = Encoding.ASCII.GetString(key);
            var rsaString = decodedResultString.Trim();

            if (!rsaString.StartsWith(RSA_HEADER) || !rsaString.EndsWith(RSA_FOOTER))
            {
                Log.Error(decodedResultString + " is not an unencrypted OpenSSL private key ");
            }

            var sb = new StringBuilder(rsaString);
            sb.Replace(RSA_HEADER, string.Empty); // remove headers/footers, if present
            sb.Replace(RSA_FOOTER, string.Empty);

            var pkString = sb.ToString().Trim(); // get string after removing leading/trailing whitespace

            try
            {
                // if there are no encryption info lines, this is an UNencrypted private key
                var binKey = Convert.FromBase64String(pkString);
                return binKey;
            }
            catch (Exception ex)
            {
                // if can't b64 decode, it must be an encrypted private key
                Log.Error("DecodePrivateKey Exception: " + ex.Message);
            }

            return null;
        }

        /// <summary>
        ///     DecodeRSAPrivateKeyToRSAParam()
        ///     This method decodes/fetches RSA Parameters from certificate private key.
        /// </summary>
        /// <param name="privateKey"></param>
        /// <returns>RSAParameters.</returns>
        private static RSAParameters DecodeRSAPrivateKeyToRSAParam(byte[] privateKey)
        {
            var rSAParams = default(RSAParameters);
            byte[] mODULUS, e, d, p, q, dP, dQ, iQ;
            try
            {
                using (var mem = new MemoryStream(privateKey))
                {
                    using (var binaryReader = new BinaryReader(mem))
                    {
                        byte bt = 0;
                        ushort twoBytes = 0;
                        var elems = 0;
                        twoBytes = binaryReader.ReadUInt16();
                        if (twoBytes == 0x8130)
                        {
                            binaryReader.ReadByte();
                        }
                        else if (twoBytes == 0x8230)
                        {
                            binaryReader.ReadInt16();
                        }
                        else
                        {
                            return rSAParams;
                        }

                        twoBytes = binaryReader.ReadUInt16();
                        if (twoBytes != 0x0102)
                        {
                            return rSAParams;
                        }

                        bt = binaryReader.ReadByte();
                        if (bt != 0x00)
                        {
                            return rSAParams;
                        }

                        elems = GetIntegerSize(binaryReader);
                        mODULUS = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        e = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        d = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        p = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        q = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        dP = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        dQ = binaryReader.ReadBytes(elems);

                        elems = GetIntegerSize(binaryReader);
                        iQ = binaryReader.ReadBytes(elems);
                        rSAParams.Modulus = mODULUS;
                        rSAParams.Exponent = e;
                        rSAParams.D = d;
                        rSAParams.P = p;
                        rSAParams.Q = q;
                        rSAParams.DP = dP;
                        rSAParams.DQ = dQ;
                        rSAParams.InverseQ = iQ;
                        return rSAParams;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("DecodeRSAPrivateKeyToRSAParam Failed to Parse Certificate Parameter" + ex.Message);
                return rSAParams;
            }
        }

        /// <summary>
        ///     GetIntegerSize()
        ///     This is helper method for parsing RSAparameters. It gets the Interger size (8 bit, 16bit or 32 bit etc).
        /// </summary>
        /// <param name="binr"></param>
        /// <returns>int.</returns>
        private static int GetIntegerSize(BinaryReader binr)
        {
            var bt = binr.ReadByte();

            // expect integer
            if (bt != 0x02)
            {
                return 0;
            }

            bt = binr.ReadByte();

            int count;
            if (bt == 0x81)
            {
                count = binr.ReadByte(); // data size in next byte
            }
            else if (bt == 0x82)
            {
                var highbyte = binr.ReadByte();
                var lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt; // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {
                // remove high order zeros in data
                count -= 1;
            }

            binr.BaseStream.Seek(-1, SeekOrigin.Current); // last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
    }

#pragma warning restore CA1416 // Validate platform compatibility
}