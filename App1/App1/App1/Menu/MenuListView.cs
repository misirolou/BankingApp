using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace App1.Menu
{
    internal class MenuListView : ListView
    {
        //Aspect of the menu list information
        public MenuListView()
        {
            //list of all the items that will be used in the menu
            List<MenuItem> data = new MenuListData();

            //data that is the list already identified on top and its backgroundcolor and vertical aspect
            HasUnevenRows = true;
            Margin = 10;
            BackgroundColor = Color.Gray;
            ItemsSource = data;
            foreach (var items in data)
            {
                Debug.WriteLine("items: {0} ---- {1}", items.Title, items.TargetType);
            }
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;

            //aspect of each of the menus cells that will contain the page title
            var cell = new DataTemplate(typeof(TextCell));
            cell.SetBinding(TextCell.TextProperty, "Title");
            ItemTemplate = cell;
        }
    }
}