using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using StudyEchoBot.Bots;

namespace StudyEchoBot.Controllers
{
    /// <summary>
    /// 天气
    /// </summary>
    [Route("api/weather")]
    [ApiController]
    public class WeatherBotController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter Adapter;
        private readonly IBot Bot;

        public WeatherBotController(IBotFrameworkHttpAdapter adapter, EchoWeatherBot weatherBot)
        {
            Adapter = adapter;
            Bot = weatherBot;
        }

        [HttpPost, HttpGet]
        public async Task PostAsync()
        {
            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            await Adapter.ProcessAsync(Request, Response, Bot);
        }
    }
}
