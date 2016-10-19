//
// Assets/Scripts/PullString/ParameterValue.cs
//
// Encapsulate arbitrary values within a response from the PullString Web API.
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
    /// An arbitrary value passed to an output or entity within a Response from the Web API.
    /// </summary>
    public class ParameterValue
    {
        /// <summary>
        /// Safely cast to a string.
        /// </summary>
        /// <returns>The parameter as astring or string.Empty</returns>
        public string StringValue
        {
            get
            {
                return stringParam;
            }
            set
            {
                updateParamValues(value);
            }
        }

        /// <summary>
        /// Safely cast to a double.
        /// </summary>
        /// <returns>The parameter as a double or 0</returns>
        public double NumericValue
        {
            get
            {
                return doubleParam;
            }
            set
            {
                updateParamValues(value);
            }
        }

        /// <summary>
        /// Safely cast to a bool.
        /// </summary>
        /// <returns>The parameter as a bool or false</returns>
        public bool BoolValue
        {
            get
            {
                return boolParam;
            }
            set
            {
                updateParamValues(value);
            }
        }

        /// <summary>
        /// Safely cast to List of ParameterValue
        /// </summary>
        /// <returns>A List or null</returns>
        public List<ParameterValue> ListValue
        {
            get
            {
                return listParam;
            }
            set
            {
                updateParamValues(value);
            }
        }

        /// <summary>
        /// Safely cast to a string.
        /// </summary>
        /// <returns>The parameter as string or string.Empty</returns>
        public Dictionary<string, ParameterValue> DictionaryValue
        {
            get
            {
                return dictParam;
            }
            set
            {
                updateParamValues(value);
            }
        }

        /// <summary>
        /// The raw object
        /// </summary>
        public object Parameter
        {
            get
            {
                return objectParam;
            }
            set
            {
                updateParamValues(value);
            }
        }

        private string stringParam;
        private double doubleParam;
        private bool boolParam;
        private List<ParameterValue> listParam;
        private Dictionary<string, ParameterValue> dictParam;
        private object objectParam;

        public ParameterValue(object parameter)
        {
            updateParamValues(parameter);
        }

        override public string ToString()
        {
            return "{\n" +
            "\tString: " + stringParam + "\n" +
            "\tNumeric: " + doubleParam + "\n" +
            "\tBool: " + boolParam + "\n" +
            "\tList: " + listParam + "\n" +
            "\tDictionary: " + dictParam + "\n" +
            "}";
        }

        private void updateParamValues(object newValue)
        {
            objectParam = newValue;
            stringParam = (newValue is string) ? (string)newValue : string.Empty;
            doubleParam = newValue.IsNumeric() ? (double)newValue : 0;
            boolParam = newValue is bool ? (bool)newValue : false;

            if (newValue is List<ParameterValue>)
            {
                listParam = (List<ParameterValue>)newValue;
            }
            else
            {
                listParam = null;
            }

            if (newValue is Dictionary<string, ParameterValue>)
            {
                dictParam = (Dictionary<string, ParameterValue>)newValue;
            }
            else
            {
                dictParam = null;
            }
        }
    }
}

