using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace App1.REST
{
    class RESTService : IRESTService
    {
        HttpClient restClient;

       public List<AccountInfo> Info { get; private set; } 

        public RESTService()
        {
            var authData = string.Format("{0}:{1}", OAuth.Username, OAuth.Password);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            restClient = new HttpClient();
            restClient.MaxResponseContentBufferSize = 256000;
            restClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        public async Task<List<AccountInfo>> RefreshDataAsync()
        {
            Info = new List<AccountInfo> ();

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
            var uri = new Uri(string.Format(OAuth.OpenBankAPI, Info.AccountId));

            try
            {
                var json = JsonConvert.SerializeObject(Info);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    response = await restClient.PostAsync(uri, content);
                }
                else
                {
                    response = await restClient.PutAsync(uri, content);
                }

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"				TodoItem successfully saved.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
        }

        public async Task DeleteInfoAsync(string id)
        {
            var uri = new Uri(string.Format(OAuth.OpenBankAPI, id));

            try
            {
                var response = await restClient.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"				TodoItem successfully deleted.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
        }
    }
}
