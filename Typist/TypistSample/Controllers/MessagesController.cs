using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Storage;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using RyanDawkins.Typist.Middleware;
using static Microsoft.Bot.Builder.Middleware.MiddlewareSet;

namespace TypistSample.Controllers
{
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        private BotFrameworkAdapter _adapter;

        /// <summary>
        /// In this sample Bot, a new instance of the Bot is created by the controller 
        /// on every incoming HTTP reques. The bot is constructed using the credentials
        /// found in the config file. Note that no credentials are needed if testing
        /// the bot locally using the emulator. 
        /// </summary>        
        public MessagesController(IConfiguration configuration)
        {
            var bot = new Bot(new BotFrameworkAdapter(configuration))
                .Use(new TypistMiddleware(90));
            bot.OnReceive(BotReceiveHandler);

            _adapter = (BotFrameworkAdapter)bot.Adapter;
        }

        private async Task BotReceiveHandler(IBotContext context, NextDelegate next)
        {
            if (context.Request.Type == ActivityTypes.Message)
            {
                context.Reply(context.Request.AsMessageActivity().Text);
            }

            await next();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Activity activity)
        {
            try
            {
                await _adapter.Receive(this.Request.Headers["Authorization"].FirstOrDefault(), activity);
                return this.Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return this.Unauthorized();
            }
        }
    }
}
