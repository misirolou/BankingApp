using System.Diagnostics;
using Xamarin.Forms;

namespace App1.Cell
{
    public class TransactionCell : ViewCell
    {
        //Transactions cells used to determine the layout of each of the transaction received from the list view
        public TransactionCell()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var emptylabel = new Label() { BackgroundColor = Color.Teal };
            var idLabelholder = new Label { FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.Center };
            var dateofCompletionLabel = new Label() { HorizontalTextAlignment = TextAlignment.Center };
            var amountLabel = new Label() { HorizontalTextAlignment = TextAlignment.Center };
            var balanceLabel = new Label() { HorizontalTextAlignment = TextAlignment.Center };

            //account id of entity that did the transaction counterparty
            idLabelholder.SetBinding(Label.TextProperty, "counterparty.id");
            //date that the transaction was verified and completed
            dateofCompletionLabel.SetBinding(Label.TextProperty, "details.completed");
            //amount that was used in the transaction
            amountLabel.SetBinding(Label.TextProperty, "details.value.amount");
            //balance of the users account
            balanceLabel.SetBinding(Label.TextProperty, "details.new_balance.amount");

            Debug.WriteLine("Should have added to the Transaction page");

            grid.BackgroundColor = Color.Gray;
            grid.ColumnSpacing = 5;
            grid.RowSpacing = 5;
            //used a grid to display information of the transactions
            grid.Children.Add(dateofCompletionLabel, 0, 0);
            grid.Children.Add(amountLabel, 1, 0);
            grid.Children.Add(balanceLabel, 2, 0);
            grid.Children.Add(emptylabel, 0, 1);
            Grid.SetColumnSpan(emptylabel, 3);

            View = grid;
        }
    }
}