using App1.Models;
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

        public async Task<string> GetData(string id)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync(OAuth.OpenBankAPI);
                return await result.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> RegisterUserJsonRequest(Users user)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                var result = await client.PostAsync(OAuth.OpenBankAPI, content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> RegisterUserFormRequest(Users user)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                 new KeyValuePair<string, string>("Username", user.User),
                 new KeyValuePair<string, string>("Password", user.Password),
             });
                var result = await client.PostAsync(OAuth.OpenBankAPI, content);
                return await result.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> NewSession()
        {
            string responseFromServer = "";
            using (var client = new HttpClient())
            {
                //pedido ao servidor pelo request token
                WebRequest request = (HttpWebRequest)WebRequest.Create(OAuth.OpenBankAPI);
                request.ContentType = "application/json";
                request.Method = "POST";
                var authdata =
                    string.Format(
                        "username= \"danielfaria921@gmail.com\",password= \"Bankingdont243**\",consumer_key=\"ewufgucqycz2lgeifodknycmgsz40t5xb1kqjtjd\"");
                request.Headers["Authorizaton"] = "DirectLogin" + authdata;
                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        try
                        {
                            JsonValue jsondoc = await Task.Run(() => JsonObject.Load(dataStream));
                            Debug.WriteLine("Response {0}", jsondoc.ToString());
                            responseFromServer = jsondoc;
                            return jsondoc;
                        }
                        catch (WebException err)
                        {
                            Stream stream = err.Response.GetResponseStream();
                            StreamReader reader = new StreamReader(stream);
                            responseFromServer = reader.ReadToEnd();
                        }
                    }
                }
            }
            return responseFromServer;
        }

        public async Task<JsonValue> UserInContactPage()
        {
            var request2 = (HttpWebRequest)WebRequest.Create(Banks.BankUrl);
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

        public Task<List<AccountInfo>> RefreshDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveInfoAsync(AccountInfo item, bool isNewItem)
        {
            throw new NotImplementedException();
        }

        public Task DeleteInfoAsync(string id)
        {
            throw new NotImplementedException();
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