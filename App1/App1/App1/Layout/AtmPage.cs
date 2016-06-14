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
    internal class AtmPage : ContentPage
    {
        private Label resultsLabel;
        protected ListView ListView;
        public ListView _atmlistview;
        protected Map Map;
        public static double Latitude { get; private set; }
        public static double Longitude { get; private set; }

        public AtmPage()
        {
            resultsLabel = new Label
            {
                Text = "MAP should appear here.",
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            ActivityIndicator indicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                IsRunning = true,
                IsVisible = true
            };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            indicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

            Task.WhenAll(Takingcareofbussiness());

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

        private async Task Takingcareofbussiness()
        {
            //unit testing soe varibles may add this to a unique unit test in the later stages
            //  var json = "{'banks':[{'id':'rbs','short_name':'The Royal Bank of Scotland','full_name':'The Royal Bank of Scotland','logo':'http://www.red-bank-shoreditch.com/logo.gif','website':'http://www.red-bank-shoreditch.com'},{'id':'test-bank','short_name':'TB','full_name':'Test Bank','logo':null,'website':null},{'id':'testowy_bank_id','short_name':'TB','full_name':'Testowy bank',    'logo':null,'website':null},{'id':'nordea','short_name':'Nordea','full_name':'Nordea Bank AB','logo':'http://logonoid.com/images/nordea-logo.jpg','website':'http://www.nordea.com/'},{'id':'nordeaab','short_name':'Nordea','full_name':'Nordea Bank AB','logo':'http://logonoid.com/images/nordea-logo.jpg','website':'http://www.nordea.com/'},{'id':'hsbc-test','short_name':'HSBC Test','full_name':'Hongkong and Shanghai Bank','logo':null,'website':null},{'id':'erste-test','short_name':'Erste Bank Test','full_name':'Erste Bank Test','logo':null,'website':null},{'id':'deutche-test','short_name':'Deutche Bank Test','full_name':'Deutche Bank Test','logo':null,'website':null},{'id':'obp-bankx-m','short_name':'Bank X','full_name':'The Bank of X','logo':'https://static.openbankproject.com/images/bankx/bankx_logo.png','website':'https://www.example.com'}]}";
            //  var jsonconverting = JsonConvert.DeserializeObject<Banklist>(json);

            //trying to get information online if some error occurs this is caught and taken care of, a message is displayed in this case
            try
            {
                //indicates the activity indicator to start
                IsBusy = true;

                var rest = new ManagerRESTService(new RESTService());
                var uri = string.Format(Constants.ATMsUrl, AccountsPage.Bankid);

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
                            List<Atm> data = t.Result.atms;

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