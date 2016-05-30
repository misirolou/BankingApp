using Xamarin.Forms;

namespace App1.Layout
{
    internal class ProductPage : ContentPage
    {
        public ProductPage()
        {
            Title = "ProductsPage";
            Icon = new FileImageSource { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
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