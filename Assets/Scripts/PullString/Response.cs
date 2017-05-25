//
// Assets/Scripts/PullString/Response.cs
//
// Encapsulate a response from the PullString Web API.
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//

using System;
using System.Collections.Generic;

namespace PullString
{
    /// <summary>
    /// Presents the output of a request to the PullString Web API.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Represents status and any errors returned by Web API
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Identifies an ongoing conversation to the Web API and can persist across sessions. It is required after a
        /// conversation is started.
        /// </summary>
        public string ConversationId { get; private set; }

        /// <summary>
        /// Identifies state to the Web API and can persist across sessions.
        /// </summary>
        public string ParticipantId { get; private set; }

        /// <summary>
        /// Unique identifier for a version of the content.
        /// </summary>
        public string ETag { get; private set; }

        /// <summary>
        /// Dialog or behaviors returned from the Web API.
        /// </summary>
        public Output[] Outputs { get; private set; }

        /// <summary>
        /// Counters, flags, etc for the conversation.
        /// </summary>
        public Entity[] Entities { get; private set; }

        /// <summary>
        /// Time of content modification.
        /// </summary>
        public DateTime LastModified { get; private set; }

        /// <summary>
        /// Indicates that there may be another response to process in the specified number of seconds. Set a timer and
        /// call CheckForTimedResponse() from a conversation to retrieve it.
        /// </summary>
        public double TimedResponseInterval { get; private set; }

        /// <summary>
        /// The recognized speech, if audio has been submitted.
        /// </summary>
        public string AsrHypothesis { get; private set; }

        /// <summary>
        /// The public endpoint for the current conversation.
        /// </summary>
        /// <returns></returns>
        public string Endpoint { get; private set; }

        public Response()
        {
            // default constructor, handy for constructing error responses
        }

        public Response(Dictionary<string, object> json)
        {
            Status = new Status();
            var error = (Dictionary<string, object>)json.GetValue(Keys.Error);
            if (error != null)
            {
                Status.Success = false;
                Status.ErrorMessage = (string)error.GetValue(Keys.Message);
                if (error.ContainsKey(Keys.Status))
                {
                    Status.StatusCode = (long)error[Keys.Status];
                }
            }

            ConversationId = (string)json.GetValue(Keys.ConversationId);
            ParticipantId = (string)json.GetValue(Keys.ParicipantId);
            ETag = (string)json.GetValue(Keys.ETag);
            AsrHypothesis = (string)json.GetValue(Keys.AsrHypothesis);
            Endpoint = (string)json.GetValue(Keys.EndpointHeader);

            if (json.ContainsKey(Keys.TimedResponseInterval))
            {
                TimedResponseInterval = (double)json.GetValue(Keys.TimedResponseInterval);
            }

            if (json.ContainsKey(Keys.LastModified))
            {
                var interval = (string)json[Keys.LastModified];
                // The UTC identifier breaks DateTime parsing. C# prefers 'Z'
                interval = interval.Replace(" UTC", "Z");
                LastModified = Convert.ToDateTime(interval);
            }

            var outputs = (List<object>)json.GetValue(Keys.Outputs);
            if (outputs != null)
            {
                var outputList = new List<Output>();
                foreach (var outObj in outputs)
                {
                    var o = (Dictionary<string, object>)outObj;
                    var outputType = o.GetValue(Keys.Type);
                    if (outputType == null)
                    {
                        continue;
                    }

                    if (outputType.Equals(EOutputType.Behavior))
                    {
                        outputList.Add(new BehaviorOutput(o));
                    }
                    else if (outputType.Equals(EOutputType.Dialog))
                    {
                        outputList.Add(new DialogOutput(o));
                    }
                }

                Outputs = outputList.ToArray();
            }

            var entities = (Dictionary<string, object>)json.GetValue(Keys.Entities);
            if (entities != null)
            {
                var entityList = new List<Entity>();
                foreach (var e in entities)
                {
                    var name = e.Key;
                    var val = e.Value;
                    var type = val.GetType();

                    if (type == typeof(string))
                    {
                        entityList.Add(new Label(name, (string)val));
                    }
                    else if (type == typeof(bool))
                    {
                        entityList.Add(new Flag(name, (bool)val));
                    }
                    else if (type == typeof(double) || type == typeof(Int64))
                    {
                        // MiniJSON parses numbers as double or Int64
                        entityList.Add(new Counter(name, Convert.ToDouble(val)));
                    }
                    else if (type == typeof(List<object>))
                    {
                        // MiniJSON parses arrays as List<object>
                        var list = (List<object>)val;
                        entityList.Add(new List(name, list.ToArray()));
                    }
                }

                Entities = entityList.ToArray();
            }
        }

        public override string ToString()
        {
            string outputs = "[";
            if (Outputs != null)
            {
                foreach (var o in Outputs)
                {
                    outputs += o.ToString().Replace("\n", "\n\t") + ",";
                }
            }

            outputs = outputs.Trim(',') + "]";

            string entities = "[";
            if (Entities != null)
            {
                foreach (var e in Entities)
                {
                    entities += e.ToString().Replace("\n", "\n\t") + ",";
                }
            }

            entities = entities.Trim(',') + "]";

            var status = Status.ToString().Replace("\n", "\n\t") + ",";

            return "{\n" +
            "\tStatus: " + status + "\n" +
            "\tOutputs: " + outputs + "\n" +
            "\tEntities: " + entities + "\n" +
            "\tTimedResponseInterval: " + TimedResponseInterval + "\n" +
            "\tConversationId: " + ConversationId + "\n" +
            "\tParticpantId: " + ParticipantId + "\n" +
            "\tETag: " + ETag + "\n" +
            "\tLastModified: " + LastModified + "\n" +
            "\tAsrHypothesis: " + AsrHypothesis + "\n" +
            "}";
        }
    }

    /// <summary>
    /// Describe the status and any errors from a Web API response.
    /// </summary>
    public class Status
    {
        public bool Success { get; set; }

        public long StatusCode { get; set; }

        public string ErrorMessage { get; set; }

        public Status()
        {
            Success = true;
            StatusCode = 200;
            ErrorMessage = string.Empty;
        }

        override public string ToString()
        {
            return "{\n" +
            "\tSuccess: " + (Success ? "true" : "false") + "\n" +
            "\tStatusCode: " + StatusCode + "\n" +
            "\tErrorMessage: " + ErrorMessage + "\n" +
            "}";
        }
    }
}
