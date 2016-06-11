using App1.Models;
using Xamarin.Forms;

namespace App1.Cell
{
    internal class TransactionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ValidTemplate { get; set; }

        public DataTemplate InvalidTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            int i = 0;
            return int.Parse(((Transactions.Details)item).value.amount) >= i ? ValidTemplate : InvalidTemplate;
        }
    }
}