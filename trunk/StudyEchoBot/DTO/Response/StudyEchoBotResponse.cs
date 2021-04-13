using Newtonsoft.Json;

namespace StudyEchoBot.DTO.Response
{
    /// <summary>
    /// 公共响应类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StudyEchoBotResponse<T>
    {
        /// <summary>
        /// 原因
        /// </summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        [JsonProperty("result")]
        public T Result { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
    }
}
