namespace App1.Models
{
    internal class Banks
    {
        public const string BankUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/banks{0}";

        public string BankId { get; set; }

        public string BankfullName { get; set; }

        public string BankshortName { get; set; }

        public string Bankwebsite { get; set; }
    }
}