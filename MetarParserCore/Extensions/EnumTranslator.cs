using System;
using System.ComponentModel;
using System.Linq;

namespace MetarParserCore.Extensions
{
    /// <summary>
    /// Additional methods for enums
    /// </summary>
    public class EnumTranslator
    {
        /// <summary>
        /// Get description of the enum value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Enum value)
        {
            var info = value.GetType().GetField(value.ToString());

            if (info?.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
                return attributes.First().Description;

            return value.ToString();
        }

        /// <summary>
        /// Get enum value by description text
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="description">Description text</param>
        /// <returns></returns>
        public static T GetValueByDescription<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            return default;
        }
    }
}
