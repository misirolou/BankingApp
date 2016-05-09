using App1.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Json;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class ContactPage : ContentPage
    {
        private Label BankInfo;

        public ContactPage()
        {
            Button Back = new Button()
            {
                Image = (FileImageSource)Device.OnPlatform(
                        iOS: ImageSource.FromFile("Back.png"),
                        Android: ImageSource.FromFile("Back.png"),
                        WinPhone: ImageSource.FromFile("Back.png")),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.Gray
            };
            Back.Clicked += BackButtonClicked;

            BankInfo = new Label();
            //The contact page will contain the banks URLs to websites
            Title = "ContactPage";
            Icon = new FileImageSource { File = "Phone.png" };
            var stackLayout = new StackLayout
            {
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Children = {
                    Back,
                    BankInfo,
                    new Label() {
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

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopAsync();
        }

        // Parse the various requests made from the httprequests and add them to the screen
        // conditions, and location to the screen.
        private void ParseAndDisplay(JsonValue json)
        {
            var term = new Banks();
            var obj = JsonObject.Parse(json);
            //specifying the main key which is the object
            var properties = obj["Banks"];
            //specifying the properties used in the banks obj key
            term.BankId = properties["id"];
            term.BankshortName = properties["short_name"];
            term.BankfullName = properties["full_name"];
            term.Bankwebsite = properties["website"];
            // Get the weather reporting fields from the layout resource:
            // JsonValue BankID =  Task<Label>(Banks BankID);

            // Extract the array of name/value results for the field name "weatherObservation".
            //BankID = json["id"];
        }

        public async Task<JsonValue> UserInContactPage()
        {
            //pedido ao servidor pelo request token
            WebRequest request = (HttpWebRequest)WebRequest.Create(Banks.BankUrl);
            request.Method = "GET";
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

        // if it worked, we should have oauth_token and oauth_token_secret in the response
        /* foreach (string pair in .Split(new char[] { ',' }))
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
         BankInfo.Text = String.Format("{0}:{1}:{2}", banks.BankfullName, banks.BankshortName, banks.Bankwebsite);
         BankInfo.BackgroundColor = Color.Gray;*/
    }
}