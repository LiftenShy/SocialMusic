using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SocialMusic.Buisness.Implements
{
    public class HttpRequestBuilder
    {
        private HttpMethod _method = null;
        private string _requestUri = "";
        private HttpContent _content = null;
        private string _token = "";
        private string _acceptHeader = "application/json";
        private TimeSpan _timeout = TimeSpan.FromMinutes(1);

        public HttpRequestBuilder()
        {
        }

        public HttpRequestBuilder AddMethod(HttpMethod method)
        {
            _method = method;
            return this;
        }

        public HttpRequestBuilder AddRequestUri(string requestUri)
        {
            _requestUri = requestUri;
            return this;
        }

        public HttpRequestBuilder AddContent(HttpContent content)
        {
            _content = content;
            return this;
        }

        public HttpRequestBuilder AddBearerToken(string token)
        {
            _token = token;
            return this;
        }

        public HttpRequestBuilder AddAcceptHeader(string acceptHeader)
        {
            _acceptHeader = acceptHeader;
            return this;
        }

        public HttpRequestBuilder AddTimeout(TimeSpan timeout)
        {
            _timeout = timeout;
            return this;
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            // Check required arguments
            //EnsureArguments();

            // Setup request
            var request = new HttpRequestMessage
            {
                Method = _method,
                RequestUri = new Uri(_requestUri)
            };

            if (_content != null)
                request.Content = _content;

            if (!string.IsNullOrEmpty(_token))
                request.Headers.Authorization =
                  new AuthenticationHeaderValue("Bearer", _token);

            request.Headers.Accept.Clear();
            if (!string.IsNullOrEmpty(_acceptHeader))
                request.Headers.Accept.Add(
                   new MediaTypeWithQualityHeaderValue(_acceptHeader));

            // Setup client
            var client = new HttpClient();
            client.Timeout = _timeout;

            return await client.SendAsync(request);
        }
    }
}
