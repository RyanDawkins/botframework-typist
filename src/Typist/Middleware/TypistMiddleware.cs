using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Middleware;
using Microsoft.Bot.Schema;
using RyanDawkins.Typist.Utility;
using static Microsoft.Bot.Builder.Middleware.MiddlewareSet;

namespace RyanDawkins.Typist.Middleware
{
    /// <summary>
    /// This typist middleware makes it appear as if the bot was a person typing by delaying the messages sent sequentially.
    /// </summary>
    public class TypistMiddleware : Microsoft.Bot.Builder.Middleware.IMiddleware, ISendActivity
    {

        private readonly int _typistWordsPerMinute;
        const int SECONDS_PER_MINUTE = 60;
        const int MILLISECONDS_PER_SECOND = 1000;

        public TypistMiddleware(int typistWordsPerMinute)
        {
            _typistWordsPerMinute = typistWordsPerMinute;
        }

        public async Task SendActivity(IBotContext context, IList<IActivity> activities, MiddlewareSet.NextDelegate next)
        {
            activities
                .Where(activity => activity.Type.Equals(ActivityTypes.Message))
                .Select(activity => activity.AsMessageActivity())
                .ToList()
                .ForEach(activity =>
                {
                    int wordCount = TypistUtility.GetWordCount(activity.Text);
                    int timeInMs = TypistUtility.CalculateTimeToType(_typistWordsPerMinute, wordCount);

                    Activity typingActivity = ((Activity)context.Request).CreateReply();
                    typingActivity.Type = ActivityTypes.Typing;
                    typingActivity.Value = timeInMs;

                    Activity delayActivity = ((Activity)context.Request).CreateReply();
                    delayActivity.Type = ActivityTypesEx.Delay;
                    delayActivity.Value = timeInMs;

                    int indexOfActivity = activities.IndexOf(activity);
                    activities.Insert(indexOfActivity, delayActivity);
                    activities.Insert(indexOfActivity, typingActivity);
                });

            await next();
        }
    }
}
