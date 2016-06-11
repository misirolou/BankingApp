using App1.Menu;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class MenuPage : ContentPage
    {
        public ListView Menu { get; set; }

        //what the menu page will look like
        public MenuPage()
        {
            var data = new MenuListData();

            Title = "menu"; // The Title property must be set.
            BackgroundColor = Color.Teal;
            Icon = new FileImageSource { File = "robot.png" };

            Menu = new ListView()
            {
                SeparatorVisibility = SeparatorVisibility.Default,
                SeparatorColor = Color.Teal,
                BackgroundColor = Color.Gray
            };
            Menu.ItemsSource = data;
            //aspect of each of the menus cells that will contain the page title
            var cell = new DataTemplate(typeof(TextCell));
            cell.SetBinding(TextCell.TextProperty, "Title");
            Menu.ItemTemplate = cell;

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