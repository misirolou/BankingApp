using Xamarin.Forms;

namespace App1.Cell
{
    //Aspect of each of the products cells
    internal class productCells : ViewCell
    {
        public productCells()
        {
            //Id labels identification and layout
            Label codeLabel = new Label()
            {
                Text = "id: ",
            };
            //Binding of the id label used to switch between different ids
            codeLabel.SetBinding(Label.TextProperty, "code");
            //shortnames labels identification and layout
            Label nameLabel = new Label()
            {
            };
            nameLabel.SetBinding(Label.TextProperty, "name");
            //fullnames labels identification and layout
            Label categoryLabel = new Label()
            {
                Text = "Fullname: ",
            };
            categoryLabel.SetBinding(Label.TextProperty, "category");
            //logo labels identification and layout
            Label familyLabel = new Label()
            {
            };
            familyLabel.SetBinding(Label.TextProperty, "family");
            //website labels identification and layout
            Label super_familyLabel = new Label()
            {
                Text = "Website: ",
            };
            super_familyLabel.SetBinding(Label.TextProperty, "super_family");
            //empty label used to create seperations between each view
            var emptylabel = new Label() { BackgroundColor = Color.Teal };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.BackgroundColor = Color.Gray;
            grid.ColumnSpacing = 2;
            grid.RowSpacing = 5;
            //used a grid to display information of the transactions
            grid.Children.Add(codeLabel, 0, 0);
            Grid.SetColumnSpan(codeLabel, 2);
            grid.Children.Add(nameLabel, 0, 1);
            Grid.SetColumnSpan(nameLabel, 2);
            grid.Children.Add(familyLabel, 0, 2);
            grid.Children.Add(super_familyLabel, 1, 2);
            grid.Children.Add(emptylabel, 0, 3);
            Grid.SetColumnSpan(emptylabel, 2);

            View = grid;
        }
    }
}