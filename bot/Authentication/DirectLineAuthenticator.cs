using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.BotBuilderSamples.Controllers;
using Newtonsoft.Json;

namespace CoreBot.Authentication
{
    public class DirectLineAuthenticator
    {
        private readonly string _token;

        public DirectLineAuthenticator(string directlineSecret, string userId)
        {
            var fetchTokenUri = "https://directline.botframework.com/v3/directline/tokens/generate";
            _token = FetchTokenAsync(fetchTokenUri, directlineSecret, userId).Result;
        }

        public string GetAccessToken()
        {
            return _token;
        }

        private async Task<string> FetchTokenAsync(string fetchUri, string directlineSecret, string userId)
        {
            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, fetchUri);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", directlineSecret);
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(new { User = new { Id = userId } }),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.SendAsync(request);
                string token = string.Empty;

                if (response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    token = JsonConvert.DeserializeObject<DirectLineToken>(body).token;
                }

                return token;
            }
        }
    }

    public class DirectLineToken
    {
        public string conversationId { get; set; }
        public string token { get; set; }
        public int expires_in { get; set; }
    }
}
