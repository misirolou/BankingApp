using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    class BalcaoPage : ContentPage
    {
        public BalcaoPage()
        {

            Title = "BalcaoPage";
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "BalcaoPage should have a map of Banks",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }
}
