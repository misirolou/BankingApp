using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Models
{
    class Account
    {
        //"https://apisandbox.openbankproject.com/obp/v2.0.0/my/accounts" the login account
        public class Self
        {
            public string href { get; set; }
        }

        //"href": "https://apisandbox.openbankproject.com/obp/v2.0.0/my/banks/bank_id/accounts/account_id/account" detailed info about account
        //this request is made already by the Self link
        public class Detail
        {
            public string href { get; set; }
        }

        public class Links
        {
            public Self self { get; set; }
            public Detail detail { get; set; }
        }


        public class Accounts
        {
            public string id { get; set; }
            public object label { get; set; }
            public string bank_id { get; set; }
            public Links _links { get; set; }
        }

    }
}
