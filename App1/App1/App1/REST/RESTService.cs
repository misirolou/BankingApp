using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using App1.Models;

namespace App1.REST
{
    public class RESTService : IRESTService
    {
        private HttpClient restClient;

        public List<AccountInfo> Info { get; private set; }

        /*  204 (NO CONTENT) – the request has been successfully processed and the response is intentionally blank.
        400 (BAD REQUEST) – the request is not understood by the server.
        404 (NOT FOUND) – the requested resource does not exist on the server.*/

        //login into the API with my account Direct Login
        public RESTService()
        {
            var users = new Users();
            users.User = OAuth.Username;
            users.Password = OAuth.Password;
            Debug.WriteLine("in RestService");
            //authorization header used to send information to receive token
            var authData = string.Format("{0}:{1}:{2}", OAuth.Username, OAuth.Password, OAuth.oauth_consumer_key);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            Debug.WriteLine("RestService authHeadrer: " + authHeaderValue);

            restClient = new HttpClient();
            restClient.MaxResponseContentBufferSize = 256000;
            restClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("DirectLogin",
                authHeaderValue);
        }

        //Refreshing information
        public async Task<List<AccountInfo>> RefreshDataAsync()
        {
            Info = new List<AccountInfo>();

            var uri = new Uri(string.Format(OAuth.OpenBankAPI, string.Empty));

            try
            {
                var response = await restClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Info = JsonConvert.DeserializeObject<List<AccountInfo>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return Info;
        }

        public async Task SaveInfoAsync(AccountInfo Info, bool isNewItem = false)
        {
            var uri = new Uri(string.Format(OAuth.OpenBankAPI, Info.token));

            try
            {
                var json = JsonConvert.SerializeObject(Info);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    //Creates the new data fetched from the API
                    response = await restClient.PostAsync(uri, content);
                }
                else
                {
                    //Update data from the API
                    response = await restClient.PutAsync(uri, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"				Token successfully saved.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
        }

        public async Task DeleteInfoAsync(string token)
        {
            var uri = new Uri(string.Format(OAuth.OpenBankAPI, token));

            try
            {
                var response = await restClient.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"				token successfully deleted.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
        }

        public async Task<JsonValue> NewSession()
        {

          //  string responseFromServer = "";
            //pedido ao servidor pelo request token
            WebRequest request = (HttpWebRequest)WebRequest.Create(OAuth.OpenBankAPI);
            request.ContentType = "application/json";
            request.Method = "POST";
            // request.Headers.Add("Authorization", "OAuth " + authorizationstring);
           // var authData = string.Format("username={0},password={1},consumer_key={2}", OAuth.Username, OAuth.Password, OAuth.oauth_consumer_key);
            var authdata = string.Format("username= \"danielfaria921@gmail.com\",password= \"Bankingdont243**\",consumer_key=\"ewufgucqycz2lgeifodknycmgsz40t5xb1kqjtjd\"");
            request.Headers["Authorizaton"] = "DirectLogin" + authdata;
            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream dataStream = response.GetResponseStream())
                {
                    JsonValue jsondoc = await Task.Run(() => JsonObject.Load(dataStream));
                    Debug.WriteLine("Response {0}", jsondoc.ToString());
                    return jsondoc;
                }
            }
        }



        public async Task<JsonValue> UserInContactPage()
        {
            var request2 = (HttpWebRequest) WebRequest.Create(Banks.BankUrl);
            request2.ContentType = "application/json";
            request2.Method = "GET";

            using (HttpWebResponse response = await request2.GetResponseAsync() as HttpWebResponse)
            {
                if (response.StatusCode != HttpStatusCode.OK)
                    Debug.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        Debug.WriteLine("Response contained empty body...");
                    }
                    else
                    {
                        Debug.WriteLine("Response Body: \r\n {0}", content);
                    }
                    return content;
                }
            }

          /*  //pedido ao servidor pelo request token
            WebRequest request = (HttpWebRequest) WebRequest.Create(Banks.BankUrl);
            request.Method = "GET";
            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream dataStream = response.GetResponseStream())
                {
                    JsonValue jsondoc = await Task.Run(() => JsonValue.Load(dataStream));
                    Debug.WriteLine("Response: {0}", jsondoc);
                    return jsondoc;
                }
            }*/
        }

        public async Task<string> CreateSession(string user, string pass)
        {
            using (var client = new HttpClient())
            {
            //Url link to OpenBankAPI
                      var url = string.Format(OAuth.OpenBankAPI);
                      //Converting to JSON
                      var json = JsonConvert.SerializeObject(user);

                      //headers data that goes with the information given
                      var authData = string.Format("{0}:{1}:{2}", OAuth.Username, OAuth.Password, OAuth.oauth_consumer_key);
                      var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

                      Debug.WriteLine("RestService authHeader: " + authHeaderValue);

                      HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                      client.DefaultRequestHeaders.Add("DirectLogin", authHeaderValue);

                      //defining post method
                      var resp = await client.PostAsync(url, content);

                      if (resp.IsSuccessStatusCode)
                      {
                          var result = JsonConvert.DeserializeObject<Users>(resp.Content.ReadAsStringAsync().Result);
                          var Token = result.token;
                          Debug.WriteLine(Token);
                          return Token;
                      }
                  }
                  return null;
              }
        }
}