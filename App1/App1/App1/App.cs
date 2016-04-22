using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace App1
{
    public class App : Application
    {
        public App()
        {
            var button1 = new Button();
            // The root page of your application
            MainPage = new ContentPage
            {
               Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        button1,
                        new Label {
                            HorizontalOptions = LayoutOptions.Center,
                            Text = "Welcome to Xamarin Forms!" + "Daniel Faria"
                        }
                    }
                }
            };
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
