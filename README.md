# About
This piece of middleware is to simulate that a real person is typing between messages that are sent from the bot to the user. This is done by sending a typing indicator between messages. 
The delay is determined by the number of words in the MessageActivity.Text field and the words per minute configured from the middlewares constructor.

The purpose of this middleware is to give a more realistic feel to the bot as it simulates a person. Another reason is to prevent the user from having to scroll up to read a number of
messages that may have came in from the bot all at once.

# Installing
```bash
Install-Package RyanDawkins.Typist –IncludePrerelease
```

# Usage
```c#
int wordsPerMinute = 120;
bot.Use(new TypistMiddleware(wordsPerMinute));
```