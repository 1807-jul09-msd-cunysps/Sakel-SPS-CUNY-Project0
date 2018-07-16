using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenges
{
   public static class Program
    {
        public static bool IsPalindrome(string input)
        {
            for (int i = 0; i < input.Length / 2; i++)
            {
                if (!input[i].Equals(input[input.Length - i - 1]))
                {
                    return false;
                }
            }
            return true;
        }
        static void Main(string[] args)
        {
        }
    }
}
