using OzonEdu.MerchApi.DTO;

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Client
{
    public class MerchClient : IMerchClient
    {
        private readonly HttpClient _httpClient;

        public MerchClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<CheckWasIssuedMerchResponse> CheckWasIssuedMerch(
            CheckWasIssuedMerchRequest request, CancellationToken token)
        {
            using HttpResponseMessage response = await _httpClient.PostAsync("api/merch/check-was-issued-merch", request, token);
            return await response.Content.ReadAsStringAsync<CheckWasIssuedMerchResponse>(token);
        }

        public async Task<IssueMerchResponse> IssueMerch(
                    IssueMerchRequest request, CancellationToken token)
        {
            using HttpResponseMessage response = await _httpClient.PostAsync("api/merch/issue-merch", request, token);
            return await response.Content.ReadAsStringAsync<IssueMerchResponse>(token);
        }
    }

    static internal class HttpClientExtension
    {
        public static async Task<HttpResponseMessage> PostAsync(this HttpClient client,
            string requestUri, object request, CancellationToken token)
        {
            StringContent stringContent = new(JsonSerializer.Serialize(request));
            return await client.PostAsync(requestUri, stringContent, token);
        }
    }

    static internal class HttpContentExtension
    {
        public static async Task<T> ReadAsStringAsync<T>(this HttpContent content, CancellationToken token)
        {
            string body = await content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<T>(body);
        }
    }
}