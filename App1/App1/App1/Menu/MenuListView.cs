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
            List<MenuItem> data = new MenuListData();

            ItemsSource = data;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;
            SeparatorVisibility = SeparatorVisibility.None;

            var cell = new DataTemplate(typeof(Cells));
            cell.SetBinding(Cells.MenuTitleProperty, "Title");

            ItemTemplate = cell;
        }
    }
}