//
// Assets/Scripts/PullString/Entity.cs
//
// Encapsulate an entity within a response from the PullString Web API.
//
// Copyright (c) 2016 PullString, Inc.
//
// The following source code is licensed under the MIT license.
// See the LICENSE file, or https://opensource.org/licenses/MIT.
//

namespace PullString
{
    /// <summary>
    /// Define the list of entity types
    ///
    /// * EEntityType.Label
    /// * EEntityType.Counter
    /// * EEntityType.Flag
    /// * EEntityType.List
    /// </summary>
    public class EEntityType
    {
        public const string Label = "label";
        public const string Counter = "counter";
        public const string Flag = "flag";
        public const string List = "list";
    }

    /// <summary>
    /// Base class to describe a single entity, such as a label, counter, flag, or list.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        /// EEntityType for this object, i.e.
        ///
        /// * EEntityType.Label
        /// * EEntityType.Counter
        /// * EEntityType.Flag
        /// * EEntityType.List
        /// </summary>
        public abstract string Type { get; }

        /// <summary>
        /// Descriptive name of entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The value of the entity, the type of which will vary depending on the subclass.
        ///
        /// * Label: string
        /// * Counter: double
        /// * Flag: bool
        /// * List: object[]
        /// </summary>
        public object Value { get; set; }

        override public string ToString()
        {
            return "{\n" +
            "\tType: " + Type + "\n" +
            "\tName: " + Name + "\n" +
            "\tValue: " + Value + "\n" +
            "}";
        }
    }

    /// <summary>
    /// Subclass of Entity to describe a single Label.
    /// </summary>
    public class Label : Entity
    {
        /// <returns>EEntityType.Label</returns>
        override public string Type { get { return EEntityType.Label; } }

        /// <summary>
        /// Safely cast Value object as string.
        /// </summary>
        /// <returns>Value as a string or string.Empty</returns>
        public string StringValue
        {
            get
            {
                if (Value is string)
                {
                    return (string)Value;
                }
                return string.Empty;
            }
        }

        public Label(string name, string val)
        {
            Name = name;
            Value = val;
        }
    }

    /// <summary>
    /// Subclass of Entity to describe a single counter.
    /// </summary>
    public class Counter : Entity
    {
        /// <returns>EEntityType.Counter</returns>
        override public string Type { get { return EEntityType.Counter; } }

        /// <summary>
        /// Safely cast Value as a double
        /// </summary>
        /// <returns>Value as a double or -1</returns>
        public double DoubleValue
        {
            get
            {
                if (Value is double)
                {
                    return (double)Value;
                }
                return -1.0;
            }
        }

        public Counter(string name, double val)
        {
            Name = name;
            Value = val;
        }
    }

    /// <summary>
    /// Subclass of Entity to describe a single Flag.
    /// </summary>
    public class Flag : Entity
    {
        /// <returns>EEntityType.Flag</returns>
        override public string Type { get { return EEntityType.Flag; } }

        /// <summary>
        /// Safely cast Value as a bool.
        /// </summary>
        /// <returns>Value as a bool or false</returns>
        public bool BoolValue
        {
            get
            {
                if (Value is bool)
                {
                    return (bool)Value;
                }
                return false;
            }
        }

        public Flag(string name, bool val)
        {
            Name = name;
            Value = val;
        }
    }

    /// <summary>
    /// Subclass of Entity to describe a single List.
    /// </summary>
    public class List : Entity
    {
        /// <returns>EEntityType.List</returns>
        override public string Type { get { return EEntityType.List; } }

        /// <summary>
        /// Safely cast Value as an object[]
        /// </summary>
        /// <returns></returns>
        public object[] ArrayValue
        {
            get
            {
                if (Value is object[])
                {
                    return (object[])Value;
                }
                return null;
            }
        }

        public List(string name, object[] val)
        {
            Name = name;
            Value = val;
        }
    }
}

