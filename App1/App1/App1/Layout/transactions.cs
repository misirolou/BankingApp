using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class transactions : ContentPage
    {
        public transactions()
        {
            Title = "TransactionsPage";
            Content = new StackLayout
            {
                Children =
                {
                    new Label
                    {
                        Text = "TransactionsPage should have most of the users transactions",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            };
        }
    }
}
        
        
    

