using System.Text;
using UrlShortener.Services;
using System.Security.Cryptography;

namespace UrlShortener.Helper
{
    public class UrlShortenerHelper
    {
        public UrlShortenerHelper()
        {
        }
        private string Hash(string input)
        {
            using var sha1 = SHA1.Create();
            return Convert.ToHexString(sha1.ComputeHash(Encoding.UTF8.GetBytes(input))).Substring(0,6);
        }

        public string GenerateHash(string url)
        {
            int l = url.Length;
            string hash = Hash(url);
            return hash.ToString();
        }
        public bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

    }
}
