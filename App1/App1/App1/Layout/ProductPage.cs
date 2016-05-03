using Xamarin.Forms;

namespace App1.Layout
{
    internal class ProductPage : ContentPage
    {
        public ProductPage()
        {
            Title = "Products";
            Icon = new FileImageSource { File = "robot.png" };
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "Product page should contain products of the bank",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }
}