using App1.Cell;
using App1.Models;
using App1.REST;
using Newtonsoft.Json;
using Syncfusion.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using static App1.Models.Transactions;

namespace App1.Layout
{
    //transaction page used to show all the transaction information
    internal class transactionPage : ContentPage
    {
        private ListView _listView;
        private StackLayout menuLayout;
        private Grid labelLayout;
        private SearchBar searchBar;
        protected TransactionList transactionList;

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

        private bool dateAscending;
        private bool amountAscending;
        private bool valueAscending;

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

       /*     Button exitButton = new Button()
            {
                Image = (FileImageSource)Device.OnPlatform(
                    iOS: ImageSource.FromFile("Exit.png"),
                    Android: ImageSource.FromFile("Exit.png"),
                    WinPhone: ImageSource.FromFile("Exit.png")),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.Gray
            };
            exitButton.Clicked += async (sender, args) => await Navigation.PopToRootAsync();*/

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
                    }
                }
            };

            Button dateButton = new Button
            {
                Text = "DATE",
                FontAttributes = FontAttributes.Bold
            };
            dateButton.Clicked += OnDateButton_Clicked;

            Button amountButton = new Button
            {
                Text = "AMOUNT",
                FontAttributes = FontAttributes.Bold
            };
            //amountButton.Clicked += OnAmountButton_Clicked;

            Button balanceButton = new Button
            {
                Text = "BALANCE",
                FontAttributes = FontAttributes.Bold
            };
            //balanceButton.Clicked += OnBalanceButton_Clicked;

            labelLayout = new Grid();
            labelLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            labelLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            labelLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            labelLayout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            labelLayout.Children.Add(dateButton, 0, 0);
            labelLayout.Children.Add(amountButton, 1, 0);
            labelLayout.Children.Add(balanceButton, 2, 0);

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

                            transactionList =
                                JsonConvert.DeserializeObject<TransactionList>(result);

                            dateAscending = true;
                            amountAscending = true;
                            valueAscending = true;

                            //used to take out the t and z out of the date information received from OpenBank
                            for (int i = 0; i < transactionList.transactions.Count; i++)
                            {
                                char[] delimiters = new char[] { 'T', 'Z' };

                                var date = transactionList.transactions[i].details.completed.Split(delimiters);
                                var test = string.Join(" ", date);
                                transactionList.transactions[i].details.completed = test;
                            }

                            _listView = new ListView
                            {
                                HasUnevenRows = true,
                                Margin = 10,
                                SeparatorColor = Color.Teal,
                                ItemsSource = transactionList.transactions,
                                ItemTemplate = new DataTemplate(typeof(TransactionCell))
                            };
                            _listView.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as Transaction);
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

        private void FilterDate(string filter)
        {
            //preparing for the list
            _listView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(filter))
            {
                //If nothing changed stays the same
                _listView.ItemsSource = transactionList.transactions;
            }
            else
            {
                //Changes the asked for date according to what was searched
                _listView.ItemsSource = transactionList.transactions
                    .Where(x => x.details.completed.ToLower()
                   .Contains(filter.ToLower()));
            }

            _listView.EndRefresh();
        }

        //Navigate to the specified transaction verifying detailed information about it
        private async void NavigateTo(Transaction transaction)
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

        //used to order according to Dates received
        private void OnDateButton_Clicked(object sender, EventArgs e)
        {
            int i = 0;
            try
            {
                if (dateAscending)
                {
                    _listView.BeginRefresh();

                    Debug.WriteLine("date test {0}", transactionList.transactions[i].details.completed);

                   var transaction = (from newdate in transactionList.transactions
                        orderby newdate.details.completed
                        select newdate).Cast<TransactionList>().ToList();

                    List<TransactionList> newList = transaction.ToList();

                    Debug.WriteLine("newlist {0} _:", newList.Count);

                    _listView = new ListView
                    {
                        HasUnevenRows = true,
                        Margin = 10,
                        SeparatorColor = Color.Teal,
                        ItemsSource = newList[i].transactions,
                        ItemTemplate = new DataTemplate(typeof(TransactionCell))
                    };
                    _listView.EndRefresh();
                    dateAscending = false;
                }
                else
                {
                    _listView.BeginRefresh();
                   var transaction = (from newdate in transactionList.transactions
                                                          orderby newdate.details.completed descending
                                                          select newdate).ToList<TransactionList>();

                    List<TransactionList> newList = transaction.ToList();

                    _listView = new ListView
                    {
                        HasUnevenRows = true,
                        Margin = 10,
                        SeparatorColor = Color.Teal,
                        ItemsSource = newList[i].transactions,
                        ItemTemplate = new DataTemplate(typeof(TransactionCell))
                    };
                    _listView.EndRefresh();
                    dateAscending = true;
                }
            }
            catch (Exception err)
            {
                _listView.EndRefresh();
                Debug.WriteLine("errmessage : {0}", err);
            }
        }

        private void OnAmountButton_Clicked(object sender, EventArgs e)
        {
            int i = 0;
            try
            {
                List<TransactionList> transaction;

                if (!amountAscending)
                {
                    _listView.BeginRefresh();
                    transaction = (List<TransactionList>)(from newvalue in transactionList.transactions
                                                          orderby newvalue.details.new_balance.amount
                                                          select newvalue).ToList<TransactionList>();

                    _listView = new ListView
                    {
                        HasUnevenRows = true,
                        Margin = 10,
                        SeparatorColor = Color.Teal,
                        ItemsSource = transaction[i].transactions,
                        ItemTemplate = new DataTemplate(typeof(TransactionCell))
                    };
                    _listView.EndRefresh();
                    amountAscending = false;
                }
                else
                {
                    _listView.BeginRefresh();
                    transaction = (List<TransactionList>)(from newvalue in transactionList.transactions
                                                          orderby newvalue.details.new_balance.amount descending
                                                          select newvalue).ToList<TransactionList>();

                    _listView = new ListView
                    {
                        HasUnevenRows = true,
                        Margin = 10,
                        SeparatorColor = Color.Teal,
                        ItemsSource = transaction[i].transactions,
                        ItemTemplate = new DataTemplate(typeof(TransactionCell))
                    };
                    _listView.EndRefresh();
                    amountAscending = true;
                }
            }
            catch (Exception err)
            {
                _listView.EndRefresh();
                Debug.WriteLine("errmessage : {0}", err);
            }
        }

        private void OnBalanceButton_Clicked(object sender, EventArgs e)
        {
            int i = 0;
            try
            {
                List<TransactionList> transaction;
                if (!valueAscending)
                {
                    _listView.BeginRefresh();
                    transaction = (List<TransactionList>)(from newBalance in transactionList.transactions
                                                          orderby newBalance.details.new_balance.amount
                                                          select newBalance).ToList<TransactionList>();

                    _listView = new ListView
                    {
                        HasUnevenRows = true,
                        Margin = 10,
                        SeparatorColor = Color.Teal,
                        ItemsSource = transaction[i].transactions,
                        ItemTemplate = new DataTemplate(typeof(TransactionCell))
                    };
                    _listView.EndRefresh();
                    valueAscending = false;
                }
                else
                {
                    _listView.BeginRefresh();
                    transaction = (List<TransactionList>)(from newBalance in transactionList.transactions
                                                          orderby newBalance.details.new_balance.amount descending
                                                          select newBalance).ToList<TransactionList>();

                    _listView = new ListView
                    {
                        HasUnevenRows = true,
                        Margin = 10,
                        SeparatorColor = Color.Teal,
                        ItemsSource = transaction[i].transactions,
                        ItemTemplate = new DataTemplate(typeof(TransactionCell))
                    };
                    _listView.EndRefresh();
                    valueAscending = true;
                }
            }
            catch (Exception err)
            {
                _listView.EndRefresh();
                Debug.WriteLine("errmessage : {0}", err);
            }
        }
    }
}