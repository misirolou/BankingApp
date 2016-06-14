using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class TransactionDetailedPage : ContentPage
    {
        private Label valueamount,
            newbalancecurrency,
            newbalanceamount,
            completed,
            description,
            posted,
            typeLabel,
            counterpartyidbank,
            counterpartyid,
            myAccountBank,
            myAccountid,
            idTransLabel;

        private Grid ButtonLayout;
        private StackLayout AccountLayout;
        private StackLayout menuLayout;

        public TransactionDetailedPage()
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
                        Text = "your Account Information",
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontAttributes = FontAttributes.Bold
                    },
                    exitButton
                }
            };

            //label for the number of this account used
            idTransLabel = new Label()
            {
                Text = "Transaction id: " + transactionPage.transactionid,
                Margin = 2,
                HorizontalTextAlignment = TextAlignment.Center
            };
            //this is your balances amount shown
            myAccountid = new Label()
            {
                Text = "Your Account ID: " + transactionPage.accountid,
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.Black,
                Margin = 2,
            };
            //this contains the currency used
            myAccountBank = new Label()
            {
                Text = "Your Bank: " + transactionPage.accountbank,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = 2,
            };

            //this is the specified bank id
            counterpartyid = new Label()
            {
                Text = "Counterparty name: " + transactionPage.counterid,
                HorizontalTextAlignment = TextAlignment.Center,
                Margin = 2,
                BackgroundColor = Color.Black
            };
            //this is your iban may be empty in some cases
            counterpartyidbank = new Label()
            {
                Text = "Counterparty bank: " + transactionPage.counterbank,
                Margin = 2,
                HorizontalTextAlignment = TextAlignment.Center
            };

            description = new Label()
            {
                Text = "description: " + transactionPage.description,
                Margin = 2,
                BackgroundColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center
            };

            completed = new Label()
            {
                Text = "Date of Completion: " + transactionPage.completed,
                Margin = 2,
                HorizontalTextAlignment = TextAlignment.Center
            };

            newbalanceamount = new Label()
            {
                Text = "NewBalance " + transactionPage.newbalanceamount,
                Margin = 2,
                HorizontalTextAlignment = TextAlignment.Center
            };

            newbalancecurrency = new Label()
            {
                Text = "Currency " + transactionPage.newbalancecurrency,
                Margin = 2,
                BackgroundColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center
            };

            valueamount = new Label()
            {
                Text = "Amount: " + transactionPage.valueamount,
                Margin = 2,
                BackgroundColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center
            };
            IsBusy = false;
            //may need to change order
            //Layout of the accounts detailed information
            AccountLayout = new StackLayout()
            {
                BackgroundColor = Color.Gray,
                Margin = 10,
                Children =
                    {
                        idTransLabel,
                        myAccountid,
                        myAccountBank,
                        counterpartyid,
                        counterpartyidbank,
                        valueamount,
                        newbalanceamount,
                        newbalancecurrency,
                        completed,
                        description
                    }
            };

            //Layout of the Home page(PrincipalPage.cs)
            Title = "TransactionPage Detailed";
            Icon = new FileImageSource() { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            //the content that will be used in the account
            Content = new StackLayout()
            {
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Children =
                {
                    menuLayout,
                    AccountLayout,
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

                //label for the number of this account used
                idTransLabel = new Label()
                {
                    Text = "Transaction id: " + transactionPage.transactionid,
                    Margin = 2,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                //this is your balances amount shown
                myAccountid = new Label()
                {
                    Text = "Your Account ID: " + transactionPage.accountid,
                    HorizontalTextAlignment = TextAlignment.Center,
                    BackgroundColor = Color.Black,
                    Margin = 2,
                };
                //this contains the currency used
                myAccountBank = new Label()
                {
                    Text = "Your Bank: " + transactionPage.accountbank,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = 2,
                };

                //this is the specified bank id
                counterpartyid = new Label()
                {
                    Text = "Counterparty name: " + transactionPage.counterid,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = 2,
                    BackgroundColor = Color.Black
                };
                //this is your iban may be empty in some cases
                counterpartyidbank = new Label()
                {
                    Text = "Counterparty bank: " + transactionPage.counterbank,
                    Margin = 2,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                //this is the type of account that you have, in some cases may be empty
                typeLabel = new Label()
                {
                    Text = "type: " + transactionPage.typesome,
                    Margin = 2,
                    HorizontalTextAlignment = TextAlignment.Center
                };

                description = new Label()
                {
                    Text = "description: " + transactionPage.description,
                    Margin = 2,
                    HorizontalTextAlignment = TextAlignment.Center
                };

                completed = new Label()
                {
                    Text = "Date of Completion: " + transactionPage.completed,
                    Margin = 2,
                    HorizontalTextAlignment = TextAlignment.Center
                };

                newbalanceamount = new Label()
                {
                    Text = "NewBalance " + transactionPage.newbalanceamount,
                    Margin = 2,
                    HorizontalTextAlignment = TextAlignment.Center
                };

                newbalancecurrency = new Label()
                {
                    Text = "Currency " + transactionPage.newbalancecurrency,
                    Margin = 2,
                    HorizontalTextAlignment = TextAlignment.Center
                };

                valueamount = new Label()
                {
                    Text = "Amount: " + transactionPage.valueamount,
                    Margin = 2,
                    HorizontalTextAlignment = TextAlignment.Center
                };
                IsBusy = false;
                //may need to change order
                //Layout of the accounts detailed information
                AccountLayout = new StackLayout()
                {
                    BackgroundColor = Color.Gray,
                    Margin = 10,
                    Children =
                    {
                        idTransLabel,
                        myAccountid,
                        myAccountBank,
                        counterpartyid,
                        counterpartyidbank,
                        valueamount,
                        newbalanceamount,
                        newbalancecurrency,
                        completed,
                        description,
                        typeLabel,
                    }
                };
                Content = AccountLayout;
            }
            catch (Exception err)
            {
                IsBusy = false;
                await DisplayAlert("Alert", "There was a problem sorry :(", "OK");
                Debug.WriteLine("Caught error transactionpage: {0}.", err);
            }
        }
    }
}