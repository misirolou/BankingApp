using System;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;

namespace App1.UWP
{
    internal class Speech : ITextSpeech
    {
        public async void Speak(string text)
        {
            using (var speech = new SpeechSynthesizer())
            {
                var stream = await speech.SynthesizeTextToStreamAsync(text);
                var mediaElement = new MediaElement();
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();
            }
        }
    }
}