using System;
using System.Linq;
using System.Reflection;

namespace XamarinCognitiveServices.Enumeration
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumDescriptionAttribute : Attribute
    {
        #region private properties
        private readonly string description;
        #endregion

        #region public properties
        public string Description { get { return description; } }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:XamarinCognitiveServices.Enumeration.EnumDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">Description.</param>
        public EnumDescriptionAttribute(string description)
        {
            this.description = description;
        }
        #endregion
    }

    public static class EnumHelper
    {
        #region Public Methods
        /// <summary>
        /// Gets the type of the attribute of.
        /// </summary>
        /// <returns>The attribute of type.</returns>
        /// <param name="enumVal">Enum value.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var typeInfo = enumVal.GetType().GetTypeInfo();
            var v = typeInfo.DeclaredMembers.First(x => x.Name == enumVal.ToString());
            return v.GetCustomAttribute<T>();
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns>The description.</returns>
        /// <param name="enumVal">Enum value.</param>
        public static string GetDescription(this Enum enumVal)
        {
            var attr = GetAttributeOfType<EnumDescriptionAttribute>(enumVal);
            return attr != null ? attr.Description : string.Empty;
        }
        #endregion
    }
}
