//
// Assets/Scripts/PullString/Phoneme.cs
//
// Encapsulate a phoneme within a response from the PullString Web API.
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
    /// Describe a single phoneme for an audio response, e.g., to drive automatic lip sync.
    /// </summary>
    public class Phoneme
    {
        public string Name { get; private set; }

        public double SecondsSinceStart { get; private set; }

        public Phoneme(Dictionary<string, object> json)
        {
            Name = (string)json.GetValue(Keys.Name);
            if (json.ContainsKey(Keys.SecondsSinceStart))
            {
                SecondsSinceStart = (double)json[Keys.SecondsSinceStart];
            }
        }

        override public string ToString()
        {
            return "{\n" +
            "\tName: " + Name + "\n" +
            "\tSecondsSinceStart: " + SecondsSinceStart + "\n" +
            "}";
        }
    }
}

