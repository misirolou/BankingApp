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

            //Id labels identification and layout
            Label label = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.Gray
            };
            //Binding of the id label used to switch between different ids
            label.SetBinding(Label.TextProperty, "label");

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

            StackLayout stack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.Gray,
                Children = { IdLabel, bankLabel }
            };

            //this is the actual layout of each of the cells
            var nameLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Padding = 5,
                Margin = 10,
                BackgroundColor = Color.Gray,
                Children = {stack, label}
            };
            View = nameLayout;
        }
    }
}