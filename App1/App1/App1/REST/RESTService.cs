using App1.Layout;
using App1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace App1.REST
{
    public class RESTService : IRESTService
    {
        public static string Token = "";
        public static string Payment = "";
        public static string body = "";
        private HttpClient _client;

        /*  204 (NO CONTENT) – the request has been successfully processed and the response is intentionally blank.
        400 (BAD REQUEST) – the request is not understood by the server.
        404 (NOT FOUND) – the requested resource does not exist on the server.*/

        //This function will be used to GET information for the user that doesn´t require any type of login or access tokens
        public async Task<T> GetwithoutToken<T>(string url)
        {
            var responseFromServer = "";
            //the url used to make the request for the information
            var uri = string.Format(url);
            var request2 = (HttpWebRequest)WebRequest.Create(uri);
            request2.ContentType = "application/json";
            //method used
            request2.Method = "GET";

            try
            {
                using (HttpWebResponse response = await request2.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Debug.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    }
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        //this is used to read the requsted information
                        string content = reader.ReadToEnd();
                        Debug.WriteLine("content:  {0}", content);
                        //Used to deserilize the information in the determined list
                        var jsonResult = JsonConvert.DeserializeObject<T>(content);
                        Debug.WriteLine("Treated json {0}", jsonResult);
                        //in case that the string is null should return nothing
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            Debug.WriteLine("Response contained empty body...");
                            return jsonResult;
                        }
                        else
                        {
                            //should be the result that im looking for
                            Debug.WriteLine("jsonresult {0}", jsonResult);
                            return jsonResult;
                        }
                    }
                }
            }
            //used to catch any exception that may occur
            catch (WebException err)
            {
                Stream stream = err.Response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                responseFromServer = reader.ReadToEnd();
                var JsonResultException = JsonConvert.DeserializeObject<T>(responseFromServer);

                if (string.IsNullOrWhiteSpace(responseFromServer))
                {
                    Debug.WriteLine("Response contained empty body...");
                }
                return JsonResultException;
            }
        }

        //The user should be autheticated and this GET request will be using the tokenreceived from the function Createsession
        public async Task<string> GetWithToken(string url)
        {
            var uri = string.Format(url);
            //Used to make the request
            var request2 = (HttpWebRequest)WebRequest.Create(uri);
            request2.ContentType = "application/json";
            //Method used
            request2.Method = "GET";
            //Access token and its format
            var authToken = string.Format("token=\"{0}\"", Token);
            Debug.WriteLine("authtoken {0}", authToken);
            //Header request used to send the token
            request2.Headers[HttpRequestHeader.Authorization] = "DirectLogin " + authToken;

            try
            {
                using (HttpWebResponse response = await request2.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Debug.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    }
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            Debug.WriteLine("Response contained empty body...");
                            return content;
                        }
                        else
                        {
                            Debug.WriteLine("jsonresult getwithToken {0}", content);
                            return content;
                        }
                    }
                }
            }
            //used to catch any exception that may occur
            catch (WebException err)
            {
                Stream stream = err.Response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                var responseFromServer = reader.ReadToEnd();
                if (string.IsNullOrWhiteSpace(responseFromServer))
                {
                    Debug.WriteLine("Response contained empty body...");
                }
                return responseFromServer;
            }
        }

        //The user should be autheticated to make a payment all these attributes are received from the payment page
        public async Task<bool> MakePayment(Payments.To accountTo, Payments.To bankTo, Payments.Value currencyTo, Payments.Value amountTo, Payments.Body descriptionTo)
        {
            //Url used to make the request to OpenBank
            var url = string.Format(Constants.PaymentUrl, AccountsPage.Bankid, AccountsPage.Accountid);

            //Token needed to verify the users authenticity
            var authToken = string.Format(" token=\"{0}\"", Token);

            //body that will be sent to the server containing the information the user added to the payment info
            body = "{ \"to\" :{\"bank_id\":\"" + bankTo.bank_id + "\",\"account_id\":\"" + accountTo.account_id + "\" },  \"value\":{ \"currency\": \"" + currencyTo.currency + "\",   \"amount\":\"" + amountTo.amount + "\" },  \"description\":\"" + descriptionTo.description + "\"}";

            try
            {
                using (_client = new HttpClient())
                {
                    _client.MaxResponseContentBufferSize = 256000;
                    //_client.DefaultRequestHeaders.Add("content-type", "application/json");
                    //Headers
                    _client.DefaultRequestHeaders.Accept.Clear();

                    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("DirectLogin", authToken);
                    //body
                    var content = new StringContent(body);
                    content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                 /*     var formContent = new FormUrlEncodedContent(new[]
                      {
                          new KeyValuePair<string, string>("Content-Type", "application/json"),
                          new KeyValuePair<string, string>("Body", body)
                      });*/
                    
                    //Response message received from OpenBank
                    HttpResponseMessage response = null;
                    response = await _client.PostAsJsonAsync(url, body);
                   // response = await _client.PostAsync(url, content);

                    if (response != null && response.StatusCode != HttpStatusCode.OK)
                    {
                        Debug.WriteLine("BODY OF INFORMATION ADDED {0}", body);

                        Debug.WriteLine("Error fetching data. Server returned status code: {0} ::: {1} :::: {2}",response.StatusCode, response.Content.Headers, response.Headers);
                        Debug.WriteLine("Requestmessage: {0}", response.RequestMessage);
                        Debug.WriteLine("Requset uri {0}:", response.RequestMessage.RequestUri);
                        Debug.WriteLine("Requst header: {0}:: Content::{1}", response.RequestMessage.Headers, response.RequestMessage.Content.Headers);
                        return false;
                    }
                    else
                    {
                        Debug.WriteLine(@"				TodoItem successfully saved.");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
                return false;
            }
        }

        //verifcation of the users autheticty depending on the received tokenreceived
        public
        bool IsAutheticated
        {
            get
            {
                Debug.WriteLine("Here in authetiacation");
                var some = !string.IsNullOrWhiteSpace(Token);
                Debug.WriteLine("vaue of some {0}", some);
                return some;
            }
        }

        //Post of the users attributes of their login information and receiving the Access Token needed for future requests
        public async Task<bool> CreateSession(Users user, Users pass)
        {
            string responseFromServer;
            //Request to create the session of the user
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Constants.OpenBankAPI);
            request.ContentType = "application/json";
            //method used to post information
            request.Method = "POST";
            //Header information
            var authData = string.Format("username=\"{0}\", password=\"{1}\",consumer_key=\"{2}\"", user.User, pass.Password, Constants.oauth_consumer_key);
            request.Headers[HttpRequestHeader.Authorization] = "DirectLogin " + authData;

            try
            {
                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    using (var writer = new StreamWriter(requestStream))
                    {
                        // Send the data to the server
                        writer.Flush();
                        writer.Dispose();
                    }
                }
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Debug.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                        return false;
                    }
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        try
                        {
                            StreamReader reader = new StreamReader(dataStream);
                            Token = reader.ReadToEnd();
                            Debug.WriteLine("token received {0}", Token);
                            if (string.IsNullOrWhiteSpace(Token))
                            {
                                Debug.WriteLine("Response contained empty body...");
                            }
                            Debug.WriteLine("Response {0}", Token);
                            if (string.IsNullOrWhiteSpace(Token))
                            {
                                return false;
                            }
                            else
                            {
                                var json = Token;
                                Debug.WriteLine("json token {0}", json);
                                //deserializing string of information received into json type to then be called
                                var token1 = JsonConvert.DeserializeObject<Token>(json);
                                Debug.WriteLine("token.token {0}", token1.token);
                                Token = token1.token;
                                return true;
                            }
                        }
                        catch (WebException err)
                        {
                            Stream stream = err.Response.GetResponseStream();
                            StreamReader reader = new StreamReader(stream);
                            responseFromServer = reader.ReadToEnd();

                            if (string.IsNullOrWhiteSpace(responseFromServer))
                            {
                                Debug.WriteLine("Response contained empty body... {0}", err.Response);
                            }
                            return false;
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
                    Debug.WriteLine("Response contained empty body...{0}", err.Response);
                }
                return false;
            }
        }
    }
}