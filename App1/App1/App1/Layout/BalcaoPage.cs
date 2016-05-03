using Xamarin.Forms;

namespace App1.Layout
{
    internal class BalcaoPage : ContentPage
    {
        public BalcaoPage()
        {
            Title = "BalcaoPage";
            Icon = new FileImageSource { File = "robot.png" };
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "BalcaoPage should have a map of Banks",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }
}