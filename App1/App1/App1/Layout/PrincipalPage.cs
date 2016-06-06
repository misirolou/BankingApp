using App1.Cell;
using App1.Models;
using App1.REST;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class PrincipalPage : ContentPage
    {
        private ListView _listView;
        private StackLayout stackLayout;

        public PrincipalPage()
        {
            //Transaction button used to change to the transaction page
            var transactionButton = new Button
            {
                Text = "Transactions",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            transactionButton.Clicked += OntransactionButtonClicked;

            //bank cards button used to change to the cards page
            var cardsbutton = new Button()
            {
                Text = "Cards",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            cardsbutton.Clicked += OncardsButtonClicked;

            //button that when tapped will change to the menu page
            Button menuButton = new Button()
            {
                Image = new FileImageSource() { File = "menu.png" }
            };
            menuButton.Clicked += async (sender, args) => await Navigation.PushAsync(new MenuPage());

            //button that when tapped will change to the menu page may need to do some things beforehand
            Button exitButton = new Button()
            {
                Image = new FileImageSource() { File = "Exit.png" }
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

            //Layout of the Home page(PrincipalPage.cs)
            Title = "Home";
            Icon = new FileImageSource() { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            //this is the type of layout the grids will be specified in
            stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Padding = 1,
                Children = { transactionButton, cardsbutton }
            };

            Content = new StackLayout()
            {
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Padding = 1,
                Children =
                {
                     new Label {Text = "Getting your account information", HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.Center},
                     stackLayout,
                    indicator
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
                var uri = string.Format(Constants.AccountUrl);

                //getting information from the online location
                await rest.GetWithToken<Accounts.Detail>(uri).ContinueWith(t =>
                {
                    //Problem occured a message is displayed to the user
                    if (t.IsFaulted)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Alert", "Something went wrong sorry with your account information :(", "OK");
                        });
                    }
                    //going to next request needed to display more detailed information of the users account
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            Debug.WriteLine("t result of Accounts {0}",t.Result);
                            Debug.WriteLine("getting href detailed {0}", t.Result.href);
                            await rest.GetWithToken<AccountInfo.AccountInfoDetailed>(t.Result.href).ContinueWith(task =>
                            {
                                //Problem occured a message is displayed to the user
                                if (task.IsFaulted)
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        DisplayAlert("Alert", "Something went wrong sorry with your detailed information :(", "OK");
                                    });
                                }
                                //everything went fine, information should be displayed
                                else
                                {
                                    Debug.WriteLine("result tostring info detailed {0}", task.Result.ToString());
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        _listView = new ListView
                                        {
                                            BackgroundColor = Color.Gray,
                                            HasUnevenRows = true
                                        };
                                        _listView.ItemsSource = task.Result.ToString();
                                        _listView.ItemTemplate = new DataTemplate(typeof(AccountCell));
                                    });
                                }
                            });
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
                        new Label {Text = "Contact list go up and down", HorizontalTextAlignment = TextAlignment.Center},
                        _listView,
                        stackLayout
                    }
                };
            }
            catch (Exception err)
            {
                IsBusy = false;
                await DisplayAlert("Alert", "Internet problems cant receive information", "OK");
                Debug.WriteLine("Caught error principalpage: {0}.", err);
            }
        }

        //should take te user to the transaction page
        private async void OntransactionButtonClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new transactionPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error transactionpage: {0}.", err);
            }
        }

        //should take the user to the cards page
        private async void OncardsButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Clicked cards button");
            try
            {
                await Navigation.PushAsync(new cardPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error cardspage: {0}.", err);
            }
        }
    }
}