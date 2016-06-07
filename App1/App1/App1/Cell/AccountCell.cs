using Xamarin.Forms;

namespace App1.Cell
{
    internal class AccountCell : ViewCell
    {
        public AccountCell()
        {
            //Id labels identification and layout
            Label providerLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            providerLabel.SetBinding(Label.TextProperty, "provider");

            //Id labels identification and layout
            Label idLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            idLabel.SetBinding(Label.TextProperty, "id");

            //Id labels identification and layout
            Label ownerLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            ownerLabel.SetBinding(Label.TextProperty, "display_name");
            //this is the actual layout of each of the cells
            var AccountInfoLayout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { ownerLabel }
            };
            View = AccountInfoLayout;
        }
    }
}