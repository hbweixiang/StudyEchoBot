using System.Collections.Generic;
using Newtonsoft.Json;

namespace StudyEchoBot.DTO.Response
{
    /// <summary>
    /// 天气查询响应类
    /// </summary>
    public class WeatherQueryResponse
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// 实时天气
        /// </summary>
        [JsonProperty("realtime")]
        public RealtimeWeather RealtimeWeather { get; set; }
    }

    /// <summary>
    /// 实际天气
    /// </summary>
    public class RealtimeWeather
    {
        /// <summary>
        /// 温度
        /// </summary>
        [JsonProperty("temperature")]
        public int Temperature { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        /// <summary>
        /// 资讯
        /// </summary>
        [JsonProperty("info")]
        public string Info { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("direct")]
        public string Direct { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("power")]
        public string Power { get; set; }

        /// <summary>
        /// 空气质量指数
        /// </summary>
        [JsonProperty("aqi")]
        public int Aqi { get; set; }
    }

    /// <summary>
    /// 将来天气
    /// </summary>
    public class FutureWeather
    {

    }
}