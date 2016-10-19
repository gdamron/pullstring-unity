//
// Assets/Scripts/PullString/Output.cs
//
// Encapsulate an output within a response from the PullString Web API.
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//

using System.Collections.Generic;

namespace PullString
{
    /// <summary>
    /// Define the set of outputs that can be returned in a response.
    ///
    /// * EOutputType.Dialog
    /// * EOutputType.Behavior
    /// </summary>
    public class EOutputType
    {
        public const string Dialog = "dialog";
        public const string Behavior = "behavior";
    }

    /// <summary>
    /// Base class for outputs that are of type dialog or behavior.
    /// </summary>
    public abstract class Output
    {
        /// <summary>
        /// An EOutputType, i.e.
        ///
        /// * EOutputType.Dialog
        /// * EOutputType.Behavior
        /// </summary>
        public abstract string Type { get; }

        /// <summary>
        /// Unique identifier for this Output.
        /// </summary>
        public string Guid { get; protected set; }
    }

    /// <summary>
    /// Subclass of Output that represents a dialog response
    /// </summary>
    public class DialogOutput : Output
    {
        /// <returns>EOutputType.Dialog</returns>
        override public string Type { get { return EOutputType.Dialog; } }

        /// <summary>
        /// A character's text response.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Location of recorded audio, if available.
        /// </summary>
        public string AudioUri { get; set; }

        /// <summary>
        /// Location of recorded video, if available.
        /// </summary>
        public string VideoUri { get; set; }

        /// <summary>
        /// The speaking character.
        /// </summary>
        public string Character { get; set; }

        /// <summary>
        /// Duration of spoken line in seconds.
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// Optional arbitrary string data that was associated with the dialog line within PullString Author.
        /// </summary>
        public string UserData { get; set; }

        /// <summary>
        /// Optional arbitrary string data that was associated with the dialog line within PullString Author.
        /// </summary>
        public Phoneme[] Phonemes { get; set; }

        public DialogOutput(Dictionary<string, object> json)
        {
            Guid = (string)json.GetValue(Keys.Guid);
            Text = (string)json.GetValue(Keys.Text);
            AudioUri = (string)json.GetValue(Keys.Uri);
            VideoUri = (string)json.GetValue(Keys.VideoFile);
            Character = (string)json.GetValue(Keys.Character);
            UserData = (string)json.GetValue(Keys.UserData);

            if (json.ContainsKey(Keys.Duration))
            {
                Duration = (double)json[Keys.Duration];
            }

            if (!json.ContainsKey(Keys.Phonemes))
            {
                return;
            }

            var phonemes = (Dictionary<string, object>[])json[Keys.Phonemes];
            Phonemes = new Phoneme[phonemes.Length];
            int i = 0;
            foreach (var p in phonemes)
            {
                Phonemes[i] = new Phoneme(p);
                i++;
            }
        }

        public DialogOutput() {/* default constructor */}

        public override string ToString()
        {
            string phonemes = "[";

            if (Phonemes != null)
            {
                foreach (var p in Phonemes)
                {
                    phonemes += p.ToString().Replace("\n", "\n\t") + ",";
                }
            }

            phonemes = phonemes.Trim(',') + "]";

            return "{\n" +
            "\tType: " + Type + "\n" +
            "\tText: " + Text + "\n" +
            "\tGuid: " + Guid + "\n" +
            "\tUri: " + AudioUri + "\n" +
            "\tVideoFile: " + VideoUri + "\n" +
            "\tDuration: " + Duration + "\n" +
            "\tCharacter: " + Character + "\n" +
            "\tPhonemes: " + phonemes + "\n" +
            "\tUserData: " + UserData + "\n" +
            "}";
        }
    }

    /// <summary>
    /// Subclass of Output that represents a behavior response
    /// </summary>
    public class BehaviorOutput : Output
    {
        /// <returns>EOutputType.Behavior</returns>
        override public string Type { get { return EOutputType.Behavior; } }

        /// <summary>
        /// The name of the behavior.
        /// </summary>
        public string Behavior { get; set; }

        /// <summary>
        /// A dictionary with any parameters defined for the behavior
        /// </summary>
        public Dictionary<string, ParameterValue> Parameters { get; set; }

        public BehaviorOutput(Dictionary<string, object> json)
        {
            Guid = (string)json.GetValue(Keys.Guid);
            Behavior = (string)json.GetValue(Keys.Behavior);
            var paramDict = (Dictionary<string, object>)json.GetValue(Keys.Parameters);
            if (paramDict != null)
            {
                Parameters = new Dictionary<string, ParameterValue>();
                foreach (var kv in paramDict)
                {
                    var param = new ParameterValue(kv.Value);
                    Parameters.Add(kv.Key, param);
                }
            }
        }

        public BehaviorOutput() {/* default constructor */}

        public override string ToString()
        {
            string parameters = "[";
            if (Parameters != null)
            {
                foreach (var p in Parameters)
                {
                    parameters += p.ToString().Replace("\n", "\n\t") + ",";
                }
            }


            parameters = parameters.Trim(',') + "]";

            return "{\n" +
            "\tType: " + Type + "\n" +
            "\tBehavior: " + Behavior + "\n" +
            "\tGuid: " + Guid + "\n" +
            "\tParameters: " + parameters + "\n" +
            "}";
        }
    }
}

