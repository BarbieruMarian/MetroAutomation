using System;
using System.Linq;

namespace TestFramework.Helper
{
    public static class Utils
    {
        public static string RandomString(
            int length,
            string randomChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")
        {
            Random random = new Random();
            return new string(Enumerable.Repeat(randomChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomDigits(int length) =>
            RandomString(length, "0123456789");
    }
}
