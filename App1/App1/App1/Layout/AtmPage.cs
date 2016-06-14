using App1.Models;
using App1.REST;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App1.Layout
{
    //Atm Page used to show the location of all the atms using a map provided by google
    internal class AtmPage : ContentPage
    {
        private Label resultsLabel;
        protected ListView ListView;
        public ListView _atmlistview;
        protected Map Map;

        //layout of the atmpage and its functionalities
        public AtmPage()
        {
            resultsLabel = new Label
            {
                Text = "MAP should appear here.",
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            //indicator used to show that something is working in the background
            ActivityIndicator indicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                IsRunning = true,
                IsVisible = true
            };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            indicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

            //Task used to make the request needed to show the information on the map
            Task.WhenAll(Takingcareofbussiness());

            //Initial layout of the page
            Title = "ATMPage";
            Icon = new FileImageSource { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            Content = new StackLayout
            {
                Children = {
                        resultsLabel
                    }
            };
        }

        //used to take care of bussiness so it can receive the coordinate needed to show on the map
        private async Task Takingcareofbussiness()
        {
            //trying to get information online if some error occurs this is caught and taken care of, a message is displayed in this case
            try
            {
                //indicates the activity indicator to start
                IsBusy = true;

                var rest = new ManagerRESTService(new RESTService());
                var uri = string.Format(Constants.ATMsUrl, AccountsPage.Bankid);

                //this is the request made to the REST service to receive information
                await rest.GetwithoutToken<atmlist>(uri).ContinueWith(t =>
                {
                    //Problem occured a message is displayed to the user
                    if (t.IsFaulted)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Alert", "Something went wrong sorry :(", "OK");
                        });
                    }
                    //everything went fine, information should be displayed
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            //Used to show the data received from the Result achieved
                            List<Atm> data = t.Result.atms;

                            //If there is no data the map will show the location of a company that i am a trainee at
                            if (data.Count == 0)
                            {
                                Map = new Map(MapSpan.FromCenterAndRadius(
                                    new Position(0, 0), Distance.FromMiles(0.5)))
                                {
                                    IsShowingUser = true,
                                    HeightRequest = 100,
                                    WidthRequest = 960,
                                    VerticalOptions = LayoutOptions.FillAndExpand
                                };
                                Map.Pins.Add(new Pin
                                {
                                    Position = new Position(32.6672502, -16.9168688),
                                    Label = "Nothing over here ",
                                    Address = "Top Secret Company "
                                });
                                Map.MoveToRegion(MapSpan.FromCenterAndRadius(
                                    new Position(32.6672502, -16.9168688), Distance.FromMiles(2.0)));
                                Map.Margin = 5;
                            }
                            //it will receive all the information and add the pins necessary to the map using the coordinates received
                            else
                            {
                                Debug.WriteLine("Latitude {0}--- Longitude{1} ---- name {2}", data[0].location.latitude, data[0].location.longitude, data[0].name);

                                Map = new Map(MapSpan.FromCenterAndRadius(
                                    new Position(data[0].location.latitude, data[0].location.longitude), Distance.FromMiles(0.5)))
                                {
                                    IsShowingUser = true,
                                    HeightRequest = 100,
                                    WidthRequest = 960,
                                    VerticalOptions = LayoutOptions.FillAndExpand
                                };
                                Map.MoveToRegion(MapSpan.FromCenterAndRadius(
                                    new Position(data[0].location.latitude, data[0].location.longitude), Distance.FromMiles(1.5)));
                                Map.Margin = 5;
                                for (int i = 0; i < data.Count; i++)
                                {
                                    Map.Pins.Add(new Pin
                                    {
                                        Position = new Position(data[i].location.latitude, data[i].location.longitude),
                                        Label = "Name: " + data[i].name,
                                        Address = "Address: " + data[i].address.line_1 + data[i].address.line_2 + data[i].address.line_3 + ";City: " + data[i].address.city + ";State: " + data[i].address.state
                                    });
                                }
                            }
                        });
                    }
                });

                //indicates the activity indicator that all the information is loaded and ready
                IsBusy = false;

                //Final Layout of this page
                Content = new StackLayout
                {
                    Children = {
                        new Label() {Text = "ATMs Locations of Bank: " + AccountsPage.Bankid},
                        Map
                    }
                };
            }
            catch (Exception err)
            {
                IsBusy = false;
                await DisplayAlert("Alert", "Internet problems cant receive information", "OK");
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }
    }
}