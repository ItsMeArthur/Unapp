using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Una.Presentation.Wasm.Services.Providers
{
    internal class RequestProvider : IRequestProvider
    {
        private readonly Lazy<HttpClient> _httpClient =
        new(() =>
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }, LazyThreadSafetyMode.ExecutionAndPublication);

        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            HttpClient httpClient = GetOrCreateHttpClient();

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(uri).ConfigureAwait(false);
                await HandleResponse(response).ConfigureAwait(false);
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResult>(responseContent);
                return result;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<TResult> PostAsync<TResult>(string uri, object data, string header = "")
        {
            HttpClient httpClient = GetOrCreateHttpClient();

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var requestContent = new StringContent(JsonConvert.SerializeObject(data));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, requestContent).ConfigureAwait(false);

            await HandleResponse(response).ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResult>(responseContent);
            return result;
        }

        public async Task<TResult> PutAsync<TResult>(string uri, object data, string header = "")
        {
            HttpClient httpClient = GetOrCreateHttpClient();

            if (!string.IsNullOrEmpty(header))
            {
                AddHeaderParameter(httpClient, header);
            }

            var requestContent = new StringContent(JsonConvert.SerializeObject(data));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PutAsync(uri, requestContent).ConfigureAwait(false);

            await HandleResponse(response).ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResult>(responseContent);
            return result;
        }

        public async Task<bool> DeleteAsync(string uri)
        {
            var httpClient = GetOrCreateHttpClient();
            var response = await httpClient.DeleteAsync(uri).ConfigureAwait(false);
            await HandleResponse(response).ConfigureAwait(false);
            return true;
        }

        private HttpClient GetOrCreateHttpClient()
        {
            var httpClient = _httpClient.Value;

            // TODO: Implement JWT Bearer Authentication
            if (false)
            {
                AddAuthenticationHeader(httpClient);
            }

            return httpClient;
        }

        private static void AddHeaderParameter(HttpClient httpClient, string parameter)
        {
            if (httpClient == null)
                return;

            if (string.IsNullOrEmpty(parameter))
                return;

            httpClient.DefaultRequestHeaders.Add(parameter, Guid.NewGuid().ToString());
        }

        private static void AddAuthenticationHeader(HttpClient httpClient)
        {
            if (httpClient == null)
                return;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "<token>");
        }

        private static async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new HttpRequestException(response.StatusCode.ToString() + " - " + content);
            }
        }
    }
}
