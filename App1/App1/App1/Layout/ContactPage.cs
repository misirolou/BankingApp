using App1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Json;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class ContactPage : ContentPage
    {

        public List<Banks> banks { get; set; }

        private Label BankInfo;

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

            BankInfo = new Label();
            //The contact page will contain the banks URLs to websites
            Title = "ContactPage";
            Icon = new FileImageSource { File = "Phone.png" };
            var stackLayout = new StackLayout
            {
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Children = {
                    Back,
                    BankInfo,
                    new Label() {
                        BackgroundColor = Color.Gray,
                        Text = "",
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Start
                    },
                    new Label {
                        BackgroundColor = Color.Gray,
                        Text = "Bank 2 : OBP",
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Start
                    },
                    new Label {
                        BackgroundColor = Color.Gray,
                        Text = "Bank 3 : OBP2",
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Start
                    }
                }
            };
            stackLayout.Children.Add(BankInfo);
            this.Content = stackLayout;
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}