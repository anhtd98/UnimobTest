using System.Numerics;
using System.Text;

namespace _01_Game.Scripts.Manager
{
    public static class NumberFormatter
    {
        private static readonly string[] Suffixes =
        {
            "", "K", "M", "B", "T", "Q", "Qd", "Qn", "Sx", "Sp", "Oc", "No", "Dc"
        };

        public static string FormatShort(BigInteger value, int decimalPlaces = 1)
        {
            if (value < 1000)
                return value.ToString();

            int suffixIndex = 0;
            BigInteger unit = BigInteger.One;

            while (value >= unit * 1000 && suffixIndex < Suffixes.Length - 1)
            {
                unit *= 1000;
                suffixIndex++;
            }

            BigInteger whole = value / unit;
            BigInteger remainder = value % unit;

            if (decimalPlaces <= 0 || remainder == 0)
                return $"{whole}{Suffixes[suffixIndex]}";

            BigInteger scale = BigInteger.Pow(10, decimalPlaces);
            BigInteger decimalValue = (remainder * scale) / unit;

            if (decimalValue == 0)
                return $"{whole}{Suffixes[suffixIndex]}";

            string decimalStr = decimalValue.ToString().PadLeft(decimalPlaces, '0');
            decimalStr = TrimTrailingZeros(decimalStr);

            return $"{whole}.{decimalStr}{Suffixes[suffixIndex]}";
        }

        private static string TrimTrailingZeros(string s)
        {
            int i = s.Length - 1;
            while (i >= 0 && s[i] == '0')
                i--;

            return s.Substring(0, i + 1);
        }
    }
}