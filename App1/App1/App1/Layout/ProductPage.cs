using App1.Cell;
using App1.Models;
using App1.REST;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class ProductPage : ContentPage
    {
        private ListView _listView;

        public ProductPage()
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

            Task.WhenAll(Takingcareofbussiness());

            Title = "ProductsPage";
            Icon = new FileImageSource { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "Product page should contain products of the bank",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    },
                    indicator
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
                var uri = string.Format(Constants.BankUrl);

                //getting information from the online location
                await rest.GetwithoutToken<productlist>(uri).ContinueWith(t =>
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
                                HasUnevenRows = true,
                                Margin = 10,
                                SeparatorColor = Color.Teal
                            };
                            _listView.ItemsSource = t.Result.products;
                            _listView.ItemTemplate = new DataTemplate(typeof(productCells));
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