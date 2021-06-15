﻿using System.Collections.Generic;

namespace Firebase.Authentication
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    internal static class EnumExtensions
    {
        private static readonly Dictionary<string, string> EnumCache = new Dictionary<string, string>();

        /// <summary>
        /// Finds the <see cref="EnumMemberAttribute"/> on given enum and returns its value.
        /// </summary>
        public static string ToEnumMemberString<T>(this T type) where T : Enum
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, type);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException($"Failed to parse {type} name");
            }

            if (EnumCache.TryGetValue(name, out var value))
            {
                return value;
            }

            value = ((EnumMemberAttribute[])enumType.GetTypeInfo()
                .DeclaredFields.First(fieldInfo => fieldInfo.Name == name)
                .GetCustomAttributes(typeof(EnumMemberAttribute), true))
                .Single()
                .Value;

            EnumCache.Add(name, value);
            return value;
        }
    }
}
