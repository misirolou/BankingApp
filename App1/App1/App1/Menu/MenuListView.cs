using App1.Cell;
using System.Collections.Generic;
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
            ItemsSource = data;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;

            //aspect of each of the menus cells that will contain the page title
            var cell = new DataTemplate(typeof(Cells));
            cell.SetBinding(Cells.MenuTitleProperty, "Title");
            ItemTemplate = cell;
        }
    }
}