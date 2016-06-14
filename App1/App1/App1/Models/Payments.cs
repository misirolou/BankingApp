namespace App1.Models
{
    public class Payments
    {
        public class From
        {
            public string bank_id { get; set; }
            public string account_id { get; set; }
        }

        public class To
        {
            public string bank_id { get; set; }
            public string account_id { get; set; }
        }

        public class Value
        {
            public string currency { get; set; }
            public string amount { get; set; }
        }

        public class Body
        {
            public To to { get; set; }
            public Value value { get; set; }
            public string description { get; set; }
        }

        public class Value2
        {
            public string currency { get; set; }
            public string amount { get; set; }
        }

        public class Charge
        {
            public string summary { get; set; }
            public Value2 value { get; set; }
        }

        public class Payment
        {
            public string id { get; set; }
            public string type { get; set; }
            public From from { get; set; }
            public Body body { get; set; }
            public string transaction_ids { get; set; }
            public string status { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public object challenge { get; set; }
            public Charge charge { get; set; }
        }
    }
}