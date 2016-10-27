//
// Assets/Scripts/PullString/Conversation.cs
//
// Encapsulate a conversation thread for PullString's Web API.
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using MiniJSON;

namespace PullString
{
    /// <summary>
    /// The Conversation class can be used to interface with the PullString API.
    ///
    /// To begin a conversation, call the Begin() method, providing a PullString project ID and a Request object
    /// specifying your API key.
    ///
    /// The Web API returns a Response object that can contain zero or more outputs, such as lines of dialog or
    /// behaviors. This Response object is passed to the callback as its sole parameter.
    /// </summary>
    /// <example>
    /// <code>
    /// // create a Request with valid API key
    /// var request = new Request() {
    ///     ApiKey = MY_API_KEY
    /// };
    ///
    /// // create conversation and reqister response delegate
    /// var conversation = new Conversation();
    /// conversation.OnResponseReceived += (Response response) =>
    /// {
    ///     // Debug.Log(response);
    /// }
    ///
    /// // start conversation
    /// conversation.Begin(MY_PROJECT, request);
    /// </code>
    /// </example>
    public class Conversation : MonoBehaviour
    {
        /// <summary>
        /// The signature for a function that receives reseponse from the PullString Web API.
        /// </summary>
        /// <example>
        /// <code>
        /// void OnPullstringResponse(PullString.Response reseponse)
        /// {
        ///     // handle response...
        /// }
        /// </code>
        /// </example>
        /// <param name="response">A PullString.Response object from the Web API</param>
        public delegate void ResponseDelegate(Response response);

        /// <summary>
        /// The event for receiving responses from the PullString Web API.
        /// </summary>
        /// <example>
        /// <code>
        /// var conversation = new Conversation();
        /// conversation.OnResponseReceived += (Response response) =>
        /// {
        ///     // handle response...
        /// };
        /// </code>
        /// </example>
        public event ResponseDelegate OnResponseReceived;

