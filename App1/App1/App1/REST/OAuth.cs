using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Auth;

namespace App1.REST
{
    class OAuth
    {
        // URL of REST service
        public static string OpenBankAPI = "https://apisandbox.openbankproject.com/obp/v2.0.0{0}";
        // Credentials that are hard coded into the REST service
        public static string Username = "danielfaria921@gmail.com";
        public static string Password = "Bankingdont243**";

        private void LogintoOpenBank(bool allowCancel)
        {
            var auth = new OAuth1Authenticator(
                consumerKey: "ewufgucqycz2lgeifodknycmgsz40t5xb1kqjtjd",
                consumerSecret: "4hsiqdzrs3iszz2jzjzlrit0yd4cv4c10w0hpjhj",
                requestTokenUrl: new Uri(""),
                authorizeUrl: new Uri("https://apisandbox.openbankproject.com/dialog/oauth/"),
                accessTokenUrl: new Uri(""),
                callbackUrl: new Uri(""),

                getUsernameAsync: null);

            auth.AllowCancel = allowCancel;
            // If authorization succeeds or is canceled, .Completed will be fired.
           /*   auth.Completed += (sender, eventArgs) =>
              {
                  DismissViewController(true, null);
                  if (eventArgs.IsAuthenticated)
                  {
                      // Use eventArgs.Account to do wonderful things
                  }
              }

              private PresentViewController(auth.GetUI(), true, null);*/

        }

        //obtaining OAuth token and secret
        private String requestToken = "";
        private String requestTokenSecret = "";

        //OAuth timestamp
        private static TimeSpan TS = DateTime.UtcNow - new DateTime();
        private String timestamp = Convert.ToUInt64(TS.TotalSeconds).ToString();

        //Oauth Nonce used only one time
        string nonce = new Random().Next(1230000, 9999999).ToString();

        //parameters to include in the request for the tokens
        System.Collections.Generic.List<string> parameters = new System.Collections.Generic.List<string>();
        

    }
}
