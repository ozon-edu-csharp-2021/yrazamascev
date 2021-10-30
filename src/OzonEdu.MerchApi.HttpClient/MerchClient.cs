using OzonEdu.MerchApi.DTO;

using System;
using System.Net.Http;
using System.Net.Http.Json;
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
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/merch/check-was-issued-merch", request, token);

            return await response.Content.ReadFromJsonAsync<CheckWasIssuedMerchResponse>(cancellationToken: token);
        }

        public async Task<IssueMerchResponse> IssueMerch(
                    IssueMerchRequest request, CancellationToken token)
        {
            using HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/merch/issue-merch", request, token);

            return await response.Content.ReadFromJsonAsync<IssueMerchResponse>(cancellationToken: token);
        }
    }
}