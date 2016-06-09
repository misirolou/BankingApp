using App1.Layout;
using System.Collections.Generic;

namespace App1.Menu
{
    public class MenuListData : List<MenuItem>
    {
        public MenuListData()
        {
            //the main page to be selected
            this.Add(new MenuItem
            {
                Title = "Accounts",
                TargetType = typeof(AccountsPage)
            });

            //the transaction page to be selected
            this.Add(new MenuItem()
            {
                Title = "Transactions",
                TargetType = typeof(transactionPage)
            });

            //the card page to be selected
            this.Add(new MenuItem()
            {
                Title = "Cards",
                TargetType = typeof(cardPage)
            });

            //the products page to be selected
            this.Add(new MenuItem()
            {
                Title = "Products",
                TargetType = typeof(ProductPage)
            });

            //the contact page to be selected
            this.Add(new MenuItem()
            {
                Title = "Contacts",
                TargetType = typeof(ContactPage)
            });

            //the branches page to be selected
            this.Add(new MenuItem()
            {
                Title = "Branches",
                TargetType = typeof(BalcaoPage)
            });

            //the atm page to be selected
            this.Add(new MenuItem()
            {
                Title = "ATMs",
                TargetType = typeof(AtmPage)
            });
        }
    }
}