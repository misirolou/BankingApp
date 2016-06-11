using Xamarin.Forms;

namespace App1.Cell
{
    internal class AllAccountsCell : ViewCell
    {
        public AllAccountsCell()
        {
            //Id labels identification and layout
            Label IdLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.Gray
            };
            //Binding of the id label used to switch between different ids
            IdLabel.SetBinding(Label.TextProperty, "id");

            //used to seperate each view
            Label emptylabel = new Label()
            {
                BackgroundColor = Color.Gray
            };
            //Binding of the id label used to switch between different ids

            //Id labels identification and layout
            Label bankLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.Gray
            };
            //Binding of the id label used to switch between different ids
            bankLabel.SetBinding(Label.TextProperty, "bank_id");

            //Id labels identification and layout
            Label _linksLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            _linksLabel.SetBinding(Label.TextProperty, "_links");

            //Id labels identification and layout
            Label selfLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            selfLabel.SetBinding(Label.TextProperty, "self");
            //Id labels identification and layout
            Label detailLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            detailLabel.SetBinding(Label.TextProperty, "detail");

            //Id labels identification and layout
            Label hrefLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            hrefLabel.SetBinding(Label.TextProperty, "href");

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.BackgroundColor = Color.Gray;
            grid.ColumnSpacing = 2;
            grid.RowSpacing = 5;
            //used a grid to display information of the transactions
            grid.Children.Add(IdLabel, 0, 0);
            grid.Children.Add(bankLabel, 1, 0);
            grid.Children.Add(emptylabel, 0, 1);
            Grid.SetColumnSpan(emptylabel, 2);

            //each grid created is contained inside a Viewcell
            View = grid;
        }
    }
}