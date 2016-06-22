using App1.Cell;
using App1.Models;
using App1.REST;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    //transaction page used to show all the transaction information
    internal class transactionPage : ContentPage
    {
        private ListView _listView;
        private StackLayout menuLayout;
        private Grid labelLayout;
        public static string counterid { get; private set; }
        public static string accountid { get; private set; }
        public static string transactionid { get; private set; }
        public static string valueamount { get; private set; }
        public static string description { get; private set; }
        public static string completed { get; private set; }
        public static string newbalanceamount { get; private set; }
        public static string newbalancecurrency { get; private set; }
        public static string accountbank { get; private set; }
        public static string counterbank { get; private set; }
        public static string typesome { get; private set; }

        //Wanted to add charts by following this link https://blog.xamarin.com/visualize-your-data-with-charts-graphs-and-xamarin-forms/
        //problem that it costs 995$ so i didnt think it was worth it at the moment
        //if you want you can make this  a tabbed page where this page shows the transaction information and on the other the graphical information of the users transaction
        public transactionPage()
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

            Button exitButton = new Button()
            {
                Image = (FileImageSource)Device.OnPlatform(
                    iOS: ImageSource.FromFile("Exit.png"),
                    Android: ImageSource.FromFile("Exit.png"),
                    WinPhone: ImageSource.FromFile("Exit.png")),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.Gray
            };
            exitButton.Clicked += async (sender, args) => await Navigation.PopToRootAsync();

            menuLayout = new StackLayout()
            {
                BackgroundColor = Color.Gray,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Margin = 10,
                Children =
                {
                    new Label
                    {
                        Text = "your transaction Information",
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontAttributes = FontAttributes.Bold
                    },
                    exitButton
                }
            };

            labelLayout = new Grid();
            labelLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            labelLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            labelLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            labelLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            labelLayout.Children.Add(new Label
            {
                Text = "DATE",
                HorizontalTextAlignment = TextAlignment.Start,
                FontAttributes = FontAttributes.Bold
            }, 0, 0);

            labelLayout.Children.Add(new Label
            {
                Text = "AMOUNT",
                HorizontalTextAlignment = TextAlignment.End,
                FontAttributes = FontAttributes.Bold
            }, 1, 0);
            labelLayout.Children.Add(new Label
            {
                Text = "BALANCE",
                HorizontalTextAlignment = TextAlignment.End,
                FontAttributes = FontAttributes.Bold
            }, 2, 0);

            Task.WhenAll(Takingcareofbussiness());

            Title = "TransactionsPage";
            Icon = new FileImageSource { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");

            Content = new StackLayout
            {
                BackgroundColor = Color.Teal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    new Label
                    {
                        Text = "Getting your transaction information",
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                }
            };
        }

        //Taking care of bussiness, verifyifing if the information received of the transaction history is correct
        private async Task Takingcareofbussiness()
        {
            //trying to get information online if some error occurs this is caught and taken care of, a message is displayed in this case
            try
            {
                //indicates the activity indicator to start
                IsBusy = true;

                var rest = new ManagerRESTService(new RESTService());
                Debug.WriteLine("Clicked transaction button");
                var uri = String.Format(Constants.MovementUrl, AccountsPage.Bankid, AccountsPage.Accountid);

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
                            var result = t.Result;

                            Transactions.TransactionList jsonObject =
                                JsonConvert.DeserializeObject<Transactions.TransactionList>(result);

                            _listView = new ListView
                            {
                                HasUnevenRows = true,
                                Margin = 10,
                                SeparatorColor = Color.Teal,
                                ItemsSource = jsonObject.transactions,
                                ItemTemplate = new DataTemplate(typeof(TransactionCell))
                            };
                            _listView.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as Transactions.Transaction);
                        });
                    }
                });
                //indicates the activity indicator that all the information is loaded and ready
                IsBusy = false;

                Button graphbutton = new Button()
                {
                    Text = "Graphs"
                };
                graphbutton.Clicked += async (sender, args) => await Navigation.PushAsync(new ChartsPage());
                Content = new StackLayout
                {
                    BackgroundColor = Color.Teal,
                    Spacing = 10,
                    Children =
                    {
                        menuLayout,
                        labelLayout,
                        _listView,
                        graphbutton
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

        private async void NavigateTo(Transactions.Transaction transaction)
        {
            if (transaction == null)
                return;

            transactionid = transaction.id;
            accountid = transaction.account.id;
            accountbank = transaction.account.bank.name;
            counterid = transaction.counterparty.holder.name;
            counterbank = transaction.counterparty.bank.name;
            typesome = transaction.details.type;
            description = transaction.details.description;
            completed = transaction.details.completed;
            newbalanceamount = transaction.details.new_balance.amount;
            newbalancecurrency = transaction.details.new_balance.currency;
            valueamount = transaction.details.value.amount;

            try
            {
                await Navigation.PushAsync(new TransactionDetailedPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error transactionpage: {0}.", err);
            }
        }
    }
}