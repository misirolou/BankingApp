using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    class PrincipalPage : ContentPage
    {
        public PrincipalPage() 
        {
            Title = "PrincipalPage";
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "Principal Page of the App",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
        
    }
}
