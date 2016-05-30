using Xamarin.Forms;

namespace App1.Layout
{
    internal class cardPage : ContentPage
    {
        public cardPage()
        {
            Title = "CardsPage";
            Icon = new FileImageSource { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "CardsPage should have most of your card information no information on OpenBank so thats all there is to it",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }
}