using Portable.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Security;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xamarin.Utilities;
using Encoding = System.Text.Encoding;

namespace App1.REST
{
    internal class OAuth
    {
        // URL of REST service
        public static string OpenBankAPI = "https://apisandbox.openbankproject.com/obp/v2.0.0{0}";

        // Credentials that are hard coded into the REST service for Direct Login
        public static string Username = "danielfaria921@gmail.com";

        public static string Password = "Bankingdont243**";

        // oauth_consumer_key
        protected static string oauth_consumer_key = "ewufgucqycz2lgeifodknycmgsz40t5xb1kqjtjd";

        // Use your own oauth_consumer_secret
        protected static string oauth_consumer_secret = "4hsiqdzrs3iszz2jzjzlrit0yd4cv4c10w0hpjhj";

        // version of oauth used
        protected const string oauth_version = "1.0";

        // Encryption method used
        protected const string oauth_signature_method = "HMAC-SHA1";

        protected static string oauth_token = "";
        protected static string oauth_token_secret = "";

        //Request token needed to verify users authentication
        private static async void RequestToken(string[] args)
        {
            // callback to the app
            string oauth_callback = "oob";
            // oauth_nonce used only once
            string oauth_nonce = new Random().Next(123400, 9999999).ToString();
            // this is left blank for now
            string oauth_signature = "";
            // timestamp used when making your token
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string oauth_timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();

            //Tokens needed to verify users authentication access
            string oauth_request_token = "";
            string oauth_request_token_secret = "";

            //method used and request token url
            string method = "POST";
            string RequestTokenUri = "https://apisandbox.openbankproject.com/oauth/initiate";

            // Create a list of OAuth parameters
            List<KeyValuePair<string, string>> oauthparameters = new List<KeyValuePair<string, string>>();
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_callback", oauth_callback));
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_consumer_key", oauth_consumer_key));
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_nonce", oauth_nonce));
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_signature_method", oauth_signature_method));
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_timestamp", oauth_timestamp));
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_version", oauth_version));

            // Sort the OAuth parameters on the key
            oauthparameters.Sort((x, y) => x.Key.CompareTo(y.Key));

            // Construct the Base String
            string basestring = method.ToUpper() + "&" + WebUtility.UrlEncode(RequestTokenUri) + "&";
            foreach (KeyValuePair<string, string> pair in oauthparameters)
            {
                basestring += pair.Key + "%3D" + WebUtility.UrlEncode(pair.Value) + "%26";
            }
            basestring = basestring.Substring(0, basestring.Length - 3);
            //Gets rid of the last "%26" added by the foreach loop
            // Makes sure all the Url encoding gives capital letter hexadecimal
            // i.e. "=" is encoded to "%3D", not "%3d"

            //basestring used to get the signature
            char[] basestringchararray = basestring.ToCharArray();
            for (int i = 0; i < basestringchararray.Length - 2; i++)
            {
                if (basestringchararray[i] == '%')
                {
                    basestringchararray[i + 1] = char.ToUpper(basestringchararray[i + 1]);
                    basestringchararray[i + 2] = char.ToUpper(basestringchararray[i + 2]);
                }
            }
            basestring = new string(basestringchararray);

            // Encrypt with SHA1 creating the Signature
            var enc = ASCIIEncoding.ASCII;

            /* create the crypto class we use to generate a signature for the request */
            HMACSHA1 hmac = new HMACSHA1(enc.GetBytes(oauth_consumer_secret + "&"));
            //hmac.intialize();
            byte[] buffer = enc.GetBytes(basestring);
            string hmacsha1 = BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower();
            byte[] resultantArray = new byte[hmacsha1.Length / 2];
            for (int i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = Convert.ToByte(hmacsha1.Substring(i * 2, 2), 16);
            }
            string base64 = Convert.ToBase64String(resultantArray);
            oauth_signature = WebUtility.UrlEncode(base64);

            // Create the Authorization string for the WebRequest header
            string authorizationstring = "";
            foreach (KeyValuePair<string, string> pair in oauthparameters)
            {
                authorizationstring += pair.Key;
                authorizationstring += "=";
                authorizationstring += pair.Value;
                authorizationstring += ",";
            }
            authorizationstring += "oauth_signature=" + oauth_signature;

            string responseFromServer = "";
            //pedido ao servidor pelo request token
            WebRequest request = (HttpWebRequest)WebRequest.Create(RequestTokenUri);
            request.Method = method;
            // request.Headers.Add("Authorization", "OAuth " + authorizationstring);
            request.Headers["Authorization"] = "OAuth " + authorizationstring;
            var asyncResult = request.BeginGetResponse(
                ar =>
                {
                    using (WebResponse response = request.EndGetResponse(ar))
                        try
                        {
                            Stream dataStream = response.GetResponseStream();
                            StreamReader reader = new StreamReader(dataStream);
                            responseFromServer = reader.ReadToEnd();
                        }
                        catch (WebException err)
                        {
                            Stream dataStream = err.Response.GetResponseStream();
                            StreamReader reader = new StreamReader(dataStream);
                            responseFromServer = reader.ReadToEnd();
                        }
                    
                }, request);
            
            Debug.WriteLine(responseFromServer);
            Debug.WriteLine(asyncResult);

            // if it worked, we should have oauth_token and oauth_token_secret in the response
            foreach (string pair in responseFromServer.Split(new char[] { '&' }))
            {
                string[] split_pair = pair.Split(new char[] { '=' });

                switch (split_pair[0])
                {
                    case "oauth_token":
                        oauth_request_token = split_pair[1];
                        break;

                    case "oauth_token_secret":
                        oauth_request_token_secret = split_pair[1];
                        break;
                }
            }
            Debug.WriteLine(oauth_request_token);
            Debug.WriteLine(oauth_request_token_secret);
        }

