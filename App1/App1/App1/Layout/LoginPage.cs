using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
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
        Label messageLabel, privacy;

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
            //switch button choosing if you want your information public or private
            Switch switcher = new Switch()
            {
                HorizontalOptions = LayoutOptions.End
            };
            //label for private mode
            privacy = new Label()
            {
                Text = "Private Mode",
                HorizontalOptions = LayoutOptions.Start
            };
            //contacts button should take you to the contacts page
            var ContactButton = new Button()
            {
                Text = "Contacts",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.StartAndExpand   
            };
            ContactButton.Clicked += OnContactButtonClicked;
            //Balcao button should take you to the banks location page
            var BalcaoButton = new Button()
            {
                Text = "Bank GPS",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            BalcaoButton.Clicked += OnBalcaoButtonClicked;
            //Atm button should take you to the ATM location page
            var AtmButton = new Button()
            {
                Text = "ATM GPS",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            AtmButton.Clicked += OnAtmButtonClicked;
            //login image
            var ImageRobot = new Image()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
            };
            //specifying location for each platform
            ImageRobot.Source = Device.OnPlatform(
                iOS: ImageSource.FromFile("Images/robot.png"),
                Android: ImageSource.FromFile("robot.png"),
                WinPhone: ImageSource.FromFile("Images/robot.png"));

            //Layout of the login page
            Title = "Login";
            //this is the type of layout the grids will be specified in 
            var stackLayout = new StackLayout
            {
                //Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Padding = 1
            };

            //specification of the switcher grid layout
            var imagegrid = new Grid
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
                }
            };

            //specification of the innergrid layout
            var innerGrid = new Grid
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                RowDefinitions =
                {
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                }
            };

            //specfication of the button grids layout
            var buttongrid = new Grid
            {
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                RowDefinitions =
                {
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)},
                }
            };

            //imagegrid contains the inicial image of the login page
            imagegrid.Children.Add(ImageRobot,0,0);

            //innergrid contaning login for user to provide login info
            innerGrid.Children.Add(new Label()
            {
                BackgroundColor = Color.Gray,
                Text = "username"
            },0,0);
            innerGrid.Children.Add(userEntry,0,1);
            innerGrid.Children.Add(new Label()
            {
                BackgroundColor = Color.Gray,
                Text = "Password"
            },0,2);
            innerGrid.Children.Add(passwordEntry,0,3);
            innerGrid.Children.Add(loginButton,0,4);

            //button grid containg buttons that alocate you to another page
            buttongrid.Children.Add(ContactButton,0,0);
            buttongrid.Children.Add(BalcaoButton,1,0);
            buttongrid.Children.Add(AtmButton,2,0);

            // stackLayout.Children.Add(outerGrid);
            stackLayout.Children.Add(imagegrid);
            stackLayout.Children.Add(innerGrid);
            stackLayout.Children.Add(buttongrid);
            this.Content = stackLayout;
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
            Navigation.InsertPageBefore(new ContactPage(), this);
            await Navigation.PopAsync();
        }

        //what happens when we click the Balcao button
        async void OnBalcaoButtonClicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new BalcaoPage(), this);
            await Navigation.PopAsync();
        }

        //what happens when we click the Atm button
        async void OnAtmButtonClicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new AtmPage(), this);
            await Navigation.PopAsync();
        }
    }
}
