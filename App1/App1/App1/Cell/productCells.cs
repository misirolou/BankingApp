using Xamarin.Forms;

namespace App1.Cell
{
    internal class productCells : ViewCell
    {
        public string code { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string family { get; set; }
        public string super_family { get; set; }
        public string more_info_url { get; set; }

        public productCells()
        {
            //Id labels identification and layout
            Label codeLabel = new Label()
            {
                Text = "id: ",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            codeLabel.SetBinding(Label.TextProperty, "code");
            //shortnames labels identification and layout
            Label nameLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            nameLabel.SetBinding(Label.TextProperty, "name");
            //fullnames labels identification and layout
            Label categoryLabel = new Label()
            {
                Text = "Fullname: ",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center
            };
            categoryLabel.SetBinding(Label.TextProperty, "category");
            //logo labels identification and layout
            Label familyLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center
            };
            familyLabel.SetBinding(Label.TextProperty, "family");
            //website labels identification and layout
            Label super_familyLabel = new Label()
            {
                Text = "Website: ",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center
            };
            super_familyLabel.SetBinding(Label.TextProperty, "super_family");
            //empty label used to create seperations between each view
            var emptylabel = new Label() { BackgroundColor = Color.Teal };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

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