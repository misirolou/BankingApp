using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App1.Layout
{
    internal class AtmPage : ContentPage
    {
        public AtmPage()
        {
            Button Back = new Button()
            {
                Image = (FileImageSource)Device.OnPlatform(
                    iOS: ImageSource.FromFile("Back.png"),
                    Android: ImageSource.FromFile("Back.png"),
                    WinPhone: ImageSource.FromFile("Back.png")),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.Gray
            };
            Back.Clicked += BackButtonClicked;

            //the map view of the area
            var map = new Map(MapSpan.FromCenterAndRadius(
                    new Position(37, -122), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            //putting pins in certain locations
            var position = new Position(37, -122); // Latitude, Longitude
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = "custom pin",
                Address = "custom detail info"
            };
            map.Pins.Add(pin);

            Title = "AtmPage";
            Icon = new FileImageSource { File = "robot.png" };
            Content = new StackLayout
            {
                Children = {
                    Back,
                    map
                   /* new Label {
                        Text = "AtmPage should have a map of ATMs",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }*/
                }
            };
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}