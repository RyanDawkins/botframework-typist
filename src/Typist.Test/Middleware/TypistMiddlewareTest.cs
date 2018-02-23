using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Schema;
using Moq;
using RyanDawkins.Typist.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using static Microsoft.Bot.Builder.Middleware.MiddlewareSet;

namespace Typist.Test.Middleware
{
    public class TypistMiddlewareTest
    {

        const int WORDS_PER_MINUTE = 90;

        private readonly TypistMiddleware _middleware;

        public TypistMiddlewareTest()
        {
            _middleware = new TypistMiddleware(WORDS_PER_MINUTE);
        }

        /// <summary>
        /// Tests to ensure message activities have a delay and typing activity before them.
        /// </summary>
        [Fact]
        public async void EnsureCorrectActivityOrder()
        {
            List<IActivity> activities = new List<IActivity>()
            {
                new Activity()
                {
                    Type = ActivityTypes.Message,
                    Text = "Hello world"
                },
                new Activity()
                {
                    Type = ActivityTypes.Message,
                    Text = "Whoah horsey!"
                }
            };

            TestAdapter testAdapter = new TestAdapter();
            IActivity activity = testAdapter.MakeActivity("");
            BotContext context = new BotContext(testAdapter, activity);

            Mock<NextDelegate> next = new Mock<NextDelegate>();

            await _middleware.SendActivity(context, activities, next.Object);

            int messageTypingDelayCount = activities.Aggregate(0, (count, a) =>
            {
                switch (a.Type)
                {
                    case ActivityTypes.Typing:
                    case ActivityTypesEx.Delay:
                    case ActivityTypes.Message:
                        return count + 1;
                }
                return count;
            });

            Assert.True(messageTypingDelayCount % 3 == 0);

            for(int i = 0; i < activities.Count; i++)
            {
                IActivity a = activities[i];
                int relativeOrder = i % 3;
                switch(relativeOrder)
                {
                    case 0:
                        Assert.Equal(ActivityTypes.Typing, a.Type);
                        break;
                    case 1:
                        Assert.Equal(ActivityTypesEx.Delay, a.Type);
                        break;
                    case 2:
                        Assert.Equal(ActivityTypes.Message, a.Type);
                        break;
                }
            }
        }

        [Fact]
        public async void EnsureNonMessageActivityDoesNotBlowUp()
        {
            List<IActivity> activities = new List<IActivity>()
            {
                new Activity()
                {
                    Type = ActivityTypes.ConversationUpdate,
                    Text = "User joined session"
                }
            };

            TestAdapter testAdapter = new TestAdapter();
            IActivity activity = testAdapter.MakeActivity("");
            BotContext context = new BotContext(testAdapter, activity);

            Mock<NextDelegate> next = new Mock<NextDelegate>();

            await _middleware.SendActivity(context, activities, next.Object);
        }

    }
}
