using App1.Cell;
using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class ContactPage : ContentPage
    {
        public List<banks> banks { get; set; }

        private Label BankInfo;

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

            var banks = new RootObject();
            foreach (var item in banks.banks)
            {
                Debug.WriteLine("item ids {0}    length {1}   item{2}", item.id, item.id.Length, item);
            }

            listView = new ListView
            {
                ItemsSource = bankList,
                ItemTemplate = new DataTemplate(() =>
                    {
                        var nativeCell = new Cells();
                        nativeCell.SetBinding(Cells.NameProperty, "short_name");
                        nativeCell.SetBinding(Cells.full_nameProperty, "full_name");
                        nativeCell.SetBinding(Cells.WebsiteProperty, "website");
                        return nativeCell;
                    })
            };

            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Xamarin.Forms native cell", HorizontalTextAlignment = TextAlignment.Center },
                    listView
                    }
            };
            /* BankInfo = new Label();
             //The contact page will contain the banks URLs to websites
             Title = "ContactPage";
             Icon = new FileImageSource { File = "Phone.png" };
             var stackLayout = new StackLayout
             {
                 BackgroundColor = Color.Teal,
                 Spacing = 10,
                 Children = {
                     Back,
                     BankInfo
                 }
             };
             stackLayout.Children.Add(BankInfo);
             this.Content = stackLayout;*/
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            //Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}