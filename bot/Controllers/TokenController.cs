// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using CoreBot.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.BotBuilderSamples.Controllers
{
    [Route("api/token")]
    [ApiController]
    [EnableCors("AllowCrossOrigins")]
    public class TokenController : ControllerBase
    {
        private string _speechServiceApiKey = "01524171d68842cd8f56090e84dff1d1";
        private string _speechServiceRegion = "southeastasia";

        private string _directLineSecret = "zVELveKIeWY.8mfZg1kssr496rkMhQCqXZeWnV9ZwQqyz-xvnEp6Sis";

        [HttpGet, HttpPost]
        [Route("directline")]
        public async Task<ObjectResult> GetDirectLineTokenAsync()
        {
            var userId = $"dl_{Guid.NewGuid()}";
            var token = new DirectLineAuthenticator(_directLineSecret, userId).GetAccessToken();

            var config = new ChatConfig()
            {
                authorizationToken = token,
                token = token,
                userId = userId,
                region = _speechServiceRegion
            };

            return Ok(config);
        }


        [HttpGet, HttpPost]
        [Route("speech")]
        public async Task<ObjectResult> GetSpeechTokenAsync()
        {
            var token = new SpeechAuthenticator(_speechServiceApiKey, _speechServiceRegion).GetAccessToken();
            var userId = $"dl_{Guid.NewGuid()}";

            var config = new ChatConfig()
            {
                authorizationToken = token,
                token = token,
                userId = userId,
                region = _speechServiceRegion
            };

            return Ok(config);
        }
    }

    public class ChatConfig
    {
        public string authorizationToken { get; set; }
        public string token { get; set; }
        public string userId { get; set; }
        public string region { get; set; }
    }
}
