using System.Collections.Generic;
using Xamarin.Forms;

namespace App1.Models
{
    internal class Transactions
    {
        public class Holder
        {
            public string name { get; set; }
            public bool is_alias { get; set; }
        }

        public class Bank
        {
            public string national_identifier { get; set; }
            public string name { get; set; }
        }

        public class Account
        {
            public string id { get; set; }
            public List<Holder> holders { get; set; }
            public string number { get; set; }
            public object kind { get; set; }
            public object IBAN { get; set; }
            public object swift_bic { get; set; }
            public Bank bank { get; set; }
        }

        public class Holder2
        {
            public string name { get; set; }
        }

        public class Bank2
        {
            public string national_identifier { get; set; }
            public string name { get; set; }
        }

        public class Counterparty
        {
            public string id { get; set; }
            public Holder2 holder { get; set; }
            public string number { get; set; }
            public object kind { get; set; }
            public object IBAN { get; set; }
            public object swift_bic { get; set; }
            public Bank2 bank { get; set; }
        }

        public class NewBalance
        {
            public string currency { get; set; }
            public string amount { get; set; }
        }

        public class Value
        {
            public string currency { get; set; }
            public string amount { get; set; }
        }

        public class Details
        {
            public string type { get; set; }
            public string description { get; set; }
            public string posted { get; set; }
            public string completed { get; set; }
            public NewBalance new_balance { get; set; }
            public Value value { get; set; }
        }

        public class Transaction
        {
            public string id { get; set; }
            public Account account { get; set; }
            public Counterparty counterparty { get; set; }
            public Details details { get; set; }
        }

        public class TransactionList
        {
            public List<Transaction> transactions { get; set; }
        }
    }
}