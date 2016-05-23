using App1.Layout;
using System;
using Xamarin.Forms;

namespace App1.Menu
{
    public class rootpage : MasterDetailPage
    {
        private MenuPage menuPage;

        //prepare the menu page
        public rootpage()
        {
            menuPage = new MenuPage();

            menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as MenuItem);

            Master = menuPage;
        }

        //Navigate to the page selected in the menu
        private void NavigateTo(MenuItem menu)
        {
            if (menu == null)
                return;

            Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);

            Detail = new NavigationPage(displayPage);

            menuPage.Menu.SelectedItem = null;
            IsPresented = false;
        }
    }
}