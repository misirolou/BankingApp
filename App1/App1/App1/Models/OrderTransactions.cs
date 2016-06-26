namespace App1.Models
{
    public class OrderTransactions
    {
        private string date;
        private string amount;
        private string newbalance;

        public string Date
        {
            get { return Date; }
            set { this.Date = value; }
        }

        public string Amount
        {
            get { return Amount; }
            set { this.Amount = value; }
        }

        public string Newbalance
        {
            get { return this.Newbalance; }
            set { this.Newbalance = value; }
        }

        public OrderTransactions(string date, string amount, string newbalance)
        {
            this.Date = date;
            this.Amount = amount;
            this.Newbalance = newbalance;
        }
    }
}