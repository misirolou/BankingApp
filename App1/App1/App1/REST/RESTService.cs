using App1.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace App1.REST
{
    public class RESTService : IRESTService
    {
        public static string token = "";

        /*  204 (NO CONTENT) – the request has been successfully processed and the response is intentionally blank.
        400 (BAD REQUEST) – the request is not understood by the server.
        404 (NOT FOUND) – the requested resource does not exist on the server.*/

        //This function will be used to GET information for the user that doesn´t require any type of login

        public async Task<T> GetwithoutToken<T>(string url)
        {
            var responseFromServer = "";
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
                    }
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string content = reader.ReadToEnd();
                        var JsonResult = JsonConvert.DeserializeObject<T>(content);
                        Debug.WriteLine("Treated json {0}", JsonResult);
                        //in case that the string is null should return nothing
                        if (string.IsNullOrWhiteSpace(content))
                        {
                            Debug.WriteLine("Response contained empty body...");
                            return JsonResult;
                        }
                        else
                        {
                            //should be the result that im looking for
                            Debug.WriteLine("jsonresult {0}", JsonResult);
                            return JsonResult;
                        }
                    }
                }
            }
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
        public async Task<bool> GetWithToken(string url, int choice)
        {
            var uri = string.Format(url);
            var request2 = (HttpWebRequest)WebRequest.Create(uri);
            request2.ContentType = "application/json";
            request2.Method = "GET";
            var authToken = string.Format("token= \"{0}\"", token);
            Debug.WriteLine("authtoken {0}", authToken);
            request2.Headers[HttpRequestHeader.Authorization] = "DirectLogin " + authToken;

            try
            {
                using (HttpWebResponse response = await request2.GetResponseAsync() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Debug.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                        return false;
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
                            switch (choice)
                            {
                                case 1: //the login account used and the info that it contains
                                    Debug.WriteLine("Response Body: \r\n {0}", content);
                                    //deserializing string of information received into json type to then be called
                                    List<Accounts> info = JsonConvert.DeserializeObject<List<Accounts>>(content);
                                    Debug.WriteLine(info.Count);
                                    foreach (var item in info.ToString())
                                    {
                                        Debug.WriteLine("itemid :{0}", item);
                                    }
                                    break;

                                case 2: //Account with more detailed information containg things like the account balance, IBAN, etc
                                    Debug.WriteLine("Response Body: \r\n {0}", content);
                                    //deserializing string of information received into json type to then be called
                                    AccountInfo info1 = JsonConvert.DeserializeObject<AccountInfo>(content);
                                    Debug.WriteLine(info1);
                                    foreach (var item in info1.ToString())
                                    {
                                        Debug.WriteLine("itemid :{0}", item);
                                    }
                                    break;

                                case 3: //this will contain the users transaction information
                                    Debug.WriteLine("Response Body: \r\n {0}", content);
                                    //deserializing string of information received into json type to then be called
                                    JsonConvert.DeserializeObject<AccountInfo>(content);
                                    break;

                                case 4: //this will contain the users card information
                                    Debug.WriteLine("Response Body: \r\n {0}", content);
                                    //deserializing string of information received into json type to then be called
                                    JsonConvert.DeserializeObject<AccountInfo>(content);
                                    break;

                                default: break;
                            }
                        }
                        return true;
                    }
                }
            }
            catch (WebException err)
            {
                Stream stream = err.Response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                var responseFromServer = reader.ReadToEnd();

                if (string.IsNullOrWhiteSpace(responseFromServer))
                {
                    Debug.WriteLine("Response contained empty body...");
                }
                return false;
            }
        }

        public async Task<T> getResponse<T>(string url)
        {
            var client = new System.Net.Http.HttpClient();

            try
            {
                //Definide o Header de resultado para JSON, para evitar que seja retornado um HTML ou XML
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Formata a Url com o metodo e o parametro enviado e inicia o acesso a Api. Como o acesso será por meio
                //da Internet, pode demorar muito, para que o aplicativo não trave usamos um método assincrono
                //e colocamos a keyword AWAIT, para que a Thread principal - UI - continuo sendo executada
                //e o método so volte a ser executado quando o download das informações for finalizado
                var response = await client.GetAsync(string.Format(url));

                //Lê a string retornada
                var JsonResult = response.Content.ReadAsStringAsync().Result;

                //Converte o resultado Json para uma Classe utilizando as Libs do Newtonsoft.Json
                var rootobject = JsonConvert.DeserializeObject<T>(JsonResult);

                return rootobject;
            }
            catch (WebException err)
            {
                Stream stream = err.Response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                var responseFromServer = reader.ReadToEnd();
                var rootobject = JsonConvert.DeserializeObject<T>(responseFromServer);
                if (string.IsNullOrWhiteSpace(responseFromServer))
                {
                    Debug.WriteLine("Response contained empty body...");
                }
                return rootobject;
            }
        }

        //verifcation of the users autheticty depending on the received tokenreceived
        public
        bool IsAutheticated
        {
            get
            {
                Debug.WriteLine("Here in authetiacation");
                var some = !string.IsNullOrWhiteSpace(token);
                Debug.WriteLine("vaue of some {0}", some);
                return some;
            }
        }

        //Post of the users attributes of their login information and receiving the tokenreceived needed for future requests
        public async Task<bool> CreateSession(Users user, Users pass)
        {
            string responseFromServer;
            //pedido ao servidor pelo request tokenreceived
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
                        return false;
                    }
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        try
                        {
                            StreamReader reader = new StreamReader(dataStream);
                            token = reader.ReadToEnd();
                            if (string.IsNullOrWhiteSpace(token))
                            {
                                Debug.WriteLine("Response contained empty body...");
                            }
                            Debug.WriteLine("Response {0}", token);
                            if (string.IsNullOrWhiteSpace(token))
                            {
                                return false;
                            }
                            else
                            {
                                var json = token;
                                Debug.WriteLine("json token {0}", json);
                                //deserializing string of information received into json type to then be called
                                var token1 = JsonConvert.DeserializeObject<Token>(json);
                                Debug.WriteLine(token1.token);
                                token = token1.token;
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
                                Debug.WriteLine("Response contained empty body...");
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
                    Debug.WriteLine("Response contained empty body...");
                }
            }
            Debug.WriteLine("end of create session {0}", token);
            return false;
        }
    }
}