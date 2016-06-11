using App1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using App1.Cell;
using App1.Cell.ListViews;
using App1.REST;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace App1.Layout
{
    internal class AtmPage : ContentPage
    {
        private BranchListView atmlist;
        private Label resultsLabel;
        protected ListView ListView;
        public ListView _atmlistview;

        public AtmPage()
        {
            //to choose what bank they want
            Picker picker = new Picker()
            {
                Title = "Bank id choose wisely"
            };
            //picker.SelectedIndex += pickerSelected;

            resultsLabel = new Label
            {
                Text = "Result will appear here.",
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            //searchbar to search banks contacts that are available
            var searchBar = new SearchBar
            {
                Placeholder = "Enter short_name of bank",
                SearchCommand = new Command(() => { resultsLabel.Text = "Result over here: "; })
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
                    picker
                }
            };

            Task.WhenAll(Takingcareofbussiness());

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

        public void FilterLocations(string filter)
        {
            /*
            if (string.IsNullOrWhiteSpace(filter))
            {
                this.ItemsSource = locations;
            }
            else
            {
                this.ItemsSource = locations
                    .Where(x => x.Title.ToLower()
                   .Contains(filter.ToLower()));
            }

            this.EndRefresh();*/
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
                var uri = string.Format(Constants.BankUrl);

                //getting information from the online location of the bank list
                await rest.GetwithoutToken<Banklist>(uri).ContinueWith(t =>
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
                            ListView = new ListView
                            {
                                BackgroundColor = Color.Gray,
                                HasUnevenRows = true,
                                Margin = 10,
                                SeparatorColor = Color.Teal
                            };
                            ListView.ItemsSource = t.Result.banks;
                            ListView.ItemTemplate = new DataTemplate(typeof(Cells));
                        });
                    }
                });

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
                            _atmlistview = new ListView
                            {
                                BackgroundColor = Color.Gray,
                                HasUnevenRows = true,
                                Margin = 10,
                                SeparatorColor = Color.Teal
                            };
                            _atmlistview.ItemsSource = t.Result.atms;
                            _atmlistview.ItemTemplate = new DataTemplate(typeof(AtmListViews));
                        });
                    }
                });

                //indicates the activity indicator that all the information is loaded and ready
                IsBusy = false;
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