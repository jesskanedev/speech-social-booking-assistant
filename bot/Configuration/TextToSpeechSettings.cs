namespace EchoBot.Configuration
{
    /// <summary>
    /// Text to Speech settings.
    /// Find more options documented here:
    /// Standard: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support#standard-voices
    /// Neural: https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/language-support#neural-voices
    /// </summary>
    public static class TextToSpeechSettings
    {
        /// <summary>
        /// The language the bot should reply in.
        /// </summary>
        public const string Language = "en-US";

        /// <summary>
        /// The voice the bot should reply in.
        /// </summary>
        public const string Voice = "JessaNeural";
    }
}
