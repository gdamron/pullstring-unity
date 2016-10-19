//
// Assets/Scripts/PullString/WebRequest.cs
//
// Abstract web request so that we can user UnitWebRequest where available
// but fall back to WWW if necessary.
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//

using UnityEngine;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
using System.Collections;
#endif
using System.Collections.Generic;

namespace PullString
{
#if UNITY_5_4_OR_NEWER
    // Use UnityWebRequest for newer Unity versions. It moved out of the exprimental namespace in 5.4
    internal class WebRequest
    {
        public bool isError { get { return request.isError; } }
        public long responseCode { get { return request.responseCode; } }
        public string error { get { return request.error; } }
        public string responseText { get { return request.downloadHandler.text; } }

        private UnityWebRequest request { get; set; }

        public WebRequest(string url, Dictionary<string, string> headers, byte[] body)
        {
            request = new UnityWebRequest(url);
            foreach (var header in headers)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }

            request.method = UnityWebRequest.kHttpVerbPOST;
            request.uploadHandler = new UploadHandlerRaw(body);
            request.downloadHandler = new DownloadHandlerBuffer();
        }

        public AsyncOperation Send()
        {
            return request.Send();
        }

        public string GetResponseHeader(string header)
        {
            return request.GetResponseHeader(header);
        }
    }

#else
    // Otherwise, fallback to WWW for all previous versions
    internal class WebRequest
    {
        public bool isError { get { return !string.IsNullOrEmpty(request.error); } }
        public long responseCode
        {
            get
            {
                // the error code is the first 3 characters of the error string.
                int code = 0;
                if (isError) {
                    var codeStr = request.error.Substring(0, 3);
                    int.TryParse(codeStr, out code);
                }
                return code;
            }
        }
        public string error
        {
            get
            {
                // strip out the error code and return the rest.
                var error = request.error;
                if (!string.IsNullOrEmpty(error) && error.Length > 4)
                {
                    return request.error.Substring(4);
                }
                return string.Empty;
            }
        }
        public string responseText { get { return request.text; } }

        private string url { get; set; }
        private Dictionary<string, string> headers { get; set; }
        private byte[] body { get; set; }
        private WWW request { get; set; }

        public WebRequest(string url, Dictionary<string, string> headers, byte[] body)
        {
            this.url = url;
            this.headers = headers;
            this.body = body;
        }

        public WWW Send()
        {
            request = new WWW(url, body, headers);
            return request;
        }

        public string GetResponseHeader(string header)
        {
            if (request.responseHeaders.ContainsKey(header)) {
                return request.responseHeaders[header];
            }
            return null;
        }
    }
#endif
}