using App1.REST;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class PrincipalPage : ContentPage
    {
        private Label numberLabel, nameLabel, ibanLabel, amountLabel, bankLabel, currencyLabel, typeLabel, swiftLabel;
        private StackLayout ButtonLayout;
        private StackLayout AccountLayout;
        private static string href;

        public PrincipalPage()
        {
            //Transaction button used to change to the transaction page
            var transactionButton = new Button
            {
                Text = "Transactions",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            transactionButton.Clicked += OntransactionButtonClicked;

            //bank cards button used to change to the cards page
            var cardsbutton = new Button()
            {
                Text = "Cards",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            cardsbutton.Clicked += OncardsButtonClicked;

            //button that when tapped will change to the menu page
            Button menuButton = new Button()
            {
                Image = new FileImageSource() { File = "menu.png" }
            };
            menuButton.Clicked += async (sender, args) => await Navigation.PushAsync(new MenuPage());

            //button that when tapped will change to the menu page may need to do some things beforehand
            Button exitButton = new Button()
            {
                Image = new FileImageSource() { File = "Exit.png" }
            };
            exitButton.Clicked += async (sender, args) => await Navigation.PopToRootAsync();

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

            //Layout of the Home page(PrincipalPage.cs)
            Title = AccountsPage.accountid;
            Icon = new FileImageSource() { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            //this is the type of layout the grids will be specified in
            ButtonLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Padding = 1,
                Children = { transactionButton, cardsbutton }
            };

            //the content that will be used in the account
            Content = new StackLayout()
            {
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Padding = 1,
                Children =
                {
                     new Label {Text = "Getting your account information", HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.Center},
                     ButtonLayout,
                    indicator
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
                var uri = string.Format(AccountsPage.href);

                Debug.WriteLine("uri principalpage {0}", uri);

                if (string.IsNullOrWhiteSpace(uri))
                {
                    Debug.WriteLine("Response contained empty body...");
                }
                else
                {
                    //getting information from the online location about the users detailed account info
                    //in this case it can only get information from one selected account
                    await rest.GetWithToken<AccountInfo.AccountInfoDetailed>(uri).ContinueWith(task =>
                {
                    //Problem occured a message is displayed to the user
                    if (task.IsFaulted)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Alert", "Something went wrong sorry with your detailed information :(", "OK");
                        });
                    }
                    //everything went fine, information should be displayed
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            numberLabel = new Label()
                            {
                                Text = "id" + task.Result.number
                            };
                            nameLabel = new Label()
                            {
                                Text = "name: "
                            };
                            nameLabel.SetBinding(Label.TextProperty, "display_name");

                            ibanLabel = new Label()
                            {
                                Text = "IBAN: " + task.Result.IBAN
                            };
                            amountLabel = new Label()
                            {
                                Text = "Balance amount: " + task.Result.balance.amount
                            };
                            bankLabel = new Label()
                            {
                                Text = "bank: " + task.Result.bank_id
                            };
                            currencyLabel = new Label()
                            {
                                Text = "Currency: " + task.Result.balance.currency
                            };
                            typeLabel = new Label()
                            {
                                Text = "type: " + task.Result.type
                            };
                            swiftLabel = new Label()
                            {
                                Text = "swift/bic: " + task.Result.swift_bic
                            };
                            AccountLayout = new StackLayout()
                            {
                                BackgroundColor = Color.Gray,
                                Children = { nameLabel, numberLabel, amountLabel, currencyLabel, bankLabel, ibanLabel, swiftLabel, typeLabel }
                            };
                        });
                    }
                });
                }
                //indicates the activity indicator that all the information is loaded and ready
                IsBusy = false;
                Content = new StackLayout
                {
                    BackgroundColor = Color.Teal,
                    Spacing = 10,
                    Children =
                    {
                        new Label {Text = "Account list go up and down", HorizontalTextAlignment = TextAlignment.Center},
                        AccountLayout,
                        ButtonLayout
                    }
                };
            }
            catch (Exception err)
            {
                IsBusy = false;
                await DisplayAlert("Alert", "There was a problem sorry :(", "OK");
                Debug.WriteLine("Caught error principalpage: {0}.", err);
            }
        }

        //should take te user to the transaction page
        private async void OntransactionButtonClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new transactionPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error transactionpage: {0}.", err);
            }
        }

        //should take the user to the cards page
        private async void OncardsButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Clicked cards button");
            try
            {
                await Navigation.PushAsync(new cardPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error cardspage: {0}.", err);
            }
        }
    }
}