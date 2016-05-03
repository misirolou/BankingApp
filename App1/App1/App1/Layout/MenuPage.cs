using Xamarin.Forms;

namespace App1.Layout
{
    internal class MenuPage : ContentPage
    {
        public MenuPage()
        {
            /* Title = "Menu";
             Icon = new FileImageSource { File = "robot.png" };*/
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "Menu page that should overlay the other pages",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }
}