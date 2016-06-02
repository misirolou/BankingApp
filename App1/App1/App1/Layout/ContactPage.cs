using App1.Cell;
using App1.Models;
using App1.REST;
using App1.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class ContactPage : ContentPage
    {
        private List<Banklist> banklist;
        private Label resultsLabel, something;
        private SearchBar searchBar;
        private ListView _listView;

        private ContactViewModel ViewModel { get; set; }

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
                Placeholder = "Enter short_name of bank",
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

            var x = Takingcareofbussiness();

            Debug.WriteLine("xresult {0}", x.Result);

            _listView = new ListView
            {
                BackgroundColor = Color.Gray,
                ItemsSource = x.Result,
                ItemTemplate = new DataTemplate(() =>
                {
                    Debug.WriteLine("trying to make cells");
                    var nativeCell = new Cells();

                    nativeCell.SetBinding(Cells.IDProperty, "id");
                    nativeCell.SetBinding(Cells.NameProperty, "short_name");
                    nativeCell.SetBinding(Cells.full_nameProperty, "full_name");
                    nativeCell.SetBinding(Cells.LogoProperty, "logo");
                    nativeCell.SetBinding(Cells.WebsiteProperty, "website");
                    Debug.WriteLine("nativecell:: {0}", nativeCell);
                    return nativeCell;
                })
            };

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
                    new Label {Text = "Testing contacts", HorizontalTextAlignment = TextAlignment.Center},
                    indicator,
                    _listView
                }
            };
        }

        private async Task<string> Takingcareofbussiness()
        {
            try
            {
                ViewModel.IsBusy = true;

                var rest = new ManagerRESTService(new RESTService());
                var uri = string.Format(Constants.BankUrl);
                var some = await rest.GetwithoutToken<Banklist>(uri);
                Debug.WriteLine("done request this is the response {0}", some);
                //var banklist = JsonConvert.DeserializeObject<Banklist>();
                foreach (var item in some.banks)
                {
                    Debug.WriteLine("itemid :{0}", item.id);
                }
                /* _listView = new ListView
                 {
                     BackgroundColor = Color.Gray,
                     ItemsSource = some.banks,
                     ItemTemplate = new DataTemplate(() =>
                     {
                         Debug.WriteLine("trying to make cells");
                         var nativeCell = new Cells();

                         nativeCell.SetBinding(Cells.IDProperty, "id");
                         nativeCell.SetBinding(Cells.NameProperty, "short_name");
                         nativeCell.SetBinding(Cells.full_nameProperty, "full_name");
                         nativeCell.SetBinding(Cells.LogoProperty, "logo");
                         nativeCell.SetBinding(Cells.WebsiteProperty, "website");
                         Debug.WriteLine("nativecell:: {0}", nativeCell);
                         return nativeCell;
                     })
                 };*/
                IsBusy = false;
                Debug.WriteLine("listview {0}", _listView.Header);
                return some.banks.ToString();
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
                IsBusy = false;
                return err.Message;
            }
        }
    }
}