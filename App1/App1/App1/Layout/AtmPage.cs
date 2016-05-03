using Xamarin.Forms;

namespace App1.Layout
{
    internal class AtmPage : ContentPage
    {
        public AtmPage()
        {
            Title = "AtmPage";
            Icon = new FileImageSource { File = "robot.png" };
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "AtmPage should have a map of ATMs",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }
}