        /// <summary>
        /// The current conversation ID. Conversation IDs can persist across sessions, if desired.
        /// </summary>
        /// <returns>The current conversation ID</returns>
        public string ConversationId
        {
            get
            {
                if (requestInternal != null)
                {
                    return requestInternal.ConversationId;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Get the current participant ID, which identifies the current state for clients. This can persist accross
        /// sessions, if desired.
        /// </summary>
        /// <returns>The current participant ID.</returns>
        public string ParticipantId
        {
            get
            {
                if (requestInternal != null)
                {
                    return requestInternal.ParticipantId;
                }
                return string.Empty;
            }
        }

        // The current API endpoint, which will vary depending on whether a converstion has been started
        internal string Endpoint
        {
            get
            {
                var endpoint = Keys.ConversationEndpoint;
                if (!string.IsNullOrEmpty(ConversationId))
                {
                    endpoint += "/" + ConversationId;
                }
                return endpoint;
            }
        }

        private RestClient restClient = new RestClient(VersionInfo.ApiBaseUrl);
        private StreamingClient streamingClient = new StreamingClient(VersionInfo.ApiBaseUrl);
        private Speech speech;
        private Request requestInternal;
        private HttpWebRequest asrStreamingRequest;

        /// <summary>
        /// Start a new conversation with the Web API and receive a response via the OnResponseReceived event.
        /// </summary>
        /// <example>
        /// <code>
        /// var request = new Request () {
        ///     ApiKey = MY_API_KEY
        /// };
        /// conversation.Begin(MY_PROJECT, request);
        /// </code>
        /// </example>
        /// <param name="project">The PullSring project ID.</param>
        /// <param name="request">A Request object with a valid ApiKey value set</param>
        public void Begin(string project, Request request)
        {
            ensureRequestExists(request);

            var body = getBody(requestInternal, new Dictionary<string, object> {
                { "project", project },
                { "time_zone_offset", request.TimezoneOffset }
            });

            var json = Json.Serialize(body);
            var bytes = Encoding.UTF8.GetBytes(json);
            StartCoroutine(postRequest(bytes));
        }

        /// <summary>
        /// Send user input text to the Web API and receive a response via the OnResponseReceived event.
        /// </summary>
        /// <param name="text">User input text.</param>
        /// <param name="request">[Optional] A request object with at least apiKey and conversationId set.</param>
        public void SendText(string text, Request request = null)
        {
            ensureRequestExists(request);

            var body = getBody(requestInternal, new Dictionary<string, object> {
                { "text", text }
            });

            var json = Json.Serialize(body);
            var bytes = Encoding.UTF8.GetBytes(json);
            StartCoroutine(postRequest(bytes));
        }

        /// <summary>
        /// Send an activity name or ID to the Web API and receive a response via the OnResponseReceived event.
        /// </summary>
        /// <param name="activity">The activity name or ID.</param>
        /// <param name="request">[Optional] A request object with at least apiKey and conversationId set.</param>
        public void SendActivity(string activity, Request request = null)
        {
            ensureRequestExists(request);

            var body = getBody(requestInternal, new Dictionary<string, object> {
                { "activity", activity }
            });

            var json = Json.Serialize(body);
            var bytes = Encoding.UTF8.GetBytes(json);
            StartCoroutine(postRequest(bytes));
        }

        /// <summary>
        /// Send an event to the Web API and receive a response via the OnResponseReceived event.
        /// </summary>
        /// <param name="eventName">The event name.</param>
        /// <param name="parameters">Any accompanying paramters.</param>
        /// <param name="request">[Optional] A request object with at least apiKey and conversationId set.</param>
        public void SendEvent(string eventName, Dictionary<string, object> parameters, Request request = null)
        {
            ensureRequestExists(request);

            var evnt = new Dictionary<string, object> {
                { "name", eventName },
                { "parameters", parameters }
            };

            var body = getBody(requestInternal, new Dictionary<string, object> {
                { "event", evnt }
            });

            var json = Json.Serialize(body);
            var bytes = Encoding.UTF8.GetBytes(json);
            StartCoroutine(postRequest(bytes));
        }

        /// <summary>
        /// Jump the conversation directly to a response.
        /// </summary>
        /// <param name="responseId">The UUID of the response to jump to.</param>
        /// <param name="request">[Optional] A request object with at least apiKey and conversationId set.</param>
        public void GoTo(string responseId, Request request = null)
        {
            ensureRequestExists(request);

            var body = getBody(requestInternal, new Dictionary<string, object> {
                { "goto", responseId }
            });

            var json = Json.Serialize(body);
            var bytes = Encoding.UTF8.GetBytes(json);
            StartCoroutine(postRequest(bytes));
        }

        /// <summary>
        /// Call the Web API to see if there is a time-based response to process. You nly need to call this if the
        /// previous response returned a value for the timedResponseInterval >= 0. In this case, set a timer for that
        /// value (in seconds) and then call this method. If there is no time-based response, OnResponseReceived will
        /// be passed an empty Response object.
        /// </summary>
        /// <param name="request">[Optional] A request object with at least apiKey and conversationId set.</param>
        public void CheckForTimedResponse(Request request = null)
        {
            ensureRequestExists(request);

            var body = getBody(requestInternal);
            var json = Json.Serialize(body);
            var bytes = Encoding.UTF8.GetBytes(json);
            StartCoroutine(postRequest(bytes));
        }

        /// <summary>
        /// Request the values of the specified entities (i.e.: labels, counters, flags, and lists) from the Web API.
        /// </summary>
        /// <param name="names">An array of entity names.</param>
        /// <param name="request">[Optional] A request object with at least apiKey and conversationId set.</param>
        public void GetEntities(string[] names, Request request = null)
        {
            ensureRequestExists(request);

            var body = getBody(requestInternal, new Dictionary<string, object> {
                { "get_entities", names }
            });

            var json = Json.Serialize(body);
            var bytes = Encoding.UTF8.GetBytes(json);
            StartCoroutine(postRequest(bytes));
        }

        /// <summary>
        /// Change the value of the specified entities (i.e.: labels, counters, flags, and lists) via the Web API.
        /// </summary>
        /// <param name="entities">An array specifying the entities to set (with their new values).</param>
        /// <param name="request">[Optional] A request object with at least apiKey and conversationId set.</param>
        public void SetEntities(Entity[] entities, Request request = null)
        {
            ensureRequestExists(request);

            var entDict = entities.ToDictionary(e => e.Name, e => e.Value);
            var body = getBody(requestInternal, new Dictionary<string, object> {
                { "set_entities", entDict }
            });

            var json = Json.Serialize(body);
            var bytes = Encoding.UTF8.GetBytes(json);
            StartCoroutine(postRequest(bytes));
        }

        /// <summary>
        /// Send an entire audio sample of the user speaking to the Web API. Audio must be, mono 16-bit linear PCM at a
        /// sample rate of 16000 samples per second.
        /// </summary>
        /// <param name="clip">Mono 16-bit linear PCM audio clip at 16k Hz.</param>
        /// <param name="request">[Optional] A request object with at least apiKey and conversationId set.</param>
        public void SendAudio(AudioClip clip, Request request = null)
        {
            ensureRequestExists(request);

            var bytes = Speech.PrepareAudioData(clip);
            StartCoroutine(postRequest(bytes, true));
        }

        /// <summary>
        /// Initiate a progressive (chunked) streaming of audio data, where supported.
        ///
        /// Note, chunked streaming is not currently implemented, so this will batch up all audio and send it all at
        /// once when EndAudio() is called.
        /// </summary>
        /// <param name="request">[Optional] A request object with at least apiKey and conversationId set.</param>
        public void StartAudio(Request request = null)
        {
            ensureRequestExists(request);

            if (speech == null)
            {
                speech = new Speech();
            }

            var query = getQuery(requestInternal);
            var url = streamingClient.GetUrl(Endpoint, query);
            streamingClient.Open(url, requestInternal.ApiKey, (stream) =>
            {
                speech.StartStreaming(stream);
            });
        }

        /// <summary>
        /// Add a chunk of raw audio samples. You must call StartAudio() first. The format of the audio must be mono
        /// linear PCM audio data at a sample rate of 16000 samples per second.
        /// </summary>
        /// <param name="audio"></param>
        public void AddAudio(float[] audio)
        {
            if (speech != null)
            {
                speech.StreamAudio(audio);
            }
        }

        /// <summary>
        /// Signal that all audio has been provided via AddAudio() calls. This will complete the audio request and
        /// return the Web API response via the OnResponseReceived event.
        /// </summary>
        public void EndAudio()
        {
            ensureRequestExists();

            if (speech != null)
            {
                speech.StopStreaming();
                streamingClient.Close((response) =>
                {
                    if (OnResponseReceived != null)
                    {
                        OnResponseReceived(response);
                    }
                });
            }
        }

        // async web request using UnityWebRequest
        private IEnumerator postRequest(byte[] body, bool isAudio = false)
        {
            var headers = getHeaders(requestInternal, isAudio);
            var query = getQuery(requestInternal);
            var post = restClient.Post(Endpoint, query, headers, body);

            yield return post.Send();

            var response = restClient.ProcessRequest(post);

            // Conversation ID and Participant ID cam change any time, so keep them current.
            if (response != null)
            {
                requestInternal.ConversationId = response.ConversationId;
                requestInternal.ParticipantId = response.ParticipantId;
            }

            if (OnResponseReceived != null)
            {
                OnResponseReceived(response);
            }
        }

        private Dictionary<string, string> getHeaders(Request request, bool isAudio)
        {
            if (request == null) { return null; }
            var headers = new Dictionary<string, string> {
                { "Authorization", "Bearer " + request.ApiKey },
                { "Accept", "application/json" },
                { "Content-Type", "application/json" }
            };

            if (isAudio)
            {
                headers["Content-Type"] = "audio/l16; rate=16000";
            }

            return headers;
        }

        private Dictionary<string, string> getQuery(Request request)
        {
            var parameters = new Dictionary<string, string> {
                { "asr_language", request.Language }
            };

            if (!string.IsNullOrEmpty(request.AccountId))
            {
                parameters.Add("account", request.AccountId);
            }

            return parameters;
        }

        private Dictionary<string, object> getBody(Request request, Dictionary<string, object> parameters = null)
        {
            var body = new Dictionary<string, object>();
            // only add build_type if non-default value is set.
            if (!string.IsNullOrEmpty(request.BuildType) && !request.BuildType.Equals(EBuildType.Production))
            {
                body.Add("build_type", request.BuildType);
            };

            if (!string.IsNullOrEmpty(ParticipantId))
            {
                body.Add("participant", ParticipantId);
            }

            // only add restart_if_modified if non-default value is set.
            if (!request.RestartIfModified)
            {
                body.Add("restart_if_modified", false);
            }

            if (parameters != null)
            {
                foreach (var kv in parameters)
                {
                    body.Add(kv.Key, kv.Value);
                }
            }

            return body;
        }

        // Keep internal request up to date.
        private void ensureRequestExists(Request request = null)
        {
            // always make a user supplied request the internal request
            if (request != null)
            {
                requestInternal = request;
            }

            // really, this should only happen if null is passed to Begin()
            if (requestInternal == null)
            {
                throw new Exception("Conversation: valid Request object missing.");
            }
        }
    }
}
