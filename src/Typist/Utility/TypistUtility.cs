using System;
using System.Collections.Generic;
using System.Text;

namespace RyanDawkins.Typist.Utility
{
    public static class TypistUtility
    {

        const int SECONDS_PER_MINUTE = 60;
        const int MILISECONDS_PER_SECOND = 1000;

        /// <summary>
        /// Method to calculate the word count of the message.
        /// </summary>
        /// <param name="message">message to count</param>
        /// <returns>the number of words in the message</returns>
        public static int GetWordCount(string message)
        {
            return message.Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        /// <summary>
        /// Calculates the time to type a message
        /// </summary>
        /// <param name="wordsPerMinute"></param>
        /// <param name="wordCount"></param>
        /// <returns></returns>
        public static int CalculateTimeToType(int wordsPerMinute, int wordCount)
        {
            double timeInMinutes = ((double)wordCount / wordsPerMinute);
            return (int)Math.Ceiling(timeInMinutes * SECONDS_PER_MINUTE * MILISECONDS_PER_SECOND);
        }

    }
}
