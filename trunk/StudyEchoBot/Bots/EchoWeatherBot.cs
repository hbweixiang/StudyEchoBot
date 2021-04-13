using System;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StudyEchoBot.Common;
using StudyEchoBot.DTO.Response;

namespace StudyEchoBot.Bots
{
    /// <summary>
    /// 天气机器人
    /// </summary>
    public class EchoWeatherBot : ActivityHandler
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// 
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<EchoWeatherBot> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public EchoWeatherBot(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<EchoWeatherBot> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="turnContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //异常处理可迭代成全局异常，免得每个机器人都需要try catch，使用中间件
            try
            {
                string text = turnContext.Activity.Text;
                string replyText;

                HttpClient httpClient = _httpClientFactory.CreateClient(nameof(StudyEchoBotConst.Weather));
                string response = await httpClient.GetStringAsync($"?city={text}&key={_configuration["WeatherKey"]}");

                StudyEchoBotResponse<WeatherQueryResponse> studyEchoBotResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<StudyEchoBotResponse<WeatherQueryResponse>>(response);
                if (!0.Equals(studyEchoBotResponse.ErrorCode))
                {
                    replyText = $"{studyEchoBotResponse.Reason}，城市名称如：温州、上海、北京";
                }
                else
                {
                    WeatherQueryResponse weatherQueryResponse = studyEchoBotResponse.Result;
                    RealtimeWeather realtimeWeather = weatherQueryResponse.RealtimeWeather;
                    replyText = $"{weatherQueryResponse.City}当前天气{realtimeWeather.Info}{realtimeWeather.Temperature}℃ {realtimeWeather.Power}{realtimeWeather.Direct}";
                }
                await turnContext.SendActivityAsync(MessageFactory.Text(replyText), cancellationToken);
            }
            catch (Exception e)
            {
                //防止暴力异常，让系统宕机
                Thread.Sleep(1000);
                _logger.LogError($"StackTrace:{e.StackTrace}\r\nMessage:{e.Message}");
                await turnContext.SendActivityAsync(MessageFactory.Text("系统内部异常，请联系管理员"), cancellationToken);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="membersAdded"></param>
        /// <param name="turnContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}