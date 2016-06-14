using System;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class PaymentPage : ContentPage
    {
        private Entry BankEntry, UserEntry, CurrencyEntry, amountEntry, descriptionEntry;

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
                Text = "Login",
            };
            confirmationButton.Clicked += OnConfirmationButtonClicked;

            //Layout of the login page
            Title = "PaymentsPage";
            Icon = new FileImageSource() { File = "robot.png" };

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

            outergrid.Children.Add(new Label() { Text = "Please add the required information" }, 0, 0);
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

        private void OnConfirmationButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}