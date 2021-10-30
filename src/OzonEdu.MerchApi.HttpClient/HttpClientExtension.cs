using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OzonEdu.MerchApi.Client
{
    static internal class HttpClientExtension
    {
        public static async Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient client,
            string requestUri, object request, CancellationToken token)
        {
            StringContent stringContent = new(JsonSerializer.Serialize(request));
            return await client.PostAsync(requestUri, stringContent, token);
        }
    }
}