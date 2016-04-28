using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    class ContactPage : ContentPage
    {
        public ContactPage()
        {
            //The contact page will contain the banks URLs to websites
            Title = "ContactPage";
            Icon = new FileImageSource {File = "robot.png"};
            StackLayout stackLayout = new StackLayout
            {

                BackgroundColor = Color.Teal,
                Spacing = 10,
                Children = {
                    new Label {
                        BackgroundColor = Color.Gray,
                        Text = "Bank 1 : RBS URL",
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
            this.Content = stackLayout;
        }
    }
}
