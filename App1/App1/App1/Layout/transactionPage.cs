using Xamarin.Forms;

namespace App1.Layout
{
    internal class transactionPage : ContentPage
    {
        public transactionPage()
        {
            Title = "TransactionsPage";
            Icon = new FileImageSource { File = "robot.png" };
            Content = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "TransactionsPage should have most of the users transactions",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }
}