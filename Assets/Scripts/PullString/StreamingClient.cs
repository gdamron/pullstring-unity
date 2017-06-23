//
// Assets/Scripts/PullString/StreamingClient.cs
//
// Encapsulate a streaming HTTP request to the PullString Web API. The primary use case is ASR.
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//

#if !UNITY_WEBGL

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MiniJSON;

namespace PullString
{
    internal class StreamingClient : HttpClient
    {
        public Stream Stream { get; private set; }
        private HttpWebRequest request;

        private const int DefaultTimeout = 15000;

        public StreamingClient(string baseUrl)
        {
            BaseUrl = baseUrl;
            ServicePointManager.ServerCertificateValidationCallback = CertValidationCallback;
        }

        public void Open(string url, string apiKey)
        {
            request = (HttpWebRequest)System.Net.WebRequest.Create(url);
            request.Method = MethodPost;
            request.Timeout = DefaultTimeout;
            request.SendChunked = true;
            request.Accept = ContentJson;
            request.ContentType = ContentAudio;
            request.Headers.Add(HttpRequestHeader.Authorization, HeaderBearer + apiKey);

            request.BeginGetRequestStream((IAsyncResult asyncResult) =>
            {
                var req = (HttpWebRequest)asyncResult.AsyncState;
                this.Stream = req.EndGetRequestStream(asyncResult);
            }, request);
        }

        public void Close(Action<Response, string> callback = null)
        {
            if (Stream == null) { return; }

            Stream.Close();
            request.BeginGetResponse((IAsyncResult asyncResult) =>
            {
                Stream rStream = null;
                try
                {
                    var req = (HttpWebRequest)asyncResult.AsyncState;
                    var response = req.EndGetResponse(asyncResult);
                    rStream = response.GetResponseStream();

                    if (rStream != null && response.ContentLength == -1) // probably chunked or something went wrong with the size
                    {
                        var reader = new StreamReader(rStream);
                        var responseText = reader.ReadToEnd();
                        var psResponse = ReadResponse(response, responseText);

                        FinalizeResponseRead(callback, response, rStream, psResponse, responseText);
                    }
                    else
                    {
                        var state = new IOState()
                        {
                            Stream = rStream,
                            Buffer = new byte[response.ContentLength],
                            Response = response
                        };

                        rStream.BeginRead(state.Buffer, 0, state.Buffer.Length, (IAsyncResult asyncRead) =>
                        {
                            var readState = (IOState)asyncRead.AsyncState;
                            var rawMessage = Encoding.UTF8.GetString(readState.Buffer);
                            var psResponse = ReadResponse(response, rawMessage);

                            readState.Stream.EndRead(asyncRead);

                            FinalizeResponseRead(callback, response, readState.Stream, psResponse, rawMessage);
                        }, state);
                    }
                }
                catch (WebException e)
                {
                    var webResponse = (HttpWebResponse)e.Response;
                    var psResponse = new Response()
                    {
                        Status = new Status()
                        {
                            Success = false,
                            StatusCode = (long)webResponse.StatusCode,
                            ErrorMessage = webResponse.StatusDescription
                        }
                    };

                    FinalizeResponseRead(callback, webResponse, rStream, psResponse, null);
                }
            }, request);
        }

        private Response ReadResponse(WebResponse aWebResponse, string aRawMessage)
        {
            var dict = Json.Deserialize(aRawMessage) as Dictionary<string, object>;
            var endpoint = aWebResponse.Headers[Keys.EndpointHeader];
            dict.Add(Keys.EndpointHeader, endpoint);

            var psResponse = new Response(dict);
            return psResponse;
        }

        private void FinalizeResponseRead(Action<Response, string> aCallback, WebResponse aWebResponse, Stream aResponseStream, Response aPullStringResponse, string aJsonResponse)
        {
            if (aResponseStream != null)
                aResponseStream.Close();

            if (aWebResponse != null)
                aWebResponse.Close();

            // if we don't set the internal reference to null, the resources are
            // not freed correctly
            Stream = null;

            if (aCallback != null)
            {
                aCallback(aPullStringResponse, aJsonResponse);
            }
        }

        bool CertValidationCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                // filter out case where revocation list is unreachable
                var chainStatus = chain.ChainStatus.Where(s => s.Status != X509ChainStatusFlags.RevocationStatusUnknown);
                if (chainStatus.Count() > 0)
                {
                    // otherwise try to validate the cert
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = TimeSpan.FromMilliseconds(DefaultTimeout);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    if (!chain.Build((X509Certificate2)cert))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private class IOState
        {
            public Stream Stream { get; set; }
            public byte[] Buffer { get; set; }
            public WebResponse Response { get; set; }
        }
    }
}

#endif
