using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace UniverseLib.Utility
{
    /// <summary>
    /// Abstract class for parsing types from strings.
    /// <br/>
    /// Implementers of this class should essentially be static.
    /// There must be a default constructor, and method results 
    /// should not depend on any kind of internal state.
    /// </summary>
    public abstract class Parser
    {
        /// <summary>
        /// Returns true if ParseUtility is able to parse the provided Type.
        /// </summary>
        public abstract bool CanParse(Type type);

        /// <summary>
        /// Returns true if ParseUtility is able to parse the provided Type.
        /// </summary>
        public bool CanParse<T>()
            => CanParse(typeof(T));

        /// <summary>
        /// Attempt to parse the provided input into an object of the provided Type. Returns true if successful, false if not.
        /// </summary>
        public bool TryParse<T>(string input, out T obj, out Exception parseException)
        {
            bool result = TryParse(input, typeof(T), out object parsed, out parseException);
            if (parsed != null)
                obj = (T)parsed;
            else
                obj = default;
            return result;
        }

        /// <summary>
        /// Attempt to parse the provided input into an object of the provided Type. Returns true if successful, false if not.
        /// </summary>
        public abstract bool TryParse(string input, Type type, out object obj, out Exception parseException);

        /// <summary>
        /// Returns the obj.ToString() result, formatted into the format which ParseUtility would expect for user input.
        /// </summary>
        public string ToStringForInput<T>(object obj)
            => ToStringForInput(obj, typeof(T));

        /// <summary>
        /// Returns the obj.ToString() result, formatted into the format which the Parser would expect for user input.
        /// </summary>
        public abstract string ToStringForInput(object obj, Type type);

        /// <summary>
        /// Gets a default example input which can be displayed to users, for example for Vector2 this would return "0 0".
        /// </summary>
        public string GetExampleInput<T>()
            => GetExampleInput(typeof(T));

        /// <summary>
        /// Gets a default example input which can be displayed to users, for example for Vector2 this would return "0 0".
        /// </summary>
        public virtual string GetExampleInput(Type type)
            => ParseUtility.GetExampleInput(type);
    }



    /// <summary>
    /// Abstract class for defining a parser for a single type.
    /// <br/>
    /// Use <see cref="Parser"/> to define a parser for multiple types.
    /// </summary>
    public abstract class Parser<T> : Parser
    {
        public sealed override bool CanParse(Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }

        public sealed override bool TryParse(string input, Type type, out object obj, out Exception parseException)
        {
            bool result = TryParse(input, type, out T typedObj, out parseException);
            obj = typedObj;
            return result;
        }

        public sealed override string ToStringForInput(object obj, Type type)
        {
            if (type == null || obj == null)
                return null;

            if (obj is T expectedObj)
            {
                return ToStringForInput(expectedObj, type);
            }

            return null;
        }

        public abstract bool TryParse(string input, Type type, out T obj, out Exception parseException);

        public abstract string ToStringForInput(T obj, Type type);
    }
}
