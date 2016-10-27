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

using System.Collections.Generic;
using MiniJSON;

namespace PullString
{
    internal class RestClient : HttpClient
    {
        public RestClient(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        // All requests to the PullString are Web API are POST requests
        public WebRequest Post(string endpoint, Dictionary<string, string> parameters, Dictionary<string, string> headers, byte[] body)
        {
            var url = GetUrl(endpoint, parameters);
            var request = new WebRequest(url, headers, body);

            return request;
        }

        // Parse web request and create a Response object
        public Response ProcessRequest(WebRequest request)
        {
            Response response = null;

            if (request.isError)
            {
                response = new Response()
                {
                    Status = new Status()
                    {
                        Success = false,
                        StatusCode = request.responseCode,
                        ErrorMessage = request.error
                    }
                };

                return response;
            }

            var responseText = request.responseText;
            var endpoint = request.GetResponseHeader(Keys.LocationHeader);

            var dict = Json.Deserialize(responseText) as Dictionary<string, object>;
            dict.Add(Keys.EndpointHeader, endpoint);
            response = new Response(dict);

            return response;
        }
    }
}
