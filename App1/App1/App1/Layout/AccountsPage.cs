using App1.Cell;
using App1.Models;
using App1.REST;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    //This page contains all the users accounts,  determinig its layout
    public class AccountsPage : ContentPage
    {
        private ListView _listView;
        private StackLayout labelLayout;
        private StackLayout menuLayout;
        private SearchBar searchBar;
        private Accounts.Account[] ListAccounts;
        private Label _accountidLabel, _bankidLabel;

        //Used to identify the href, bankid and accountid that was chosen from the cells
        public static string Href { get; private set; }

        public static string Bankid { get; private set; }
        public static string Accountid { get; private set; }

        //Determines the layout of the page and some of its functionalities
        public AccountsPage()
        {
            _accountidLabel = new Label()
            {
                Text = "Account ID",
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            _bankidLabel = new Label()
            {
                Text = "Bank ID",
                HorizontalOptions = LayoutOptions.StartAndExpand
            };

            searchBar = new SearchBar
            {
                Placeholder = "Enter id of bank",
            };

            //exitbutton used to exit the application to the loginpage
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

            //This is used to indicate that their is something loading in the background
            ActivityIndicator indicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                IsRunning = true,
                IsVisible = true
            };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            indicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy");

            //Task used to receive the information that will ve presented in the page
            Task.WhenAll(Takingcareofbussiness());

            //Label layout used to identify the fields used in the accounts table
            labelLayout = new StackLayout()
            {
                BackgroundColor = Color.Gray,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Margin = 10,
                Children =
                { _bankidLabel , _accountidLabel }
            };

            //menu layouut used to determine the layout of the menu
            menuLayout = new StackLayout()
            {
                BackgroundColor = Color.Gray,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                Margin = 10,
                Children = {
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
                        Text = "AccountPage should have most of the your accounts", HorizontalTextAlignment = TextAlignment.Center,
                    }
                }
            };
        }

        //This function is used to take care of bussiness, receiving the information requseted going through the REST service used to receive information
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
                            ListAccounts = JsonConvert.DeserializeObject<Accounts.Account[]>(t.Result);

                            //List used to list information received from the REST service
                            _listView = new ListView
                            {
                                HasUnevenRows = true,
                                Margin = 10,
                                SeparatorColor = Color.Teal,
                                ItemsSource = ListAccounts,
                                ItemTemplate = new DataTemplate(typeof(AllAccountsCell))
                            };
                            _listView.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as Accounts.Account);
                        });
                    }
                });
                //indicates the activity indicator that all the information is loaded and ready
                IsBusy = false;

                searchBar.TextChanged += (sender, e) => FilterBanks(searchBar.Text);
                searchBar.SearchButtonPressed += (sender, e) =>
                {
                    FilterBanks(searchBar.Text);
                };

                //Determinig the new layout containg the information received
                Content = new StackLayout
                {
                    BackgroundColor = Color.Teal,
                    Spacing = 10,
                    Children =
                    {
                        menuLayout,
                        searchBar,
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

        //Filter bank ids
        private void FilterBanks(string filter)
        {
            _listView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(filter))
            {
                _listView.ItemsSource = ListAccounts;
            }
            else
            {
                _listView.ItemsSource = ListAccounts
                    .Where(x => x.bank_id.ToLower()
                   .Contains(filter.ToLower()));
            }

            _listView.EndRefresh();
        }

        //used according to the cell that is selected to navigateTo to the new PrincipalPage used to show the detailed information of the account selected
        private async void NavigateTo(Accounts.Account account)
        {
            if (account == null)
                return;

            //Account id, bank id and HREF that will be called in other future page
            Accountid = account.id;
            Debug.WriteLine("accountid in accountpage {0}", Accountid);
            Bankid = account.bank_id;
            Debug.WriteLine("bankid in accountpage {0}", Bankid);
            Href = account._links.detail.href;
            Debug.WriteLine("href in accountpage {0}", Href);

            try
            {
                //Navigation to the new page
                await Navigation.PushAsync(new PrincipalPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error transactionpage: {0}.", err);
            }
        }
    }
}