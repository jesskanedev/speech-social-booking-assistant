namespace CoreBot.Helpers
{
    /// <summary>
    /// Voice message helper functions.
    /// </summary>
    public static class VoiceMessageHelpers
    {
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
            var voiceMessage = message;

            //Standard custom voice
            //var voiceMessage = $"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='en-US'><voice name='Microsoft Server Speech Text to Speech Voice (en-US, Jessa24kRUS)'>{message}</voice></speak>";

            //Neural custom voice - breaks???
//            var voiceMessage = $"<speak version='1.0' xmlns='https://www.w3.org/2001/10/synthesis' xml:lang='en-US'><voice name='Microsoft Server Speech Text to Speech Voice (en-US, JessaNeural)'><mstts:express-as type='chat'>{message}</mstts:express-as></voice></speak>";

            return voiceMessage;
        }
    }
}
