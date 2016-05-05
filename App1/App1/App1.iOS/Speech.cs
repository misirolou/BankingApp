using System;
using System.Collections.Generic;
using System.Text;
using AVFoundation;

namespace App1.iOS
{
    class Speech : ITextSpeech
    {
        float volume = 0.5f;
        float pitch = 1.0f;

        public void Speak(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                var speechSynthesizer = new AVSpeechSynthesizer();
                var speechUtterance = new AVSpeechUtterance(text)
                {
                    Rate = AVSpeechUtterance.MaximumSpeechRate / 4,
                    Voice = AVSpeechSynthesisVoice.FromLanguage("en-US"),
                    Volume = volume,
                    PitchMultiplier = pitch
                };

                speechSynthesizer.SpeakUtterance(speechUtterance);
            }
        }
    }
}
