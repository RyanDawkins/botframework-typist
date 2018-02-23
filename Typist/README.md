# About
This piece of middleware is to simulate that a real person is typing between messages that are sent from the bot to the user.

# Usage
This piece of middleware is intended for the latest version of the BotFramework which can be built from the [v4 github repository](http://github.com/microsoft/botbuilder-dotnet). 

```c#
int wordsPerMinute = 120;
bot.Use(new TypistMiddleware(wordsPerMinute));
```