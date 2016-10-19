//
// Assets/Scripts/PullString/RestClient.cs
//
// Encapsulate a REST request to the PullString Web API.
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//

#if !UNITY_5_4_OR_NEWER
using UnityEngine.Experimental.Networking;
#else
using UnityEngine.Networking;
#endif

using System.Collections.Generic;
using MiniJSON;

namespace PullString
{
    internal class RestClient
    {
        private string BaseUrl { get; set; }

        public RestClient (string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        // All requests to the PullString are Web API are POST requests
        public UnityWebRequest Post (string endpoint, Dictionary<string, string> parameters, Dictionary<string, string> headers, byte[] body)
        {
            var url = getUrl (endpoint, parameters);
            var request = new UnityWebRequest (url);
            foreach (var header in headers) {
                request.SetRequestHeader (header.Key, header.Value);
            }

            request.method = UnityWebRequest.kHttpVerbPOST;
            request.uploadHandler = new UploadHandlerRaw (body);
            request.downloadHandler = new DownloadHandlerBuffer ();

            return request;
        }

        // Parse web request and create a Response object
        public Response ProcessRequest (UnityWebRequest request)
        {
            Response response = null;

            if (request.isError) {
                response = new Response () {
                    Status = new Status () {
                        Success = false,
                        StatusCode = request.responseCode,
                        ErrorMessage = request.error
                    }
                };

                return response;
            }

            var responseText = request.downloadHandler.text;
            var endpoint = request.GetResponseHeader ("Location");

            var dict = Json.Deserialize (responseText) as Dictionary<string, object>;
            dict.Add (Keys.EndpointHeader, endpoint);
            response = new Response (dict);

            return response;
        }

        // Convert dictionary of parameters into a query string
        private string getUrl (string endpoint, Dictionary<string, string> parameters)
        {
            var url = BaseUrl;
            if (!url.EndsWith ("/") && !endpoint.StartsWith ("/")) {
                url += "/";
            }

            url += endpoint + "?";
            var query = new List<string> ();
            foreach (var kv in parameters) {
                query.Add (kv.Key + "=" + kv.Value);
            }

            url += string.Join ("&", query.ToArray ());

            return url;
        }
    }
}
