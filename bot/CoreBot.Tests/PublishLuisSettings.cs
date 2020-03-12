using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using NUnit.Framework;

namespace CoreBot.Tests
{
    public class PublishLuisSettings
    {
        private const string LuisAppId = "43be3670-65fd-41d1-9473-21201d512144";
        private const string LuisAPIKey = "b228c00a443647fc805e22f97b0f0376";
        private const string LuisAPIHostName = "westus.api.cognitive.microsoft.com";
        private const string LuisModelVersionId = "0.1";

        private const string NormalizePunctuationSettingsJson = "[{'name': 'NormalizePunctuation', 'value': 'true'}]";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SetNormalizePunctuationSetting()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var uri = $"https://{LuisAPIHostName}/luis/api/v2.0/apps/{LuisAppId}/versions/{LuisModelVersionId}/settings?" + queryString;

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", LuisAPIKey);

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(NormalizePunctuationSettingsJson);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.PutAsync(uri, content);
            }
        }
    }
}