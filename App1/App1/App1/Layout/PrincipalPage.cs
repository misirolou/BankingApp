using App1.REST;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using App1.Menu;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class PrincipalPage : ContentPage
    {
        private Label numberLabel, nameLabel, ibanLabel, amountLabel, bankLabel, currencyLabel, typeLabel, swiftLabel;
        private StackLayout ButtonLayout;
        private StackLayout AccountLayout;
        private StackLayout menuLayout;
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
                    new Label { Text = "your Account Information", HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold },
                    exitButton
                }
            };

            //Layout of the Home page(PrincipalPage.cs)
            Title = AccountsPage.Accountid;
            Icon = new FileImageSource() { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            //this is the type of layout the grids will be specified in
            ButtonLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Children = { transactionButton, cardsbutton }
            };

            //the content that will be used in the account
            Content = new StackLayout()
            {
                BackgroundColor = Color.Teal,
                Spacing = 10,
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
                var uri = string.Format(AccountsPage.Href);

                Debug.WriteLine("uri principalpage {0}", uri);

                if (string.IsNullOrWhiteSpace(uri))
                {
                    Debug.WriteLine("Response contained empty body...");
                }
                else
                {
                    //getting information from the online location about the users detailed account info
                    //in this case it can only get information from one selected account
                    await rest.GetWithToken(uri).ContinueWith(task =>
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
                            AccountInfo.AccountInfoDetailed Information = JsonConvert.DeserializeObject<AccountInfo.AccountInfoDetailed>(task.Result);

                            //because of the array list that the owner contains this was the only way to show its data could add the other objects if i wanted to
                            ListView _listView = new ListView
                            {
                                HasUnevenRows = true,
                                SeparatorColor = Color.Teal,
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                                SelectedItem = false
                            };
                            _listView.ItemsSource = Information.owners;
                            _listView.ItemTemplate = new DataTemplate((typeof(TextCell)));
                            _listView.ItemTemplate.SetBinding(TextCell.TextProperty, "display_name");

                            nameLabel = new Label()
                            {
                                HorizontalTextAlignment = TextAlignment.Center
                            };
                            nameLabel.SetBinding(TextCell.TextProperty, "owners.display_name");

                            //label for the number of this account used
                            numberLabel = new Label()
                            {
                                Text = "id: " + Information.id,
                                HorizontalTextAlignment = TextAlignment.Center
                            };
                            //this is your balances amount shown
                            amountLabel = new Label()
                            {
                                Text = "Balance amount: " + Information.balance.amount,
                                HorizontalTextAlignment = TextAlignment.Center,
                                BackgroundColor = Color.Black
                            };
                            //this contains the currency used
                            currencyLabel = new Label()
                            {
                                Text = "Currency: " + Information.balance.currency,
                                HorizontalTextAlignment = TextAlignment.Center,
                            };
                            //this is the specified bank id
                            bankLabel = new Label()
                            {
                                Text = "bank: " + Information.bank_id,
                                HorizontalTextAlignment = TextAlignment.Center,
                                BackgroundColor = Color.Black
                            };
                            //this is your iban may be empty in some cases
                            ibanLabel = new Label()
                            {
                                Text = "IBAN: " + Information.IBAN,
                                HorizontalTextAlignment = TextAlignment.Center
                            };
                            //this is your swift/bic numbers used, may be empty in some cases
                            swiftLabel = new Label()
                            {
                                Text = "swift/bic: " + Information.swift_bic,
                                HorizontalTextAlignment = TextAlignment.Center,
                                BackgroundColor = Color.Black
                            };
                            //this is the type of account that you have, in some cases may be empty
                            typeLabel = new Label()
                            {
                                Text = "type: " + Information.type,
                                HorizontalTextAlignment = TextAlignment.Center
                            };

                            //Layout of the accounts detailed information
                            AccountLayout = new StackLayout()
                            {
                                BackgroundColor = Color.Gray,
                                Margin = 10,
                                Children = { _listView,nameLabel, numberLabel, amountLabel, currencyLabel, bankLabel, ibanLabel, swiftLabel, typeLabel }
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
                        menuLayout,
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