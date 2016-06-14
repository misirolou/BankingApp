using App1.REST;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    //This page is the mainpage used to display the account detailed information that was selected 
    internal class PrincipalPage : ContentPage
    {
        private Label numberLabel, nameLabel, ibanLabel, amountLabel, bankLabel, currencyLabel, typeLabel, swiftLabel;
        private Grid ButtonLayout;
        private StackLayout AccountLayout;
        private StackLayout menuLayout;
        private static string href;

        //layout and functionalities
        public PrincipalPage()
        {
            //Transaction button used to change to the transaction page
            var transactionButton = new Button
            {
                Text = "Transaction",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            transactionButton.Clicked += OntransactionButtonClicked;

            //Balcao button should take you to the banks location page
            var BalcaoButton = new Button()
            {
                Text = "Bank Map",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            BalcaoButton.Clicked += OnBalcaoButtonClicked;

            //Atm button should take you to the ATM location page
            var AtmButton = new Button()
            {
                Text = "ATM Map",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            AtmButton.Clicked += OnAtmButtonClicked;

            //bank cards button used to change to the cards page
            var cardsbutton = new Button()
            {
                Text = "Cards",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            cardsbutton.Clicked += OncardsButtonClicked;

            //bank cards button used to change to the cards page
            var ProductButton = new Button()
            {
                Text = "Cards",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            ProductButton.Clicked += OnProductButtonClicked;

            //payment button used
            var Paymentsbutton = new Button()
            {
                Text = "Payment",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            Paymentsbutton.Clicked += OnpaymentsButtonClicked;

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
                Children = {
                    new Label { Text = "your Account Information", HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold },
                    exitButton
                }
            };

            //this is the type of layout the grids will be specified in
            ButtonLayout = new Grid()
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                RowSpacing = 5,
                ColumnSpacing = 5,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                }
            };
            ButtonLayout.Children.Add(transactionButton, 0, 0);
            ButtonLayout.Children.Add(Paymentsbutton, 1, 0);
            ButtonLayout.Children.Add(cardsbutton, 2, 0);
            ButtonLayout.Children.Add(BalcaoButton, 0, 1);
            ButtonLayout.Children.Add(ProductButton, 1,1);
            ButtonLayout.Children.Add(AtmButton, 2, 1);

            //Layout of the Home page(PrincipalPage.cs)
            Title = AccountsPage.Accountid;
            Icon = new FileImageSource() { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
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

        //Used to take care of bussiness, to show the accounts detailed information 
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

                            //label for the number of this account used
                            numberLabel = new Label()
                            {
                                Text = "id: " + Information.id,
                                Margin = 2,
                                HorizontalTextAlignment = TextAlignment.Center
                            };
                            //this is your balances amount shown
                            amountLabel = new Label()
                            {
                                Text = "Balance amount: " + Information.balance.amount,
                                HorizontalTextAlignment = TextAlignment.Center,
                                BackgroundColor = Color.Black,
                                Margin = 2,
                            };
                            //this contains the currency used
                            currencyLabel = new Label()
                            {
                                Text = "Currency: " + Information.balance.currency,
                                HorizontalTextAlignment = TextAlignment.Center,
                                Margin = 2,
                            };
                            //this is the specified bank id
                            bankLabel = new Label()
                            {
                                Text = "bank: " + Information.bank_id,
                                HorizontalTextAlignment = TextAlignment.Center,
                                Margin = 2,
                                BackgroundColor = Color.Black
                            };
                            //this is your iban may be empty in some cases
                            ibanLabel = new Label()
                            {
                                Text = "IBAN: " + Information.IBAN,
                                Margin = 2,
                                HorizontalTextAlignment = TextAlignment.Center
                            };
                            //this is your swift/bic numbers used, may be empty in some cases
                            swiftLabel = new Label()
                            {
                                Text = "swift/bic: " + Information.swift_bic,
                                HorizontalTextAlignment = TextAlignment.Center,
                                Margin = 2,
                                BackgroundColor = Color.Black
                            };
                            //this is the type of account that you have, in some cases may be empty
                            typeLabel = new Label()
                            {
                                Text = "type: " + Information.type,
                                Margin = 2,
                                HorizontalTextAlignment = TextAlignment.Center
                            };

                            //Layout of the accounts detailed information
                            AccountLayout = new StackLayout()
                            {
                                BackgroundColor = Color.Gray,
                                Margin = 10,
                                Children = { numberLabel, amountLabel, currencyLabel, bankLabel, ibanLabel, swiftLabel, typeLabel }
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

        //what happens when we click the Balcao button
        private async void OnBalcaoButtonClicked(object sender, EventArgs e)
        {
            //get information connected to the banks branch information localized on OpenBanks sandobox this needs a bankid
            try
            {
                await Navigation.PushAsync(new BalcaoPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }

        //what happens when we click the Atm button
        private async void OnAtmButtonClicked(object sender, EventArgs e)
        {
            //get information connected to the banks ATM information localized on OpenBanks sandbox
            try
            {
                await Navigation.PushAsync(new AtmPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
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

        //What will happen if the user clicks the payment button, should just change pages
        private async void OnpaymentsButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Clicked payment button");
            try
            {
                await Navigation.PushAsync(new PaymentPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error cardspage: {0}.", err);
            }
        }


        private async void OnProductButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Clicked payment button");
            try
            {
                await Navigation.PushAsync(new ProductPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error cardspage: {0}.", err);
            }
        }
    }
}