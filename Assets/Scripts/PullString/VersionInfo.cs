//
// Assets/Scripts/PullString/VersionInfo.cs
//
// Encapsulate version information for PullString's Web API.
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//

namespace PullString
{
    /// <summary>
    /// Define features to check if they are supported.
    ///
    /// * StreamingAsr
    /// </summary>
    public enum FeatureName
    {
        StreamingAsr
    }

    /// <summary>
    /// Encapsulates version information for PullString's Web API.
    /// </summary>
    public class VersionInfo
    {
        /// <summary>
        /// The public-facing endpoint of the PullString Web API.
        /// </summary>
        public const string ApiBaseUrl = "https://conversation.pullstring.ai/v1/";

        /// <summary>
        /// Check if the SDK currently supports a feature.
        /// </summary>
        /// <param name="feature">a FeatureName value</param>
        /// <returns>true if the feature is supported.</returns>
        public static bool HasFeature(FeatureName feature)
        {
            switch (feature)
            {
                case FeatureName.StreamingAsr:
                    return true;
                default:
                    return false;
            }
        }
    }
}
