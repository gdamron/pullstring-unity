//
// Assets/Scripts/PullString/Request.cs
//
// Encapsulate a request to the PullString Web API.
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//
namespace PullString
{
    /// <summary>
    /// The asset build type to request for Web API requests.
    ///
    /// * EBuildType.Production (default)
    /// * EBuildType.Staging
    /// * EBuildType.Sandbox
    ///</summary>
    public class EBuildType
    {
        public const string Production = "production";
        public const string Staging = "staging";
        public const string Sandbox = "sandbox";
    }

    /// <summary>
    /// Describe the parameters for a request to the PullString Web API.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Your API key, required for all requests.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Identifies state to the Web API and can persist across sessions.
        /// </summary>
        public string ParticipantId { get; set; }

        /// <summary>
        /// Defaults to EBuildType.Production.
        /// </summary>
        public string BuildType { get; set; }

        /// <summary>
        /// Identifies an ongoing conversation to the Web API and can persist across sessions. It is required after a
        /// conversation is started.
        /// </summary>
        public string ConversationId { get; set; }

        /// <summary>
        /// ASR language; defaults to 'en-US'.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// User locale; defaults to'en-US'.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// A value in seconds representing the offset in UTC. For example, PST would be -28800.
        /// </summary>
        public int TimezoneOffset { get; set; }

        /// <summary>
        /// Restart this conversation if a newer version of the project has been published. Default value is true.
        /// </summary>
        public bool RestartIfModified { get; set; }

        public string AccountId { get; set; }

        public Request()
        {
            Language = "en-US";
            Locale = "en-US";
            RestartIfModified = true;
            BuildType = EBuildType.Production;
        }

        public override string ToString()
        {
            var str = "Request {";
            if (!string.IsNullOrEmpty(ApiKey))
            {
                str += "\n\tApiKey: " + ApiKey + ",";
            }
            if (!string.IsNullOrEmpty(ConversationId))
            {
                str += "\n\tConversationId: " + ConversationId + ",";
            }
            if (!string.IsNullOrEmpty(ParticipantId))
            {
                str += "\n\tParticipantId=" + ParticipantId + ",";
            }
            if (!string.IsNullOrEmpty(BuildType))
            {
                str += "\n\tBuildType=" + BuildType + ",";
            }
            if (!string.IsNullOrEmpty(Language))
            {
                str += "\n\tLanguage=" + Language + ",";
            }
            if (!string.IsNullOrEmpty(Locale))
            {
                str += "\n\tLocale=" + Locale + ",";
            }
            if (!string.IsNullOrEmpty(AccountId))
            {
                str += "\n\tAccountId=" + AccountId + ",";
            }
            if (TimezoneOffset > 0)
            {
                str += "\n\tTimezoneOffset=" + TimezoneOffset + ",";
            }
            str += "\n\tRestartIfModified=" + (RestartIfModified ? "true" : "false") + ",";
            str = str.Trim(',') + "\n}";

            return str;
        }
    }
}