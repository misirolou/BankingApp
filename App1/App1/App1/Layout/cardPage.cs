using Xamarin.Forms;

namespace App1.Layout
{
    internal class cardPage : ContentPage
    {
        public cardPage()
        {
            Title = "CardsPage";
            Icon = new FileImageSource { File = "robot.png" };
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "CardsPage should have most of your card information",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }
}