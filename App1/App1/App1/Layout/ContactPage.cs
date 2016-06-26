using App1.Cell;
using App1.Models;
using App1.REST;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    //Used to display all the contact information available about all the banks available at OpenBank Project
    internal class ContactPage : ContentPage
    {
        private Label resultsLabel;
        private SearchBar searchBar;
        private ListView _listView;
        protected Banklist test;

        public ContactPage()
        {
            resultsLabel = new Label
            {
                Text = "Result will appear here.",
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            //searchbar to search banks contacts that are available
            searchBar = new SearchBar
            {
                Placeholder = "Enter full_name of bank",
                SearchCommand = new Command(() => { resultsLabel.Text = "Result: " + searchBar.Text + " here you go"; })
            };

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

            //Layout of the Contact Page, containig its title, image and final layout of the page
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            Title = "ContactPage";
            Icon = new FileImageSource { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            Content = new StackLayout
            {
                BackgroundColor = Color.Teal,
                Children =
                {
                    new Label {Text = "Getting the contact information", HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.Center},
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
                var uri = string.Format(Constants.BankUrl);

                //getting information from the online location
                await rest.GetwithoutToken<Banklist>(uri).ContinueWith(t =>
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
                            test = t.Result;
                            

                            _listView = new ListView
                            {
                                HasUnevenRows = true,
                                Margin = 10,
                                SeparatorColor = Color.Teal
                            };
                            _listView.ItemsSource = test.banks;
                            _listView.ItemTemplate = new DataTemplate(typeof(Cells));
                        });
                    }
                });
                //indicates the activity indicator that all the information is loaded and ready
                IsBusy = false;

                searchBar.TextChanged += (sender, e) => FilterContacts(searchBar.Text);
                searchBar.SearchButtonPressed += (sender, e) => {
                    FilterContacts(searchBar.Text);
                };

                Content = new StackLayout
                {
                    BackgroundColor = Color.Teal,
                    Spacing = 10,
                    Children =
                    {
                        new Label {Text = "Contact list go up and down", HorizontalTextAlignment = TextAlignment.Center},
                        searchBar,
                        _listView
                    }
                };
            }
            catch (Exception err)
            {
                IsBusy = false;
                await DisplayAlert("Alert", "Cant receive information", "OK");
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }

        public void FilterContacts(string filter)
        {
            _listView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(filter))
            {
                _listView.ItemsSource = test.banks;
            }
            else
            {
                Debug.WriteLine("test first bank {0}",test.banks[0].full_name);
                _listView.ItemsSource = test.banks
                    .Where(x => x.short_name.ToLower()
                   .Contains(filter.ToLower()));
            }

            _listView.EndRefresh();
        }
    }
}