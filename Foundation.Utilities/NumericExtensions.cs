namespace Foundation.Utilities
{
    using System;
    using System.Globalization;
    using System.Text;

    public static class NumericExtensions
    {
        private static readonly string[] Ones =
        {
            "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Twenty"
        };

        private static readonly string[] Tens =
        {
            "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
        };

        private static readonly string[] Magnatude =
        {
            "Hundred", "Thousand", "Million", "Billion", "Trillion"
        };

        public static string ToWords(this int value)
        {
            var sb = new StringBuilder();
            NumberToString(sb, value);

            return sb.ToString();
        }

        public static string ToWords(this long value)
        {
            var sb = new StringBuilder();
            NumberToString(sb, value);

            return sb.ToString();
        }

        internal static void NumberToString(StringBuilder sb, long value)
        {
            var factor = 0;
            var magnatude = 1L;
            const long scale = 1000L;
            while (magnatude * scale <= value)
            {
                magnatude *= scale;
                factor++;
            }
            while (factor > 0)
            {
                var number = Math.DivRem(value, magnatude, out value);
                MagnatudeValueToString(sb, number);
                sb.Append(Magnatude[factor]);
                magnatude /= 1000L;
                factor--;
            }
            if (value > 0)
            {
                MagnatudeValueToString(sb, value);
            }
        }

        internal static void MagnatudeValueToString(StringBuilder sb, long value)
        {
            if (value >= 100)
            {
                var number = Math.DivRem(value, 100, out value);
                sb.Append(Ones[number]);
                sb.Append(Magnatude[0]);
            }
            if (value > 20)
            {
                var number = Math.DivRem(value, 10, out value);
                sb.Append(Tens[number]);
                sb.Append(Ones[value]);
            }
            else if (value > 0)
            {
                sb.Append(Ones[value]);
            }
        }

        public static int GetDecimalPlaces(this decimal number)
        {
            var numberString = number.ToString(CultureInfo.InvariantCulture);
            var splitDecimal = numberString.Split('.');

            return splitDecimal.Length == 2 ? splitDecimal[1].Length : 0;
        }
    }
}