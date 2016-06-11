using App1.Cell;
using App1.Models;
using App1.REST;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Android;
using Android.App;
using App1.Menu;
using Xamarin.Forms;

namespace App1.Layout
{
    public class AccountsPage : ContentPage
    {
        private ListView _listView;
        private StackLayout labelLayout;
        private StackLayout menuLayout;
        private Label _accountidLabel, _bankidLabel;
        public static string Href { get; private set; }
        public static string Bankid { get; private set; }
        public static string Accountid { get; private set; }

        public AccountsPage()
        {
            _accountidLabel = new Label()
            {
                Text = "Account ID",
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            _bankidLabel = new Label()
            {
                Text = "Bank ID",
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            Button menuButton = new Button()
            {
                Image = (FileImageSource)Device.OnPlatform(
                    iOS: ImageSource.FromFile("menu.png"),
                    Android: ImageSource.FromFile("menu.png"),
                    WinPhone: ImageSource.FromFile("menu.png")),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Gray
            };
            menuButton.Clicked += async (sender, args) => await Navigation.PushAsync(new MenuPage());

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

            ActivityIndicator indicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                IsRunning = true,
                IsVisible = true
            };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            indicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

            Task.WhenAll(Takingcareofbussiness());

            labelLayout = new StackLayout()
            {
                BackgroundColor = Color.Gray,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Margin = 10,
                Children =
                { _accountidLabel, _bankidLabel}
            };

            menuLayout = new StackLayout()
            {
                BackgroundColor = Color.Gray,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Margin = 10,
                Children = { menuButton,
                    new Label { Text = "All of your accounts", HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold },
                    exitButton
                }
            };


            //layout of the accounts page
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            Title = "Accounts";
            Icon = new FileImageSource { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            BackgroundColor = Color.Teal;
            Content = new StackLayout
            {
                BackgroundColor = Color.Gray,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    new Label
                    {
                        Text = "TransactionsPage should have most of the users transactions", HorizontalTextAlignment = TextAlignment.Center,
                    }
                }
            };
        }

        private async Task Takingcareofbussiness()
        {
            //trying to get information online if some error occurs this is caught and taken care of, a message is displayed in this case
            try
            {
                //indicates the activity indicator to start
                IsBusy = true;

                var rest = new ManagerRESTService(new RESTService());
                var uri = String.Format(Constants.AccountUrl);

                //getting information from the online location
                await rest.GetWithToken(uri).ContinueWith(t =>
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
                            Accounts.Account[] jsonObject = JsonConvert.DeserializeObject<Accounts.Account[]>(t.Result);

                            _listView = new ListView
                            {
                                HasUnevenRows = true,
                                Margin = 10,
                                SeparatorColor = Color.Teal,
                                ItemsSource = jsonObject,
                                BackgroundColor = Color.Gray,
                                ItemTemplate = new DataTemplate(typeof(AllAccountsCell))
                            };
                            _listView.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as Accounts.Account);
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
                        labelLayout,
                        _listView
                    }
                };
            }
            catch (Exception err)
            {
                IsBusy = false;
                await DisplayAlert("Alert", "Internet problems cant receive information", "OK");
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }

        private async void NavigateTo(Accounts.Account account)
        {
            if (account == null)
                return;

            Accountid = account.id;
            Debug.WriteLine("accountid in accountpage {0}", Accountid);
            Bankid = account.bank_id;
            Debug.WriteLine("bankid in accountpage {0}", Bankid);
            Href = account._links.detail.href;
            Debug.WriteLine("href in accountpage {0}", Href);

            try
            {
                await Navigation.PushAsync(new PrincipalPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error transactionpage: {0}.", err);
            }
        }
    }
}