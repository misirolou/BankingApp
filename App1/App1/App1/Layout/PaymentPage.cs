using App1.Models;
using App1.REST;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class PaymentPage : ContentPage
    {
        private Entry BankEntry, UserEntry, CurrencyEntry, amountEntry, descriptionEntry;
        private StackLayout menuLayout;

        public PaymentPage()
        {
            BankEntry = new Entry
            {
                Placeholder = "Bank_id"
            };

            UserEntry = new Entry
            {
                Placeholder = "Account_id"
            };

            CurrencyEntry = new Entry()
            {
                Placeholder = "Currency"
            };

            amountEntry = new Entry()
            {
                Placeholder = "amount to user"
            };

            descriptionEntry = new Entry()
            {
                Placeholder = "description"
            };

            Button confirmationButton = new Button
            {
                Text = "Payment",
            };
            confirmationButton.Clicked += OnConfirmationButtonClicked;

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
                    new Label { Text = "All of your accounts", HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold },
                    exitButton
                }
            };

            //Layout of the login page
            Title = "PaymentsPage";
            Icon = new FileImageSource() { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            //All the grids are contained in this outergrid
            var outergrid = new Grid()
            {
                Margin = 5,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(3, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
                }
            };

            var innerGrid = new Grid
            {
                Margin = 5,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                RowDefinitions =
                {
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)}
                }
            };

            outergrid.Children.Add(new Label() { Text = "Please add the required information", FontAttributes = FontAttributes.Bold }, 0, 0);
            outergrid.Children.Add(innerGrid, 0, 1);

            innerGrid.Children.Add(BankEntry, 0, 0);
            innerGrid.Children.Add(UserEntry, 0, 1);
            innerGrid.Children.Add(CurrencyEntry, 0, 2);
            innerGrid.Children.Add(amountEntry, 0, 3);
            innerGrid.Children.Add(descriptionEntry, 0, 4);
            innerGrid.Children.Add(confirmationButton, 0, 5);

            //this is the type of layout the grids will be specified in
            var stackLayout = new StackLayout
            {
                BackgroundColor = Color.Teal,
                Padding = 1
            };

            stackLayout.Children.Add(outergrid);
            this.Content = stackLayout;
        }

        private async void OnConfirmationButtonClicked(object sender, EventArgs e)
        {
            var rest = new ManagerRESTService(new RESTService());
            //account info to whom i am making the payment to
            var accountTo = new Payments.To
            {
                account_id = UserEntry.Text
            };
            //bankid thats connected to the account specified on top to whom i am making the payment to
            var bankTo = new Payments.To
            {
                bank_id = BankEntry.Text
            };
            //currency chosen to make the payment
            var currencyTo = new Payments.Value
            {
                currency = CurrencyEntry.Text
            };
            //the amount that the user is willing to pay
            var amountTo = new Payments.Value
            {
                amount = amountEntry.Text
            };
            //a simple description of the transaction
            var descriptionTo = new Payments.Body
            {
                description = descriptionEntry.Text
            };
            //Verfication of users information through OpenBanks Direct Login where the user should receive a token
            //this token is never shown to the user, used in background functions to request authorized information for the user
            Debug.WriteLine("User put rubbish");

            string requestBody = "{\"to\":{\"bank_id\":\"" + bankTo.bank_id + "\",\"account_id\":\"" + accountTo.account_id +
                                 "\"},\"value\":\"{\"currency\":\"" + currencyTo.currency + "\",\"amount\":\"" + amountTo.amount +
                                 "\"},\"description\":\"" + descriptionTo.description + "\"}";
            Debug.WriteLine("BODY: {0}", requestBody);

            var result = await rest.MakePayment(accountTo, bankTo, currencyTo, amountTo, descriptionTo);
            Debug.WriteLine("result {0}", result);
            //if the result is false it will stay on the same page and show the message stated else it will change to the next page
            if (result)
            {
                try
                {
                    await DisplayAlert("Confirmed", "Transaction Completed", "Ok");
                }
                catch (Exception err)
                {
                    Debug.WriteLine("Caught error: {0}.", err);
                }
            }
            else
            {
                await DisplayAlert("Alert", "Something happened :(", "OK");
            }
        }
    }
}