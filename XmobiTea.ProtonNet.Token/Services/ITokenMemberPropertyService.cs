using System.Reflection;
using XmobiTea.Linq;
using XmobiTea.ProtonNet.Token.Attributes;

namespace XmobiTea.ProtonNet.Token.Services
{
    /// <summary>
    /// This interface defines a service that retrieves properties for a given type.
    /// </summary>
    interface ITokenMemberPropertyService
    {
        /// <summary>
        /// Gets properties of the specified type that meet specific criteria.
        /// </summary>
        /// <param name="type">The type for which properties should be retrieved.</param>
        /// <returns>An enumerable of PropertyInfo objects for the specified type.</returns>
        System.Collections.Generic.IEnumerable<PropertyInfo> GetProperties(System.Type type);

    }

    /// <summary>
    /// A concrete implementation of the ITokenMemberPropertyService.
    /// This class caches the properties of types that have the TokenMemberAttribute applied.
    /// </summary>
    class TokenMemberPropertyService : ITokenMemberPropertyService
    {
        /// <summary>
        /// A thread-safe dictionary to store and cache the properties of types.
        /// </summary>
        private System.Collections.Concurrent.ConcurrentDictionary<System.Type, System.Collections.Generic.IEnumerable<PropertyInfo>> tokenMemberPropertyDict { get; }

        /// <summary>
        /// Initializes a new instance of the TokenMemberPropertyService class.
        /// </summary>
        public TokenMemberPropertyService() => this.tokenMemberPropertyDict = new System.Collections.Concurrent.ConcurrentDictionary<System.Type, System.Collections.Generic.IEnumerable<PropertyInfo>>();

        /// <summary>
        /// Retrieves the properties of the specified type that are marked with the TokenMemberAttribute.
        /// </summary>
        /// <param name="type">The type for which to retrieve the properties.</param>
        /// <returns>An enumerable of PropertyInfo objects representing the properties of the type.</returns>
        public System.Collections.Generic.IEnumerable<PropertyInfo> GetProperties(System.Type type)
        {
            if (!this.tokenMemberPropertyDict.TryGetValue(type, out var answer))
            {
                answer = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(x => x.GetCustomAttribute<TokenMemberAttribute>() != null);

                this.tokenMemberPropertyDict.TryAdd(type, answer);
            }

            return answer;
        }

    }

}
