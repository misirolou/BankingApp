using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Models
{
    public class OrderTransaction
    {
        private string toname;
        private string date;
        private double amount;
        private double newbalance;

        public string Toname
        {
            get { return Toname; }
            set { this.Toname = value; }
        }

        public string Date
        {
            get { return Date; }
            set { this.Date = value; }
        }

        public double Amount
        {
            get { return Amount; }
            set { this.Amount = value; }
        }

        public double Newbalance
        {
            get { return this.Newbalance; }
            set { this.Newbalance = value; }
        }

        public OrderTransaction(string date, string toname, double amount, double newbalance)
        {
            this.Date = date;
            this.Toname = toname;
            this.Amount = amount;
            this.Newbalance = newbalance;
        }
    }
}
