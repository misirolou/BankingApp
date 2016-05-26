using System.ComponentModel;
using Xamarin.Forms;

namespace App1.Cell
{
    internal class AccountCell : ViewCell
    {
        private Label idLabel, ownerLabel, ibanLabel, balanceLabel, bankLabel, currencyLabel, typeLabel;

        public static readonly BindableProperty IDProperty =
            BindableProperty.Create("id", typeof(string), typeof(AccountCell), "");

        public string id
        {
            get { return (string)GetValue(IDProperty); }
            set { SetValue(IDProperty, value); }
        }

        public static readonly BindableProperty OwnerProperty =
            BindableProperty.Create("owner", typeof(string), typeof(AccountCell), "");

        public string owner
        {
            get { return (string)GetValue(OwnerProperty); }
            set { SetValue(OwnerProperty, value); }
        }

        public static readonly BindableProperty IbanProperty =
            BindableProperty.Create("iban", typeof(string), typeof(AccountCell), "");

        public string iban
        {
            get { return (string)GetValue(IbanProperty); }
            set { SetValue(IbanProperty, value); }
        }

        public static readonly BindableProperty BalanceProperty =
            BindableProperty.Create("balance", typeof(string), typeof(AccountCell), "");

        public string balance
        {
            get { return (string)GetValue(BalanceProperty); }
            set { SetValue(BalanceProperty, value); }
        }

        public static readonly BindableProperty BankProperty =
            BindableProperty.Create("bank", typeof(string), typeof(AccountCell), "");

        public string bank
        {
            get { return (string)GetValue(BankProperty); }
            set { SetValue(BankProperty, value); }
        }

        public static readonly BindableProperty CurrencyProperty =
            BindableProperty.Create("currency", typeof(string), typeof(AccountCell), "");

        public string currency
        {
            get { return (string)GetValue(CurrencyProperty); }
            set { SetValue(CurrencyProperty, value); }
        }

        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create("type", typeof(string), typeof(AccountCell), "");

        public string type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                idLabel.Text = id;
                ownerLabel.Text = owner;
                ibanLabel.Text = iban;
                balanceLabel.Text = balance;
                bankLabel.Text = bank;
                currencyLabel.Text = currency;
                typeLabel.Text = type;
            }
        }
    }
}