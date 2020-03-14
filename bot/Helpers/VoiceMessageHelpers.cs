using System.Text;

namespace CoreBot.Helpers
{
    /// <summary>
    /// Voice message helper functions.
    /// </summary>
    public static class VoiceMessageHelpers
    {
        private static string _voiceRegion = "en-US";
        private static string _voiceName = "JessaNeural";
        private static readonly string _voiceExpressAs = VoiceExpressionOptions.CustomerService;

        /// <summary>
        /// Wrap the message with a neural voice for more realistic text to speech.
        /// Standard voices: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support#standard-voices
        /// Neural voices: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support#neural-voices
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string WrapMessageInVoice(string message)
        {
            // No custom voice
            //var voiceMessage = message;

            //Standard custom voice
            //var voiceMessage = $"<speak version=\"1.0\" xmlns=\"https://www.w3.org/2001/10/synthesis\" xml:lang=\"{_voiceRegion}\"><voice name=\"Microsoft Server Speech Text to Speech Voice ({_voiceRegion}, Jessa24kRUS)\">{message}</voice></speak>";

            //Neural custom voice - breaks???
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"<speak version=\"1.0\" xmlns=\"https://www.w3.org/2001/10/synthesis\" xml:lang=\"{_voiceRegion}\">");
            stringBuilder.Append($"<voice name=\"Microsoft Server Speech Text to Speech Voice ({_voiceRegion}, {_voiceName})\">");
            stringBuilder.Append($"<mstts:express-as type=\"{_voiceExpressAs}\">");
            stringBuilder.Append($"{message}");
            stringBuilder.Append("</mstts:express-as>");
            stringBuilder.Append("</voice>");
            stringBuilder.Append("</speak>");

            var voiceMessage = stringBuilder.ToString();

            return voiceMessage;
        }
    }
}
