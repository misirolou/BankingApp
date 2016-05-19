using App1.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace App1.REST
{
    public class RESTService : IRESTService
    {
        private HttpClient restClient;

        public static string token = "";

        public List<Constants> url { get; set; }

        public List<AccountInfo> Info { get; private set; }

        /*  204 (NO CONTENT) – the request has been successfully processed and the response is intentionally blank.
        400 (BAD REQUEST) – the request is not understood by the server.
        404 (NOT FOUND) – the requested resource does not exist on the server.*/

        //This function will be used to GET information for the user that doesn´t require any type of login
        public async Task<string> GetwithoutToken(string url, int choice)
        {
            string responseFromServer = "";
            var uri = string.Format(url);
            var request2 = (HttpWebRequest)WebRequest.Create(uri);
            request2.ContentType = "application/json";
            request2.Method = "GET";

            try
            {
                using (HttpWebResponse response = await request2.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Debug.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                        return null;
                    }
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string content = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            Debug.WriteLine("Response contained empty body...");
                        }
                        else
                        {
                            switch (choice)
                            {
                                case 1:
                                    Debug.WriteLine("Response Body: \r\n {0}", content);
                                    //deserializing string of information received into json type to then be called
                                    var json = new JsonArray(content);
                                    Debug.WriteLine(json);
                                    bankslist info = JsonConvert.DeserializeObject<bankslist>(content);
                                    Debug.WriteLine("json deserilization:  {0} ::-:: {1} ::-:", info, info.banklist);
                                    foreach (var item in info.banklist)
                                    {
                                        Debug.WriteLine("id=={0} .. full_name=={1} .. website=={2}", item.id, item.full_name, item.website);
                                    }
                                    break;

                                case 2:
                                    Debug.WriteLine("Response Body: \r\n {0}", content);
                                    //deserializing string of information received into json type to then be called
                                    var info1 = JsonConvert.DeserializeObject<List<Location>>(content);
                                    Debug.WriteLine("json deserilization:  {0} ::-:: {1} ::-:: {2}", info1, info1.Capacity,
                                        info1.Count);
                                    break;

                                case 3:
                                    Debug.WriteLine("Response Body: \r\n {0}", content);
                                    //deserializing string of information received into json type to then be called
                                    var info2 = JsonConvert.DeserializeObject<List<Location>>(content);
                                    Debug.WriteLine("json deserilization:  {0} ::-:: {1} ::-:: {2}", info2, info2.Capacity,
                                        info2.Count);
                                    break;

                                default:
                                    break;
                            }
                        }
                        return content;
                    }
                }
            }
            catch (WebException err)
            {
                Stream stream = err.Response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                responseFromServer = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(responseFromServer))
                {
                    Debug.WriteLine("Response contained empty body...");
                }
                App.UserLoggedIn = false;
                return responseFromServer;
            }
        }

        //The user should be autheticated and this GET request will be using the token from the function Createsession
        public async Task<string> GetWithToken(string url, int choice)
        {
            string responseFromServer = "";

            var uri = string.Format(url);
            var request2 = (HttpWebRequest)WebRequest.Create(uri);
            request2.ContentType = "application/json";
            request2.Method = "GET";
            request2.Headers[HttpRequestHeader.Authorization] = token;

            try
            {
                using (HttpWebResponse response = await request2.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Debug.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                        return null;
                    }
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
                            //deserializing string of information received into json type to then be called
                            var info = JsonConvert.DeserializeObject<AccountInfo>(content);
                            Debug.WriteLine("json deserilization:  {0} ::-::", info);
                        }
                        return content;
                    }
                }
            }
            catch (WebException err)
            {
                Stream stream = err.Response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                responseFromServer = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(responseFromServer))
                {
                    Debug.WriteLine("Response contained empty body...");
                }
                return responseFromServer;
            }
        }

        //Post of the users attributes of their login information and receiving the token
        public async Task<string> CreateSession(Users user, Users pass)
        {
            string responseFromServer = "";
            //pedido ao servidor pelo request token
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Constants.OpenBankAPI);
            request.ContentType = "application/json";
            request.Method = "POST";
            var authData = string.Format("username=\"{0}\", password=\"{1}\",consumer_key=\"{2}\"", user.User, pass.Password, Constants.oauth_consumer_key);
            var header = request.Headers[HttpRequestHeader.Authorization] = "DirectLogin " + authData;
            try
            {
                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    using (var writer = new StreamWriter(requestStream))
                    {
                        // Send the data to the server
                        Debug.WriteLine(header);
                        writer.Write(header);
                        writer.Flush();
                        writer.Dispose();
                    }
                }
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Debug.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                        return null;
                    }
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        try
                        {
                            StreamReader reader = new StreamReader(dataStream);
                            token = reader.ReadToEnd();
                            if (string.IsNullOrWhiteSpace(responseFromServer))
                            {
                                Debug.WriteLine("Response contained empty body...");
                            }
                            Debug.WriteLine("Response {0}", token);
                            if (string.IsNullOrWhiteSpace(token))
                            {
                                App.UserLoggedIn = false;
                            }
                            else
                            {
                                //deserializing string of information received into json type to then be called
                                var info = JsonConvert.DeserializeObject<Users>(token);
                                Debug.WriteLine("json deserilization:  {0}", info.token);
                                App.UserLoggedIn = true;
                            }
                        }
                        catch (WebException err)
                        {
                            Stream stream = err.Response.GetResponseStream();
                            StreamReader reader = new StreamReader(stream);
                            responseFromServer = reader.ReadToEnd();

                            if (string.IsNullOrWhiteSpace(responseFromServer))
                            {
                                Debug.WriteLine("Response contained empty body...");
                            }
                            App.UserLoggedIn = false;
                            return responseFromServer;
                        }
                    }
                }
            }
            catch (WebException err)
            {
                Stream stream = err.Response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                responseFromServer = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(responseFromServer))
                {
                    Debug.WriteLine("Response contained empty body...");
                }
            }
            Debug.WriteLine("end of create session {0}", token);
            return token;
        }
    }
}