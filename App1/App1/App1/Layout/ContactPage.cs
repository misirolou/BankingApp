using App1.Cell;
using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using App1.REST;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class ContactPage : ContentPage
    {
        private List<Banklist> banklist;
        private Label resultsLabel;
        private SearchBar searchBar;
        private ListView listView;

        public ContactPage()
        {
            Button Back = new Button()
            {
                Image = (FileImageSource)Device.OnPlatform(
                        iOS: ImageSource.FromFile("Back.png"),
                        Android: ImageSource.FromFile("Back.png"),
                        WinPhone: ImageSource.FromFile("Back.png")),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.Gray
            };
            Back.Clicked += BackButtonClicked;

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

            Takingcareofbussiness();

            //ObservableCollection<banks> bankList = new ObservableCollection<banks>();

            //this will contain the list of information from the banklist individually packed each into a cell according to the layout specified
          /*  listView = new ListView
            {
                BackgroundColor = Color.Gray,
                ItemsSource = bankList,
                ItemTemplate = new DataTemplate(() =>
                    {
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
            //Layout of the Contact Page, containig its title, image and final layout of the page
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            Title = "ContactPage";
            Icon = new FileImageSource { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            Content = new StackLayout
            {
                BackgroundColor = Color.Teal,
                Children = {
                    new Label { Text = "Testing contacts", HorizontalTextAlignment = TextAlignment.Center },
                   // indicator,
                    listView
                    }
            };
        }

        private async void Takingcareofbussiness()
        {
            try
            {
                var rest = new ManagerRESTService(new RESTService());
                var uri = string.Format(Constants.BankUrl);
                var some = await rest.GetwithoutToken<Banklist>(uri);
                Debug.WriteLine("done request this is the response {0}",some);
                //var banklist = JsonConvert.DeserializeObject<Banklist>();
                foreach (var item in some.banks)
                {
                    Debug.WriteLine("itemid :{0}", item.id);
                }

                listView = new ListView
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
                };
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}