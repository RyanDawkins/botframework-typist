using RyanDawkins.Typist.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Typist.Test.Utility
{
    public class TypistUtilityTest
    {
        
        [Theory]
        [InlineData(1, "hello")]
        [InlineData(2, "hello world")]
        [InlineData(2, "hello  world")]
        [InlineData(2, "hello	world")]
        public void MessageCountTest(int expected, string input)
        {
            int actual = TypistUtility.GetWordCount(input);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(60000, 1, 1)]
        public void CalculateTimeToType(int expected, int wordsPerMinute, int wordCount)
        {
            int actual = TypistUtility.CalculateTimeToType(wordsPerMinute, wordCount);
            Assert.Equal(expected, actual);
        }

    }
}
