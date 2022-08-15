using System.ComponentModel.DataAnnotations;

namespace WebService.Models
{
    /// <summary>
    /// 调用蔡司ws返回的结果
    /// </summary>
    public class SocketResult
    {
        [Key]
        public Guid Id { get; set; }
        public string? Topic { get; set; }
        public string? Ref { get; set; }
        public SocketResultPayload? Payload { get; set; }
        public string? Event { get; set; }
        /// <summary>
        /// 实际结果
        /// </summary>
        public string Result { get; set; }
    }
}
