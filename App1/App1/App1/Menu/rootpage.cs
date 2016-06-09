using App1.Layout;
using System;
using Xamarin.Forms;

namespace App1.Menu
{
    public class rootpage : MasterDetailPage
    {
        private MenuPage _menuPage;

        //prepare the menu page
        public rootpage()
        {
            _menuPage = new MenuPage();

            _menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as MenuItem);

            Master = _menuPage;
        }

        //Navigate to the page selected in the menu
        private void NavigateTo(MenuItem menu)
        {
            if (menu == null)
                return;

            Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);

            Detail = new NavigationPage(displayPage);

            _menuPage.Menu.SelectedItem = null;
            IsPresented = false;
        }
    }
}