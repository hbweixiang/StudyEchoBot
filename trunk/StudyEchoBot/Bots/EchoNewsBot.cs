using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StudyEchoBot.Common;
using StudyEchoBot.Controllers;
using StudyEchoBot.DTO.Response;

namespace StudyEchoBot.Bots
{
    /// <summary>
    /// 新闻机器人
    /// </summary>
    public class EchoNewsBot : ActivityHandler
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
        private readonly ILogger<EchoNewsBot> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public EchoNewsBot(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<EchoNewsBot> logger)
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
                StringBuilder replyText = new StringBuilder();

                HttpClient httpClient = _httpClientFactory.CreateClient(nameof(StudyEchoBotConst.News));
                string response = await httpClient.GetStringAsync($"?type={text}&page=1&page_size=1&key={_configuration["NewsKey"]}");

                StudyEchoBotResponse<NewsQueryResponse> studyEchoBotResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<StudyEchoBotResponse<NewsQueryResponse>>(response);
                if (!0.Equals(studyEchoBotResponse.ErrorCode))
                {
                    replyText.Append($"{studyEchoBotResponse.Reason}，支持类型top(推荐,默认)guonei(国内)guoji(国际)yule(娱乐)tiyu(体育)junshi(军事)keji(科技)caijing(财经)shishang(时尚)youxi(游戏)qiche(汽车)jiankang(健康)");
                }
                else
                {
                    NewsQueryResponse newsQueryResponse = studyEchoBotResponse.Result;
                    List<News> newses = newsQueryResponse.Data;
                    foreach (News news in newses)
                    {
                        replyText.AppendLine($"类型：{news.Category} \r\n 标题：{news.Title} \r\n 作者名称：{news.AuthorName} \r\n");
                    }
                }
                await turnContext.SendActivityAsync(MessageFactory.Text(replyText.ToString()), cancellationToken);
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