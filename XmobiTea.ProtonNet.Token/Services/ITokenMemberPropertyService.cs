using System.Reflection;
using XmobiTea.Linq;
using XmobiTea.ProtonNet.Token.Attributes;

namespace XmobiTea.ProtonNet.Token.Services
{
    /// <summary>
    /// Interface cung cấp các phương thức để lấy thông tin thuộc tính của các đối tượng token.
    /// </summary>
    interface ITokenMemberPropertyService
    {
        /// <summary>
        /// Lấy danh sách các thuộc tính của một loại (type).
        /// </summary>
        /// <param name="type">Loại (type) cần lấy thuộc tính.</param>
        /// <returns>
        /// Trả về một IEnumerable chứa các thuộc tính của loại (type) được chỉ định.
        /// </returns>
        System.Collections.Generic.IEnumerable<PropertyInfo> GetProperties(System.Type type);

    }

    /// <summary>
    /// Lớp triển khai dịch vụ lấy thông tin thuộc tính của các đối tượng token.
    /// </summary>
    class TokenMemberPropertyService : ITokenMemberPropertyService
    {
        /// <summary>
        /// Bộ từ điển lưu trữ thông tin về các thuộc tính của các loại (type).
        /// </summary>
        private System.Collections.Generic.IDictionary<System.Type, System.Collections.Generic.IEnumerable<PropertyInfo>> tokenMemberPropertyDict { get; }

        /// <summary>
        /// Khởi tạo một đối tượng mới của TokenMemberPropertyService.
        /// </summary>
        public TokenMemberPropertyService()
        {
            this.tokenMemberPropertyDict = new System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.IEnumerable<PropertyInfo>>();
        }

        /// <summary>
        /// Lấy danh sách các thuộc tính của một loại (type).
        /// </summary>
        /// <param name="type">Loại (type) cần lấy thuộc tính.</param>
        /// <returns>
        /// Trả về một IEnumerable chứa các thuộc tính của loại (type) được chỉ định.
        /// </returns>
        public System.Collections.Generic.IEnumerable<PropertyInfo> GetProperties(System.Type type)
        {
            if (!this.tokenMemberPropertyDict.TryGetValue(type, out var answer))
            {
                answer = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(x => x.GetCustomAttribute<TokenMemberAttribute>() != null);

                this.tokenMemberPropertyDict[type] = answer;
            }

            return answer;
        }

    }

}
