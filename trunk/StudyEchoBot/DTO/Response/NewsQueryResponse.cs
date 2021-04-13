using System.Collections.Generic;
using Newtonsoft.Json;

namespace StudyEchoBot.DTO.Response
{
    /// <summary>
    /// 新闻查询响应类
    /// </summary>
    public class NewsQueryResponse
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("stat")]
        public string Stat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("data")]
        public List<News> Data { get; set; }

        /// <summary>
        /// 当前页数, 默认1, 最大50
        /// </summary>
        [JsonProperty("page")]
        public long Page { get; set; }

        /// <summary>
        /// 每夜返回条数, 默认30 , 最大30
        /// </summary>
        [JsonProperty("pageSize")]
        public long PageSize { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class News
    {
        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// 作者名称
        /// </summary>
        [JsonProperty("author_name")]
        public string AuthorName { get; set; }
    }
}