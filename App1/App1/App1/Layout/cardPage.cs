using App1.Cell;
using App1.Models;
using App1.REST;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    //the card page that show information of the users card information
    internal class cardPage : ContentPage
    {
        private ListView _listView;

        public cardPage()
        {
            ActivityIndicator indicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                IsRunning = true,
                IsVisible = true
            };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            indicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

           // Task.WhenAll(Takingcareofbussiness());

            Title = "CardsPage";
            Icon = new FileImageSource { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            Content = new StackLayout
            {
                BackgroundColor = Color.Teal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = {
                    new Label {
                        Text = "No information on this page",
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                }
            };
        }

        //used to take care of bussines to receive the card information of the user// OpenBank contains no known information on this
        private async Task Takingcareofbussiness()
        {
            //trying to get information online if some error occurs this is caught and taken care of, a message is displayed in this case
            try
            {
                //indicates the activity indicator to start
                IsBusy = true;

                var rest = new ManagerRESTService(new RESTService());
                Debug.WriteLine("Clicked transaction button");
                var uri = String.Format(Constants.CardsUrl, AccountsPage.Bankid);

                //getting information from the online location
                await rest.GetWithToken(uri).ContinueWith(t =>
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
                            _listView = new ListView
                            {
                                BackgroundColor = Color.Gray,
                                HasUnevenRows = true
                            };
                            //Must change this
                            _listView.ItemsSource = t.Result;
                            _listView.ItemTemplate = new DataTemplate(typeof(Cells));
                        });
                    }
                });
                //indicates the activity indicator that all the information is loaded and ready
                IsBusy = false;
                Content = new StackLayout
                {
                    BackgroundColor = Color.Teal,
                    Spacing = 10,
                    Children =
                    {
                        new Label {Text = "Card list go up and down", HorizontalTextAlignment = TextAlignment.Center},
                        _listView
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