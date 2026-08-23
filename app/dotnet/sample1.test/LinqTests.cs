
using System;
using System.Collections.Generic;
using System.Text;

namespace sample1.test
{
    public class LinqTests
    {
        [Theory]
        [InlineData("swiss", 'w')]
        [InlineData("Bal  l  ", 'b')]
        public void Should_Find_FirstNonRepeatingCharacter(string input, char expected)
        {
   
            var result = FirstNonRepeatingCharacter(input);

            Assert.Equal(expected, result);
        }

        /// <summary>
        /// finds first non repeating character
        /// case insensitive
        /// whitespace ignred
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static char FirstNonRepeatingCharacter(string input)
        {
            var group = input.ToCharArray()
                .Where(x => !char.IsWhiteSpace(x))
                .Select(x => char.ToLower(x))
                .GroupBy(c => c)
                .FirstOrDefault(g => g.Count() == 1);

            if (group != null)
                return group.Key;
            return default;
        }
    }
}
