using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    class cardPage : ContentPage
    {
        public cardPage()
        {
            Title = "CardsPage";
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "CardsPage should have most of your card information",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }
}

