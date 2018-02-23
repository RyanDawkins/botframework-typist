# About
This piece of middleware is to simulate that a real person is typing between messages that are sent from the bot to the user. This is done by sending a typing indicator between messages. 
The delay is determined by the number of words in the MessageActivity.Text field and the words per minute configured from the middlewares constructor.

# Usage
This piece of middleware is intended for the latest version of the BotFramework which can be built from the [v4 github repository](http://github.com/microsoft/botbuilder-dotnet). 

```c#
int wordsPerMinute = 120;
bot.Use(new TypistMiddleware(wordsPerMinute));
```