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
    /// 新闻
    /// </summary>
    [Route("api/news")]
    [ApiController]
    public class NewsBotController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter _adapter;
        private readonly IBot _bot;

        public NewsBotController(IBotFrameworkHttpAdapter adapter, EchoNewsBot newsBot)
        {
            _adapter = adapter;
            _bot = newsBot;
        }

        [HttpPost, HttpGet]
        public async Task PostAsync()
        {
            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            await _adapter.ProcessAsync(Request, Response, _bot);
        }
    }
}
