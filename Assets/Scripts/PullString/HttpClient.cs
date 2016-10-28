//
// Assets/Scripts/PullString/HttpClient.cs
//
// Encapsulate some shared functionality for HTTP requests in a base class
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//

using System.Collections.Generic;

namespace PullString {
    internal class HttpClient
    {
        public static string MethodPost = System.Net.WebRequestMethods.Http.Post;
        public static string ContentJson = "application/json";
        public static string ContentAudio = "audio/l16; rate=16000";
        public static string HeaderBearer = "Bearer "; // extra space is intentional

        protected string BaseUrl { get; set; }

        // Convert dictionary of parameters into a query string
        public string GetUrl(string endpoint, Dictionary<string, string> parameters)
        {
            var url = BaseUrl;
            if (!url.EndsWith("/") && !endpoint.StartsWith("/"))
            {
                url += "/";
            }

            url += endpoint + "?";
            var query = new List<string>();
            foreach (var kv in parameters)
            {
                query.Add(kv.Key + "=" + kv.Value);
            }

            url += string.Join("&", query.ToArray());

            return url;
        }
    }
}