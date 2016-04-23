using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1.Layout;
using App1.REST;
using Microsoft.VisualBasic;
using Xamarin.Forms;

namespace App1
{
    public class LoginPage : ContentPage
    {
        Entry userEntry, passwordEntry;
        Label messageLabel;

        //layout of the page
        public LoginPage()
        {
            //specifying labels and buttons utilized
            messageLabel = new Label();
            userEntry = new Entry
            {
                Placeholder = "username"
            };
            passwordEntry = new Entry
            {
                IsPassword = true
            };
            //login button
            var loginButton = new Button
            {
                Text = "Login"
            };
            loginButton.Clicked += OnLoginButtonClicked;
            //contacts button should take you to the contacts page
            var ContactButton = new Button()
            {
                Text = "Contacts",
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Start
            };
            ContactButton.Clicked += OnContactButtonClicked;
            //Balcao button should take you to the banks location page
            var BalcaoButton = new Button()
            {
                Text = "BankLocation",
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Center
            };
            BalcaoButton.Clicked += OnBalcaoButtonClicked;
            //Atm button should take you to the ATM location page
            var AtmButton = new Button()
            {
                Text = "ATMLocation",
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.End
            };
            AtmButton.Clicked += OnAtmButtonClicked;


            Title = "Login";
            //Specific layout of each of the labels and buttons used
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = {
                    new Label { Text = "Username" },
                    userEntry,
                    new Label { Text = "Password" },
                    passwordEntry,
                    loginButton,
                    messageLabel,
                    ContactButton,
                    BalcaoButton,
                    AtmButton
                }
                
            };
        }

        //what happens when we click the login button
        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var user = new Users
            {
                User = userEntry.Text,
                Password = passwordEntry.Text
            };

            //verfication of users information should be able to connect to class that takes care of users information
            var Verification = VerifyInfo(user);
            if (Verification)
            {
                App.UserLoggedIn = true;
                Navigation.InsertPageBefore(new PrincipalPage(), this);
                await Navigation.PopAsync();
            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
            }
        }
        //verifing info that is contained in the REST API OpenBank
        bool VerifyInfo(Users user)
        {
            return true;
        }

        //what happens when we click the contact button
        async void OnContactButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        //what happens when we click the Balcao button
        private void OnBalcaoButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        //what happens when we click the Atm button
        private void OnAtmButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
