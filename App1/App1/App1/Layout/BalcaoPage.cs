using System;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class BalcaoPage : ContentPage
    {
            public BalcaoPage()
            {
                Button Back = new Button()
                {
                    Image = (FileImageSource) Device.OnPlatform(
                        iOS: ImageSource.FromFile("Back.png"),
                        Android: ImageSource.FromFile("Back.png"),
                        WinPhone: ImageSource.FromFile("Back.png")),
                        VerticalOptions = LayoutOptions.Start,
                        HorizontalOptions = LayoutOptions.EndAndExpand,
                        BackgroundColor = Color.Gray
                };
                Back.Clicked += BackButtonClicked;
                Title = "BalcaoPage";
                Icon = new FileImageSource { File = "robot.png" };
                Content = new StackLayout
                {
                    Children = {
                        Back,
                        new Label {
                            Text = "BalcaoPage should have a map of Banks",
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center
                        }
                    }
                };
        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }
    }
}