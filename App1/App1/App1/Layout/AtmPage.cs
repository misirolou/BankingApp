using App1.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App1.Layout
{
    internal class AtmPage : ContentPage
    {
        private List<Banklist> banklist;
        private Label resultsLabel;
        private SearchBar searchBar;

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

            //to choose what bank they want
            Picker picker = new Picker()
            {
                Title = "Bank id choose wisely"
            };
            // picker.SelectedIndex += pickerSelected;

            resultsLabel = new Label
            {
                Text = "Result will appear here.",
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            //searchbar to search banks contacts that are available
            searchBar = new SearchBar
            {
                Placeholder = "Enter short_name of bank",
                SearchCommand = new Command(() => { resultsLabel.Text = "Result: " + searchBar.Text + " here you go"; })
            };

            //the map view of the area
            var location = new Atm();
            var map = new Map(MapSpan.FromCenterAndRadius(
                    new Position(location.location.latitude, location.location.longitude), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            //putting pins in certain locations
            var position = new Position(location.location.latitude, location.location.longitude); // Latitude, Longitude
            var pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = "custom pin",
                Address = "custom detail info"
            };
            map.Pins.Add(pin);

            var stacklayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children =
                {
                    picker, Back
                }
            };

            Title = "AtmPage";
            Icon = new FileImageSource { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            Content = new StackLayout
            {
                Children = {
                    stacklayout,
                    searchBar,
                    resultsLabel,
                    map
                }
            };
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}