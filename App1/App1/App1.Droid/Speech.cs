using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech.Tts;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace App1.Droid
{
    public class Speech : Java.Lang.Object, ITextSpeech, TextToSpeech.IOnInitListener
    {
        TextToSpeech textToSpeech;
        string toSpeak;

        //information used to interact between the app and the API OpenBank
        public void Speak(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                toSpeak = text;
                if (textToSpeech == null)
                {
                    textToSpeech = new TextToSpeech(Forms.Context, this);
                }
                else
                {
                    var p = new Dictionary<string, string>();
                    textToSpeech.Speak(toSpeak, QueueMode.Flush, p);
                }
            }
        }

        //Starting the connection of the session between API OpenBank and this app
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                var p = new Dictionary<string, string>();
                textToSpeech.Speak(toSpeak, QueueMode.Flush, p);
            }
        }
    }
}