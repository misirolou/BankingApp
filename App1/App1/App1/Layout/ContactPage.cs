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

            Title = "ContactPage";
            Content = new StackLayout
            {
                Children = {
                    new Label {
                        Text = "ContactPage should have a the contacts to the bank probably invented",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }
}
