using System.Net;
using System.Text.RegularExpressions;
using AccountManager.Services.Interfaces;

namespace AccountManager.Services.Implements
{
    public class RedirectService : IRedirectService
    {
        public string ExtractRedirectUriFromReturnUrl(string url)
        {
            var result = "";
            var decodedUrl = WebUtility.HtmlDecode(url);
            var results = Regex.Split(decodedUrl, "redirect_uri=");

            if (results.Length < 2)
            {
                return "";
            }

            result = results[1];

            var splitKey = "";

            splitKey = result.Contains("signin-oidc") ? "signin-oidc" : "scope";

            results = Regex.Split(result, splitKey);

            if (results.Length < 2)
            {
                return "";
            }

            result = results[0];

            return result.Replace("%3a", ":").Replace("%2F", "/").Replace("&", "");
        }
    }
}
