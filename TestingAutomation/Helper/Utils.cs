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

        public static string RandomGuid() =>
            Guid.NewGuid().ToString();

        public static string RandomEAN(int length= 13)
        {
            if (length != 8 && length != 13)
                throw new InvalidDataException("Ean can be only 8 or 13 digits long!");
            var ean = RandomDigits(--length).ToString();
            ean += CalculateEanCheckDigit(ean);
            return ean;
        }        

        public static string CountryCodeNumeric(string countryCode)
        {
            switch (countryCode)
            {
                case "ES": return "724";
                case "PT": return "620";
                case "TR": return "792";
                default: throw new Exception($"The is no numeric country code defined for {countryCode}!");
            }
        }

        private static int CalculateEanCheckDigit(string barcode)
        {
            var reversed = barcode.Reverse().ToArray();
            var sum = Enumerable
                .Range(0, reversed.Count())
                .Sum(i => (int)char.GetNumericValue(reversed[i]) * (i % 2 == 0 ? 3 : 1));
            return (10 - sum % 10) % 10;
        }
    }
}