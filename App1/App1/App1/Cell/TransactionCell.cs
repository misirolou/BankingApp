using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Cell
{
    public class TransactionCell : ViewCell
    {
        public TransactionCell()
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            
            var idLabelholder = new Label { FontAttributes = FontAttributes.Bold , BackgroundColor = Color.White};
            var dateofCompletionLabel = new Label() { BackgroundColor = Color.White };
            var amountLabel = new Label {  BackgroundColor = Color.White };
            var balanceLabel = new Label() { BackgroundColor = Color.White };

            //account id of entity that did the transaction counterparty
            idLabelholder.SetBinding(Label.TextProperty, "account.id");
            //date that the transaction was verified and completed
            dateofCompletionLabel.SetBinding(Label.TextProperty, "details.completed");
            //amount that was used in the transaction
            amountLabel.SetBinding(Label.TextProperty, "details.value.amount");
            //balance of the users account
            balanceLabel.SetBinding(Label.TextProperty, "details.new_balance.amount");

            grid.BackgroundColor = Color.Gray;
            grid.ColumnSpacing = 2;
            grid.RowSpacing = 2;
            //used a grid to display information of the transactions
            grid.Children.Add(idLabelholder, 1, 0);
            
            Grid.SetColumnSpan(idLabelholder, 4);
            grid.Children.Add(dateofCompletionLabel, 0, 1);
            grid.Children.Add(amountLabel, 1, 1);
            grid.Children.Add(balanceLabel, 2, 1);

            View = grid;
        }

    }
}
