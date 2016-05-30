using App1.Layout;
using App1.Models;
using App1.REST;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
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
            switcher.Toggled += Switchertoggled;
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
            }, 0, 0);
            innerGrid.Children.Add(userEntry, 0, 1);
            innerGrid.Children.Add(new Label()
            {
                BackgroundColor = Color.Gray,
                Text = "Password"
            }, 0, 2);
            innerGrid.Children.Add(passwordEntry, 0, 3);
            innerGrid.Children.Add(loginButton, 0, 4);
            innerGrid.Children.Add(messageLabel, 0, 5);

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
            var rest = new ManagerRESTService(new RESTService());
            var user = new Users
            {
                User = userEntry.Text,
            };
            var pass = new Users
            {
                Password = passwordEntry.Text
            };
            //checks if the user entry and password entry are empty
            if (userEntry.Text == null || passwordEntry.Text == null)
            {
                userEntry.Text = String.Empty;
                passwordEntry.Text = String.Empty;
                messageLabel.Text = String.Empty;
            }
            //Verfication of users information through OpenBanks Direct Login where the user should receive a token
            //this token is never shown to the user, used in background functions to request authorized information for the user
            var result = await rest.CreateSession(user, pass);
            Debug.WriteLine("result {0}", result);
            //if the result is false it will stay on the same page and show the message stated else it will change to the next page
            try
            {
                if (result)
                {
                    try
                    {
                        //could be passing to much information may have to simplify
                        await Navigation.PushAsync(new PrincipalPage());
                    }
                    catch (Exception err)
                    {
                        Debug.WriteLine("Caught error: {0}.", err);
                    }
                    //  var uri = string.Format(Constants.AccountUrl);
                    //  var result2 = await rest.GetWithToken(uri, 1);
                }
                else
                {
                    messageLabel.Text = "Login Failed";
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }

        //changes the mode according to the switch to public or private views of users accounts
        //this part may not be implemented at the moment needs some verifications beforehand may have future implications
        private void Switchertoggled(object sender, ToggledEventArgs e)
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
            var rest = new ManagerRESTService(new RESTService());
            Debug.WriteLine("Clicked contact button");
            //get informatin connected to the banks contact information localized on OpenBanks sandbox
            var uri = string.Format(Constants.BankUrl);
            await rest.GetwithoutToken(uri, 1);
            var banklist = new Banklist();
          /*  foreach (var item in banklist.banks)
            {
                Debug.WriteLine("itemid loginpage {0}",item.id);
            }*/

            try
            {
                await Navigation.PushAsync(new ContactPage());
            }
            catch (NullReferenceException err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }

        //what happens when we click the Balcao button
        private async void OnBalcaoButtonClicked(object sender, EventArgs e)
        {
            var banks = new banks();
            var rest = new ManagerRESTService(new RESTService());
            Debug.WriteLine("Clicked Bank Map button");
            //get information connected to the banks branch information localized on OpenBanks sandobox this needs a bankid
            try
            {
                await rest.GetwithoutToken(Constants.BankUrl, 1);
                var uri = string.Format(Constants.BranchesUrl, banks.id);
                await rest.GetwithoutToken(uri, 2);
                await Navigation.PushAsync(new BalcaoPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }

        //what happens when we click the Atm button
        private async void OnAtmButtonClicked(object sender, EventArgs e)
        {
            var banks = new banks();
            var rest = new ManagerRESTService(new RESTService());
            Debug.WriteLine("Clicked ATM Map button");
            //get information connected to the banks ATM information localized on OpenBanks sandbox
            try
            {
                await rest.GetwithoutToken(Constants.BankUrl, 1);
                var uri = string.Format(Constants.ATMsUrl, banks.id);
                await rest.GetwithoutToken(uri, 3);
                await Navigation.PushAsync(new AtmPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }
    }
}