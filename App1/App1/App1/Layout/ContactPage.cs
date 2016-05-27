using App1.Cell;
using App1.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class ContactPage : ContentPage
    {
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

            ObservableCollection<banks> bankList = new ObservableCollection<banks>();

            listView = new ListView
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
            };

            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            Content = new StackLayout
            {
                BackgroundColor = Color.Teal,
                Children = {
                    new Label { Text = "Testing contacts", HorizontalTextAlignment = TextAlignment.Center },
                    listView
                    }
            };
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}