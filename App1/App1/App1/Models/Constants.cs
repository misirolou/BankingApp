using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Models
{
    public class Constants
    {
        // URL of REST service
        public const string OpenBankAPI = "https://apisandbox.openbankproject.com/my/logins/direct";

        // Credentials that are used to login through the REST service for Direct Login these credentials aren´t used in the code in any way 
        public static string Username = "danielfaria921@gmail.com";

        public static string Password = "Bankingdont243**";

        // oauth_consumer_key if you want to try another account this has to be changed accordingly for this is used to verify the users account info through OpenBanks Direct Login
        //this static string is used allowing only my account to be used to login through the app
        public static string oauth_consumer_key = "ewufgucqycz2lgeifodknycmgsz40t5xb1kqjtjd";

        // Use your own oauth_consumer_secret
        protected static string oauth_consumer_secret = "4hsiqdzrs3iszz2jzjzlrit0yd4cv4c10w0hpjhj";

        //Contact information of the banks
        public const string BankUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/banks";

        //Used for map locations where the user can choose the branch or ATM they want to check out  {0} = Bank ID
        public const string BranchesUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/banks/{0}/branches";

        public const string ATMsUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/banks/{0}/atms";

        //products available from the banks
        public const string ProductsUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/banks/{0}/products";

        //This account contains the id´s that will be used while the user is logged in
        public const string AccountUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/my/accounts";

        //Accounts of the user that logged in with more details {0} = Bank ID ; {1} = Account ID 
        public const string AccountDetailedUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/my/banks/{0}/accounts/{1}/account";
        
        //Movements or transactions made by the logged in user {0} = Bank ID ; {1} = Account ID 
        public const string MovementUrl = "https://apisandbox.openbankproject.com/obp/v2.0.0/my/banks/{0}/accounts/{1}/transactions";
    }
}
