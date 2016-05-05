namespace App1.REST
{
    public class AccountInfo
    {
        public string AccountInfoUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/accounts";

        public string Owner { get; set; }

        public string AccountId { get; set; }

        public string LastAccess { get; set; }

        public string Iban { get; set; }

        public string Balance { get; set; }

        public string Bank { get; set; }

        public string Currency { get; set; }

        public string Typeaccount { get; set; }
    }
}