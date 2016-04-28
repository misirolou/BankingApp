using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.REST
{
    //this class shall be used to get and set users inicial information from the API REST OpenBank
    class Users
    {
        public string User { get; set; }

        public string Password { get; set; }

        public string Bank { get; set; }
    }
}
