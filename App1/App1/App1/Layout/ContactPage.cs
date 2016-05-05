using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using App1.Models;
using App1.REST;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class ContactPage : ContentPage
    {
        private Label BankInfo;
        
        public ContactPage()
        {
            var searchBanks = new Banks();
            UserInContactPage(searchBanks);
            BankInfo = new Label();
            //The contact page will contain the banks URLs to websites
            Title = "ContactPage";
            Icon = new FileImageSource { File = "robot.png" };
            var stackLayout = new StackLayout
            {
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Children = {
                    BankInfo,
                    new Label {
                        BackgroundColor = Color.Gray,
                        Text = "",
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Start
                    },
                    new Label {
                        BackgroundColor = Color.Gray,
                        Text = "Bank 2 : OBP",
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Start
                    },
                    new Label {
                        BackgroundColor = Color.Gray,
                        Text = "Bank 3 : OBP2",
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.Start
                    }
                }
            };
            stackLayout.Children.Add(BankInfo);
            this.Content = stackLayout;
        }

        private void UserInContactPage(Banks banks)
        {
            string responseFromServer = "";
            WebRequest request = (HttpWebRequest)WebRequest.Create(Banks.BankUrl);
            request.Method = "GET";
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
            Debug.WriteLine("userincontactpage response: "+ responseFromServer);
            Debug.WriteLine("userincontactpage asyncresult" + asyncResult);

            // if it worked, we should have oauth_token and oauth_token_secret in the response
            foreach (string pair in responseFromServer.Split(new char[] { ',' }))
            {
                string[] split_pair = pair.Split(new char[] { '"' });

                switch (split_pair[0])
                {
                    case "id":
                        banks.BankId = split_pair[1];
                        break;

                    case "full_name":
                        banks.BankfullName = split_pair[1];
                        break;

                    case "short_name":
                        banks.BankshortName = split_pair[1];
                        break;

                    case "website":
                        banks.Bankwebsite = split_pair[1];
                        break;
                }
            }
            Debug.WriteLine(banks.BankId);
            Debug.WriteLine(banks.BankfullName);
            Debug.WriteLine(banks.BankshortName);
            Debug.WriteLine(banks.Bankwebsite);
            BankInfo.Text =  String.Format("{0}:{1}:{2}", banks.BankfullName, banks.BankshortName, banks.Bankwebsite);
            BankInfo.BackgroundColor = Color.Gray;
        }
    }
}