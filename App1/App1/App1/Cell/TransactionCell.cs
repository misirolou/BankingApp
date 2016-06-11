﻿using Xamarin.Forms;

namespace App1.Cell
{
    public class TransactionCell : ViewCell
    {
        public TransactionCell()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var emptylabel = new Label() { BackgroundColor = Color.Teal };
            var idLabelholder = new Label { Text = "Counterparty: ", FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.Center };
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

            grid.BackgroundColor = Color.Gray;
            grid.ColumnSpacing = 2;
            grid.RowSpacing = 5;
            //used a grid to display information of the transactions
            grid.Children.Add(idLabelholder, 0, 1);
            Grid.SetColumnSpan(idLabelholder, 3);
            grid.Children.Add(dateofCompletionLabel, 0, 2);
            grid.Children.Add(amountLabel, 1, 2);
            grid.Children.Add(balanceLabel, 2, 2);
            grid.Children.Add(emptylabel, 0, 3);
            Grid.SetColumnSpan(emptylabel, 3);

            View = grid;
        }
    }
}