using System;
using System.Diagnostics;
using System.Threading.Tasks;
using App1.Cell;
using App1.Models;
using App1.REST;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class transactionPage : ContentPage
    {
        private ListView _listView;

        public transactionPage()
        {
            Title = "TransactionsPage";
            Icon = new FileImageSource { File = "robot.png" };
            Content = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "TransactionsPage should have most of the users transactions",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }

        private async Task Takingcareofbussiness()
        {
            //trying to get information online if some error occurs this is caught and taken care of, a message is displayed in this case
            try
            {
                //indicates the activity indicator to start
                IsBusy = true;

                var rest = new ManagerRESTService(new RESTService());
                var Accounts = new Accounts.Account();
                Debug.WriteLine("Clicked transaction button");
                var uri = String.Format(Constants.MovementUrl, Accounts.bank_id, Accounts.id);

                //getting information from the online location
                await rest.GetWithToken<Transactions>(uri).ContinueWith(t =>
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
                            _listView.ItemsSource = t.Result.ToString();
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
                        new Label {Text = "Contact list go up and down", HorizontalTextAlignment = TextAlignment.Center},
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