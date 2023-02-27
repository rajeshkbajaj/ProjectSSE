// ------------------------------------------------------------------------------
//                    Copyright (c) 2021 Medtronic, Inc.
// This software is copyrighted by and is the sole property of Medtronic/Covidien. This
// is a proprietary work to which Medtronic/Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Medtronic/Covidien.
// ------------------------------------------------------------------------------

namespace Oasis.Agent
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;

    using Utilties;

    using Oasis.Agent.Interfaces;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Threading;
    using Serilog;

    public class RestHelper : IRestHelper
    {
        /// <summary>
        /// Makes a REST request of the specified type to the specified URI, passing the specified data.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> to use for this request</param>
        /// <param name="requestUri">Target request address</param>
        /// <param name="host">host</param>
        /// <param name="requestMethod">HTTP Verb to use</param>
        /// <param name="jsonData">Data to post (if any required)</param>
        /// <param name="clientCertificate">Client certificate to use</param>
        /// <param name="requestTimeoutInSeconds">Request timeout (optional- default 60 seconds)</param>
        /// <returns>Response object containing the success indicator and any response value</returns>
        public async Task<Response<string>> SendRequestAsync(HttpClient httpClient,
            Uri requestUri,
            string host,
            HttpMethod requestMethod,
            string jsonData,
            X509Certificate2 clientCertificate)
        {
            if (clientCertificate == null)
            {
                Log.Information($"RestHelper::SendRequestAsync Request without certificate: requestUri = '{requestUri}', requestMethod: '{requestMethod}', jsonRequestData = '{jsonData}'");
            }
            else
            {
                Log.Information($"RestHelper::SendRequestAsync Request with certificate: requestUri = '{requestUri}', requestMethod: '{requestMethod}', jsonRequestData = '{jsonData}'");
            }
            using (var request = new HttpRequestMessage(requestMethod, requestUri))
            {
                using (var content = new StringContent(jsonData, Encoding.UTF8, "application/json"))
                {
                    request.Content = content;
                    request.RequestUri = requestUri;
                    request.Headers.Add("Host", host);
                    try
                    {
                        CancellationTokenSource eventSource = new CancellationTokenSource();
                        var token = eventSource.Token;
                        using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false))
                        {
                            // Throw if success of the call is false
                            response?.EnsureSuccessStatusCode();

                            var receiveStream = await response?.Content?.ReadAsStreamAsync();

                            var readStream = new StreamReader(receiveStream, Encoding.UTF8);

                            var responseData = await readStream?.ReadToEndAsync();

                            if (responseData.Length < 128)
                            {
                                Log.Information($"RestHelper::SendRequestAsync Received response: {responseData}");
                            }
                            else
                            {
                                Log.Information($"RestHelper::SendRequestAsync Received response: {responseData.Substring(0, 127)}....{responseData.Length}");
                            }
                            var jsonSerializerSettings = new JsonSerializerSettings()
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                MissingMemberHandling = MissingMemberHandling.Ignore,
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            };

                            return Response<string>.Succeeded(responseData);
                        }
                    }
                    catch (Exception httpExp)
                    {
                        Log.Error($"RestHelper::SendRequestAsync Exception: {httpExp.Message}");
                        return Response<string>.Failed("Failed to make an HTTP Request", httpExp);
                    }
                }
            }
        }
    }
}