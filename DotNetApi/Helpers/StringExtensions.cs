using System.Text;

namespace DotNetApi.Helpers
{
    public static class StringExtensions
    {
        public static string EncodeToHexString(this string normalString)
        {
            StringBuilder stringBuilder = new();
            byte[] bytes = Encoding.Unicode.GetBytes(normalString);
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        public static string DecodeFromHexString(this string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return Encoding.Unicode.GetString(bytes);
        }
    }
}
