using Newtonsoft.Json;
using System.Text;

namespace Maze.Challenge.Client
{
    public class CustomHttpClient
    {
        private readonly HttpClient _httpClient;

        public CustomHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> Get<TResponse>(string url)
        {
            var response = await _httpClient.GetAsync(url).ConfigureAwait(true);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ToString());
            }

            var outputService = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<TResponse>(outputService);

        }

        public async Task<TResponse> Post<TRequest, TResponse>(string url, TRequest request)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(url, content).ConfigureAwait(true);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ToString());
            }

            var outputService = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<TResponse>(outputService);
        }

    }
}
