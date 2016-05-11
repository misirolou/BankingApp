namespace App1.Models
{
    internal class Banks
    {
        public const string BankUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/banks";

        public const string BranchesUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/banks/{0}/branches";

        public const string ATMsUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/banks/{0}/atms";

        public const string ProductsUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/banks/obp-bank-x-g/products";

        public string BankId { get; set; }

        public string BankfullName { get; set; }

        public string BankshortName { get; set; }

        public string Bankwebsite { get; set; }
    }
}