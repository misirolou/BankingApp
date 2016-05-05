using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            restClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("DirectLogin", authHeaderValue);
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
    }
}