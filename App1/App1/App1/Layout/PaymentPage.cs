using App1.Models;
using App1.REST;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace App1.Layout
{
    //Payment page used to layout its aspect and what its functions are going to be
    internal class PaymentPage : ContentPage
    {
        private Entry _bankEntry, _userEntry, _currencyEntry, _amountEntry, _descriptionEntry;
        private StackLayout menuLayout;

        public PaymentPage()
        {
            //the bank entity to whom the owner of the account you want to pay
            _bankEntry = new Entry
            {
                Placeholder = "Bank_id"
            };

            //account id used to send the payment to
            _userEntry = new Entry
            {
                Placeholder = "Account_id"
            };

            //currency during the payment
            _currencyEntry = new Entry()
            {
                Placeholder = "Currency"
            };

            //amount the user is willing to pay
            _amountEntry = new Entry()
            {
                Placeholder = "amount to user"
            };

            //Entry for the user to do a description of the payment
            _descriptionEntry = new Entry()
            {
                Placeholder = "description"
            };

            //Confirmation button used to confirm payment
            Button confirmationButton = new Button
            {
                Text = "Payment",
            };
            confirmationButton.Clicked += OnConfirmationButtonClicked;

            //Layout of the Payment Page
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

            //innergrids used to define what each of the attributes shall be
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

            innerGrid.Children.Add(new Label() { Text = "To bank_id" }, 0, 0);
            innerGrid.Children.Add(_bankEntry, 0, 1);
            innerGrid.Children.Add(new Label() { Text = "To Account_id" }, 0, 2);
            innerGrid.Children.Add(_userEntry, 0, 3);
            innerGrid.Children.Add(new Label() { Text = "currency add EUR" }, 0, 4);
            innerGrid.Children.Add(_currencyEntry, 0, 5);
            innerGrid.Children.Add(new Label() { Text = "Amount" }, 0, 6);
            innerGrid.Children.Add(_amountEntry, 0, 7);
            innerGrid.Children.Add(new Label() { Text = "Description" }, 0, 8);
            innerGrid.Children.Add(_descriptionEntry, 0, 9);
            innerGrid.Children.Add(confirmationButton, 0, 10);

            //this is the type of layout the grids will be specified in
            var stackLayout = new StackLayout
            {
                BackgroundColor = Color.Teal,
                Padding = 1
            };
            //Stack layout used to define the whole layout of the page
            stackLayout.Children.Add(outergrid);
            this.Content = stackLayout;
        }

        //What will happen if the user clicks the confirmation button, the users entry information shall be sent and verified if correct
        //if the result is true then a message shall be displayed afirming that statement else another message shall display that something went wrong
        private async void OnConfirmationButtonClicked(object sender, EventArgs e)
        {
            var rest = new ManagerRESTService(new RESTService());
            //account info to whom i am making the payment to
            var accountTo = new Payments.To
            {
                account_id = _userEntry.Text
            };
            //bankid thats connected to the account specified on top to whom i am making the payment to
            var bankTo = new Payments.To
            {
                bank_id = _bankEntry.Text
            };
            //currency chosen to make the payment
            var currencyTo = new Payments.Value
            {
                currency = _currencyEntry.Text
            };
            //the amount that the user is willing to pay
            var amountTo = new Payments.Value
            {
                amount = _amountEntry.Text
            };
            //a simple description of the transaction
            var descriptionTo = new Payments.Body
            {
                description = _descriptionEntry.Text
            };

            //Sent to the RestService to verify the information received on this page to then be treated
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