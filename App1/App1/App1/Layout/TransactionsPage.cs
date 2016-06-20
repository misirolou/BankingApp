using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1.Cell;
using App1.Models;
using App1.REST;
using Newtonsoft.Json;
using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;

namespace App1.Layout
{
    class TransactionsPage : ContentPage
    {
        private ListView _listView;
        private StackLayout menuLayout;
        SfDataGrid dataGrid;
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

        public TransactionsPage()
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

                            List<double> valueList = jsonObject.transactions.Select(x => double.Parse(x.details.value.amount)).ToList();
                            List<double> newBalanceList = jsonObject.transactions.Select(x => double.Parse(x.details.new_balance.amount)).ToList();
                            List<String> counterpartyname = jsonObject.transactions.Select(x => x.counterparty.holder.name).ToList();

                            dataGrid = new SfDataGrid();
                           
                            //Collection of the value amount and the dates of the transactions made
                            ObservableCollection<OrderTransaction> griddata = new ObservableCollection<OrderTransaction>();

                            //adding the information to each of the charts collections
                            for (var i = 0; i < jsonObject.transactions.Count; i++)
                            {
                                griddata.Add(new OrderTransaction(jsonObject.transactions[i].details.completed, counterpartyname[i], valueList[i], newBalanceList[i]));
                                Debug.WriteLine("Date :: {0} amount ::{1}", counterpartyname[i], valueList[i]);
                            }

                            dataGrid.ItemsSource = griddata;
                            dataGrid.AutoGenerateColumns = false;

                            GridTextColumn dateColumn = new GridTextColumn();
                            dateColumn.MappingName = "Date";
                            dateColumn.HeaderText = "Date";

                            GridTextColumn counterpartyColumn = new GridTextColumn();
                            counterpartyColumn.MappingName = "Counterparty";
                            counterpartyColumn.HeaderText = "Counterparty";

                            GridTextColumn valueColumn = new GridTextColumn();
                            valueColumn.MappingName = "Value";
                            valueColumn.HeaderText = "Value";

                            GridTextColumn newBalanceColumn = new GridTextColumn();
                            newBalanceColumn.MappingName = "newbalance";
                            newBalanceColumn.HeaderText = "newbalance";

                            dataGrid.Columns.Add(dateColumn);
                            dataGrid.Columns.Add(counterpartyColumn);
                            dataGrid.Columns.Add(valueColumn);
                            dataGrid.Columns.Add(newBalanceColumn);

                            dataGrid.AllowSorting = true;

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

                Button Graph = new Button()
                {
                    Text = "Graphs"
                };
                Graph.Clicked += async (sender, args) => await Navigation.PushAsync(new ChartsPage());
                Content = new StackLayout
                {
                    BackgroundColor = Color.Teal,
                    Spacing = 10,
                    Children =
                    {
                        menuLayout,
                        dataGrid,
                        Graph
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
