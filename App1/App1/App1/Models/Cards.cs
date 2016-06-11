using System.Collections.Generic;

namespace App1.Models
{
    internal class Cards
    {
        public class Replacement
        {
            public string requested_date { get; set; }
            public string reason_requested { get; set; }
        }

        public class PinReset
        {
            public string requested_date { get; set; }
            public string reason_requested { get; set; }
        }

        public class Collected
        {
            public string date { get; set; }
        }

        public class Posted
        {
            public string date { get; set; }
        }

        public class Card
        {
            public string bank_card_number { get; set; }
            public string name_on_card { get; set; }
            public string issue_number { get; set; }
            public string serial_number { get; set; }
            public string valid_from_date { get; set; }
            public string expires_date { get; set; }
            public bool enabled { get; set; }
            public bool cancelled { get; set; }
            public bool on_hot_list { get; set; }
            public string technology { get; set; }
            public List<string> networks { get; set; }
            public List<string> allows { get; set; }
            public string account { get; set; }
            public Replacement replacement { get; set; }
            public List<PinReset> pin_reset { get; set; }
            public Collected collected { get; set; }
            public Posted posted { get; set; }
        }

        public class Cardlist
        {
            public List<Card> cards { get; set; }
        }

    }
}