        //Receiving the access token needed to do requests from OpenBank API
        private static void AccessToken(string[] args)
        {
            // Use the oauth_verifier in the previous step
            string oauth_verifier = "";
            // Use the oauth_token in the previous step
            string oauth_request_token = "";
            // oauth_nonce used only once
            string oauth_nonce = new Random().Next(123400, 9999999).ToString();
            // Leave this blank for now
            string oauth_signature = "";

            string oauth_signature_method = "HMAC-SHA1";
            // oauth_timestamp used when we received our request token
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string oauth_timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            // oauth_request_token_secret
            string oauth_token_secret = "";

            //tokens we are trying to get, needed to access nformation of the OpenBank API
            string oauth_access_token = "";
            string oauth_access_token_secret = "";

            string method = "POST";
            string AccessTokenUri = "https://apisandbox.openbankproject.com/oauth/token";

            // Create a list of OAuth parameters
            List<KeyValuePair<string, string>> oauthparameters = new List<KeyValuePair<string, string>>();
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_verifier", oauth_verifier));
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_token", oauth_token));
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_consumer_key", oauth_consumer_key));
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_nonce", oauth_nonce));
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_signature_method", oauth_signature_method));
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_timestamp", oauth_timestamp));
            oauthparameters.Add(new KeyValuePair<string, string>("oauth_version", oauth_version));

            // Sort the OAuth parameters on the key
            oauthparameters.Sort((x, y) => x.Key.CompareTo(y.Key));

            // Construct the Base String
            string basestring = method.ToUpper() + "&" + WebUtility.UrlEncode(AccessTokenUri) + "&";
            foreach (KeyValuePair<string, string> pair in oauthparameters)
            {
                basestring += pair.Key + "%3D" + WebUtility.UrlEncode(pair.Value) + "%26";
            }
            basestring = basestring.Substring(0, basestring.Length - 3);
            //Gets rid of the last "%26" added by the foreach loop

            // Makes sure all the Url encoding gives capital letter hexadecimal
            // i.e. "=" is encoded to "%3D", not "%3d"
            char[] basestringchararray = basestring.ToCharArray();
            for (int i = 0; i < basestringchararray.Length - 2; i++)
            {
                if (basestringchararray[i] == '%')
                {
                    basestringchararray[i + 1] = char.ToUpper(basestringchararray[i + 1]);
                    basestringchararray[i + 2] = char.ToUpper(basestringchararray[i + 2]);
                }
            }
            basestring = new string(basestringchararray);

            // Encrypt with either SHA1 or SHA256, creating the Signature
            var enc = Encoding.UTF8;
            HMACSHA1 hmac = new HMACSHA1(enc.GetBytes(oauth_consumer_secret + "&" + oauth_token_secret));
           // hmac.Initialize();
            byte[] buffer = enc.GetBytes(basestring);
            string hmacsha1 = BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower();
            byte[] resultantArray = new byte[hmacsha1.Length / 2];
            for (int i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = Convert.ToByte(hmacsha1.Substring(i * 2, 2), 16);
            }
            string base64 = Convert.ToBase64String(resultantArray);
            oauth_signature = WebUtility.UrlEncode(base64);

            // Create the Authorization string for the WebRequest header
            string authorizationstring = "";
            foreach (KeyValuePair<string, string> pair in oauthparameters)
            {
                authorizationstring += pair.Key;
                authorizationstring += "=";
                authorizationstring += pair.Value;
                authorizationstring += ",";
            }
            authorizationstring += "oauth_signature=" + oauth_signature;

            string responseFromServer = "";
            //pedido ao servidor pelo request token
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(AccessTokenUri);
            request.Method = method;
            // request.Headers.Add("Authorization", "OAuth " + authorizationstring);
            request.Headers["Authorization"] = "OAuth " + authorizationstring;
            var asyncResult = request.BeginGetResponse(
                ar =>
                {
                    using (WebResponse response = request.EndGetResponse(ar))
                        try
                        {
                            Stream dataStream = response.GetResponseStream();
                            StreamReader reader = new StreamReader(dataStream);
                            responseFromServer = reader.ReadToEnd();
                        }
                        catch (WebException err)
                        {
                            Stream dataStream = err.Response.GetResponseStream();
                            StreamReader reader = new StreamReader(dataStream);
                            responseFromServer = reader.ReadToEnd();
                        }
                    
                }, request);
            
            Debug.WriteLine(responseFromServer);
            Debug.WriteLine(asyncResult);

            // if it worked, we should have oauth_token and oauth_token_secret in the response
            foreach (string pair in responseFromServer.Split(new char[] { '&' }))
            {
                string[] split_pair = pair.Split(new char[] { '=' });

                switch (split_pair[0])
                {
                    case "oauth_token":
                        oauth_access_token = split_pair[1];
                        break;

                    case "oauth_token_secret":
                        oauth_access_token_secret = split_pair[1];
                        break;
                }
            }
            Debug.WriteLine(oauth_access_token);
            Debug.WriteLine(oauth_access_token_secret);
        }
    }
}