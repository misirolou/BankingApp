using System;
using System.Diagnostics;
using App1.Menu;
using App1.Models;
using Xamarin.Forms;
using MenuItem = App1.Menu.MenuItem;

namespace App1.Layout
{
    internal class MenuPage : ContentPage
    {
        public ListView Menu { get; set; }

        //what the menu page will look like
        public MenuPage()
        {
            Title = "menu"; // The Title property must be set.
            BackgroundColor = Color.Teal;
            Icon = new FileImageSource { File = "robot.png" };

            Menu = new MenuListView
            {
                SeparatorVisibility = SeparatorVisibility.Default,
                SeparatorColor = Color.Gray,
                RowHeight = 10
            };


            var menuLabel = new ContentView
            {
                Padding = new Thickness(10, 30, 0, 5),
                Content = new Label
                {
                    Text = "MENU",
                }
            };

            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(menuLabel);
            layout.Children.Add(Menu);
        }
    }
}