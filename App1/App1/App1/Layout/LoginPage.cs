using App1.Layout;
using App1.REST;
using System;
using System.Diagnostics;
using App1.Models;
using Xamarin.Forms;


namespace App1
{
    public class LoginPage : ContentPage
    {
        private Entry userEntry, passwordEntry;
        private Label messageLabel, privacy;

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
                IsPassword = true,
                Placeholder = "Password"
            };
            //login button
            Button loginButton = new Button
            {
                Text = "Login",
            };
            loginButton.Clicked += OnLoginButtonClicked;
            //switch button choosing if you want your information public or private
            Switch switcher = new Switch()
            {
                HorizontalOptions = LayoutOptions.End
            };
            switcher.Toggled += switchertoggled;
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
                Text = "Bank Map",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            BalcaoButton.Clicked += OnBalcaoButtonClicked;
            //Atm button should take you to the ATM location page
            var AtmButton = new Button()
            {
                Text = "ATM Map",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            AtmButton.Clicked += OnAtmButtonClicked;
            //login image
            var ImageRobot = new Image()
            {
                Aspect = Aspect.AspectFill,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            //specifying location for each platform
            ImageRobot.Source = Device.OnPlatform(
                iOS: ImageSource.FromFile("robot.png"),
                Android: ImageSource.FromFile("robot.png"),
                WinPhone: ImageSource.FromFile("robot.png"));

            //Layout of the login page
            Title = "Login";
            Icon = new FileImageSource() { File = "robot.png" };

            //this is the type of layout the grids will be specified in
            var stackLayout = new StackLayout
            {
                BackgroundColor = Color.Teal,
                Padding = 1
            };

            //All the grids are contained in this outergrid
            var outergrid = new Grid()
            {
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(3, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
                }
            };
            //specification of the image grid layout
            var imagegrid = new Grid
            {
                HorizontalOptions = LayoutOptions.Center,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                }
            };

            //switcher grid choosing if you want your public or private accounts
            var Switchergrid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                }
            };

            //specification of the innergrid layout
             var innerGrid = new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
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
                VerticalOptions = LayoutOptions.EndAndExpand,
                RowDefinitions =
                {
                    new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                }
            };

            messageLabel.BackgroundColor = Color.Gray;

            outergrid.Children.Add(imagegrid, 0, 0);
            outergrid.Children.Add(Switchergrid, 0, 1);
            outergrid.Children.Add(innerGrid, 0, 2);
            outergrid.Children.Add(buttongrid, 0, 3);
            
            //imagegrid contains the inicial image of the login page
            imagegrid.Children.Add(ImageRobot, 0, 0);

            //Switcher grid containing the switcher used to choose between public and private accounts
            Switchergrid.Children.Add(privacy, 0, 0);
            Switchergrid.Children.Add(switcher, 1, 0);

            //innergrid contaning login for user to provide ´the necessary login information
             innerGrid.Children.Add(new Label()
             {
                 BackgroundColor = Color.Gray,
                 Text = "username"
             },0,0);
            innerGrid.Children.Add(userEntry, 0, 1);
            innerGrid.Children.Add(new Label()
             {
                 BackgroundColor = Color.Gray,
                 Text = "Password"
             },0,2);
            innerGrid.Children.Add(passwordEntry, 0, 3);
            innerGrid.Children.Add(loginButton, 0, 4);
            innerGrid.Children.Add(messageLabel,0,5);

            //button grid containg buttons that alocate you to another page
            buttongrid.Children.Add(ContactButton, 0, 0);
            buttongrid.Children.Add(BalcaoButton, 1, 0);
            buttongrid.Children.Add(AtmButton, 2, 0);

            stackLayout.Children.Add(outergrid);
            this.Content = stackLayout;
        }

        //what happens when we click the login button
        private async void OnLoginButtonClicked(object sender, EventArgs e)
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
                Debug.WriteLine("verifcation is true");
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
        private bool VerifyInfo(Users user)
        {
            Debug.WriteLine("VerifyInfo user: " + user.User);
            Debug.WriteLine("VerifyInfo password: " + user.Password);
            return user.User == userEntry.Text && user.Password == passwordEntry.Text; 
        }

        //changes the mode according to the switch to public or private verification
        private void switchertoggled(object sender, ToggledEventArgs e)
        {
            if (e.Value.Equals(true))
            {
                Debug.WriteLine("changed text to public");
                privacy.Text = String.Format("{0} mode", "Public");
            }
            else
            {
                Debug.WriteLine("changed text to private");
                privacy.Text = String.Format("{0} mode", "Private");
            }
        }

        //what happens when we click the contact button
        private async void OnContactButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Clicked contact button");
            Navigation.InsertPageBefore(new ContactPage(), this);
            await Navigation.PopAsync();
        }

        //what happens when we click the Balcao button
        private async void OnBalcaoButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Clicked Bank Map button");
            Navigation.InsertPageBefore(new BalcaoPage(), this);
            await Navigation.PopAsync();
        }

        //what happens when we click the Atm button
        private async void OnAtmButtonClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Clicked ATM Map button");
            Navigation.InsertPageBefore(new AtmPage(), this);
            await Navigation.PopAsync();
        }
    }
}