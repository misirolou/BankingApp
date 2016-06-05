using Xamarin.Forms;

namespace App1.Cell
{
    internal class AccountCell : ViewCell
    {
        public AccountCell()
        {
            //Id labels identification and layout
            Label IdLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            IdLabel.SetBinding(Label.TextProperty, "id");

            //Id labels identification and layout
            Label ownerLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            ownerLabel.SetBinding(Label.TextProperty, "display_name");

            //Id labels identification and layout
            Label ibanLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            ibanLabel.SetBinding(Label.TextProperty, "IBAN");

            //Id labels identification and layout
            Label balanceLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            balanceLabel.SetBinding(Label.TextProperty, "balance");

            //Id labels identification and layout
            Label bankLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            bankLabel.SetBinding(Label.TextProperty, "bank_id");

            //Id labels identification and layout
            Label currencyLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            currencyLabel.SetBinding(Label.TextProperty, "currency");

            
            //Id labels identification and layout
            Label typeLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            //Binding of the id label used to switch between different ids
            typeLabel.SetBinding(Label.TextProperty, "type");

            //this is the actual layout of each of the cells
            var nameLayout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { IdLabel, ownerLabel, balanceLabel, bankLabel, ibanLabel, currencyLabel, typeLabel  }
            };
            View = nameLayout;
        }
    }
}