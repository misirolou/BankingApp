using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    class MenuPage : ContentPage
    {
        public MenuPage()
        {
           /* Title = "Menu";
            Icon = new FileImageSource { File = "robot.png" };*/
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "Menu page that should overlay the other pages",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }

    }
}
