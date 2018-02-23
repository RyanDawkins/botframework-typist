using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Middleware;
using Microsoft.Bot.Schema;
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
        const int MILISECONDS_PER_SECOND = 1000;

        public TypistMiddleware(int typistWordsPerMinute)
        {
            _typistWordsPerMinute = typistWordsPerMinute;
        }

        public async Task SendActivity(IBotContext context, IList<IActivity> activities, MiddlewareSet.NextDelegate next)
        {
            List<IMessageActivity> messageActivities = activities
                .Where(activity => activity.Type.Equals(ActivityTypes.Message))
                .Select(activity => activity.AsMessageActivity())
                .ToList();
            if (!messageActivities.Any())
            {
                await next();
                return;
            }
            
            List<IActivity> toAdd = activities.Aggregate(new List<IActivity>(), (activitiesToAdd, activity) =>
            {
                string[] words = activity.AsMessageActivity().Text.Split((char[])null);
                double timeInMinutes = ((double)words.Count() / _typistWordsPerMinute);
                int timeInMs = (int)Math.Ceiling(timeInMinutes * SECONDS_PER_MINUTE * MILISECONDS_PER_SECOND) / 2;

                Activity typingActivity = ((Activity)context.Request).CreateReply();
                typingActivity.Type = ActivityTypes.Typing;
                typingActivity.Value = timeInMs;

                Activity delayActivity = ((Activity)context.Request).CreateReply();
                delayActivity.Type = ActivityTypesEx.Delay;
                delayActivity.Value = timeInMs;

                activitiesToAdd.Add(typingActivity);
                activitiesToAdd.Add(delayActivity);
                activitiesToAdd.Add(activity);

                return activitiesToAdd;
            });
            activities.Clear();
            toAdd.ForEach(activity => activities.Add(activity));

            await next();
        }
    }
}
