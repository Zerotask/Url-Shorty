using System;
using System.Security.Cryptography;
using System.Text;

namespace UrlShorty.Services
{
    public class ShortenerService : IShortenerService
    {
        public string Shorten(string value)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(new UTF8Encoding().GetBytes(value));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}