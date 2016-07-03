using App1.Cell;
using App1.Models;
using App1.REST;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    //Product page listing all the products available from a certain bank
    internal class ProductPage : ContentPage
    {
        private ListView _listView;
        public StackLayout menuLayout;
        public Grid Labelgrid;

        //Layout and functionalities
        public ProductPage()
        {
            ActivityIndicator indicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                IsRunning = true,
                IsVisible = true
            };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            indicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

            Button exitButton = new Button()
            {
                Image = (FileImageSource)Device.OnPlatform(
                    iOS: ImageSource.FromFile("Exit.png"),
                    Android: ImageSource.FromFile("Exit.png"),
                    WinPhone: ImageSource.FromFile("Exit.png")),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.Gray
            };
            exitButton.Clicked += async (sender, args) => await Navigation.PopToRootAsync();

            menuLayout = new StackLayout()
            {
                BackgroundColor = Color.Gray,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Margin = 10,
                Children =
                {
                    new Label
                    {
                        Text = "your Account Information",
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontAttributes = FontAttributes.Bold
                    },
                    exitButton
                }
            };

            Labelgrid = new Grid();
            Labelgrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            Labelgrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Labelgrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Labelgrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Labelgrid.Children.Add(new Label() { Text = "Name", FontAttributes = FontAttributes.Bold }, 1, 0);
            Labelgrid.Children.Add(new Label() { Text = "Family", FontAttributes = FontAttributes.Bold }, 2, 0);
            Labelgrid.Children.Add(new Label() { Text = "Super_family", FontAttributes = FontAttributes.Bold }, 3, 0);

            Task.WhenAll(Takingcareofbussiness());

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
                    },
                    indicator
                }
            };
        }

        //Taking care of bussiness, Product page used to list all the products from a certain bank
        private async Task Takingcareofbussiness()
        {
            //trying to get information online if some error occurs this is caught and taken care of, a message is displayed in this case
            try
            {
                //indicates the activity indicator to start
                IsBusy = true;

                var rest = new ManagerRESTService(new RESTService());
                var uri = string.Format(Constants.ProductsUrl, AccountsPage.Bankid);

                //getting information from the online location
                await rest.GetwithoutToken<productlist>(uri).ContinueWith(t =>
                {
                    //Problem occured a message is displayed to the user
                    if (t.IsFaulted)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Alert", "Something went wrong sorry :(", "OK");
                        });
                    }
                    //everything went fine, information should be displayed
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _listView = new ListView
                            {
                                HasUnevenRows = true,
                                Margin = 10,
                                SeparatorColor = Color.Teal
                            };
                            _listView.ItemsSource = t.Result.products;
                            _listView.ItemTemplate = new DataTemplate(typeof(productCells));
                        });
                    }
                });
                //indicates the activity indicator that all the information is loaded and ready
                IsBusy = false;
                Content = new StackLayout
                {
                    BackgroundColor = Color.Teal,
                    Spacing = 10,
                    Children =
                    {
                        menuLayout,
                        Labelgrid,
                        new Label {Text = "Product list go up and down", HorizontalTextAlignment = TextAlignment.Center},
                        _listView
                    }
                };
            }
            catch (Exception err)
            {
                IsBusy = false;
                await DisplayAlert("Alert", "Internet problems ", "OK");
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }
    }
}