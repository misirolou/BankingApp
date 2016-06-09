using App1.Cell;
using App1.Models;
using App1.REST;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using App1.Menu;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class transactionPage : ContentPage
    {
        private ListView _listView;
        private DataTemplate validDataTemplate;
        private StackLayout menuLayout;
        //private DataTemplate invalidDataTemplate;

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

            Button menuButton = new Button()
            {
                Image = (FileImageSource)Device.OnPlatform(
                   iOS: ImageSource.FromFile("menu.png"),
                   Android: ImageSource.FromFile("menu.png"),
                   WinPhone: ImageSource.FromFile("menu.png")),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Gray
            };
            menuButton.Clicked += async (sender, args) => await Navigation.PushAsync(new MenuPage());

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
                Children = { menuButton,
                    new Label { Text = "your detailed account Information", HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold },
                    exitButton
                }
            };

            Task.WhenAll(Takingcareofbussiness());

            Title = "TransactionsPage";
            Icon = new FileImageSource { File = "robot.png" };
            Content = new StackLayout
            {
                BackgroundColor = Color.Teal,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    new Label
                    {
                        Text = "Getting your transaction information",HorizontalTextAlignment = TextAlignment.Center
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

                             Transactions.TransactionList jsonObject = JsonConvert.DeserializeObject<Transactions.TransactionList>(result);

                            _listView = new ListView
                            {
                                HasUnevenRows = true,
                                Margin = 10,
                                SeparatorColor = Color.Teal
                            };
                            _listView.ItemsSource = jsonObject.transactions;
                            _listView.ItemTemplate = validDataTemplate;
                            
                            validDataTemplate = new DataTemplate(() => {
                                 var grid = new Grid();
                                 grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });
                                 grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });
                                 grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });
                                 grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });

                                var idLabelholder = new Label { FontAttributes = FontAttributes.Bold };
                                var dateofCompletionLabel = new Label();
                                var amountLabel = new Label { HorizontalTextAlignment = TextAlignment.End };
                                var balanceLabel = new Label();

                                idLabelholder.SetBinding(Label.TextProperty, "account.id"); //account id of entity that did the transaction counterparty
                                dateofCompletionLabel.SetBinding(Label.TextProperty, "details.completed"); //date that the transaction was verified and completed
                                amountLabel.SetBinding(Label.TextProperty, "details.value.amount"); //amount that was used in the transaction
                                balanceLabel.SetBinding(Label.TextProperty, "details.new_balance.amount"); //balance of the users account

                                grid.Children.Add(idLabelholder, 0, 0);
                                 grid.Children.Add(dateofCompletionLabel, 1, 0);
                                 grid.Children.Add(amountLabel, 2, 0);
                                 grid.Children.Add(balanceLabel, 3, 0);

                                StackLayout stack = new StackLayout()
                                {
                                    HorizontalOptions = LayoutOptions.StartAndExpand,
                                    Orientation = StackOrientation.Horizontal,
                                    Children = { idLabelholder, dateofCompletionLabel, amountLabel, balanceLabel }
                                };

                                return new ViewCell
                                {
                                    View = stack
                                };
                            });
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
                        menuLayout,
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