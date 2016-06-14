using App1.Layout;
using App1.REST;
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
            //login image
            var imageRobot = new Image()
            {
                Aspect = Aspect.AspectFill,
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            //specifying location for each platform
            imageRobot.Source = Device.OnPlatform(
                iOS: ImageSource.FromFile("robot.png"),
                Android: ImageSource.FromFile("robot.png"),
                WinPhone: ImageSource.FromFile("robot.png"));
            //specifying labels and buttons utilized
            messageLabel = new Label();
            userEntry = new Entry
            {
                Placeholder = "username"
            };
            //Once the user has completed filling in the information something will happen
            userEntry.Completed += (sender, args) =>
            {
                var playingaround = new Animation();

                var rotation = new Animation(callback: d => imageRobot.Rotation = d,
                    start: imageRobot.Rotation,
                    end: imageRobot.Rotation + 360,
                    easing: Easing.SpringOut);
                playingaround.Add(0, 1, rotation);

                playingaround.Commit(imageRobot, "Loop", length: 1400);
            };
            passwordEntry = new Entry
            {
                IsPassword = true,
                Placeholder = "Password"
            };
            //Once the user has completed filling in the information something will happen
            passwordEntry.Completed += (sender, args) =>
            {
                var width = Application.Current.MainPage.Width;
                var playingaround = new Animation();
                //the image exits to the right
                var exitRight = new Animation(callback: d => imageRobot.TranslationX = d,
                                               start: 0,
                                               end: width,
                                               easing: Easing.SpringIn);

                //the image returns from the left
                var enterLeft = new Animation(callback: d => imageRobot.TranslationX = d,
                                               start: -width,
                                               end: 0,
                                               easing: Easing.BounceOut);

                playingaround.Add(0, 0.5, exitRight);
                playingaround.Add(0.5, 1, enterLeft);

                playingaround.Commit(imageRobot, "In/Out", length: 1400);
            };
            //login button
            Button loginButton = new Button
            {
                Text = "Login",
            };
            loginButton.Clicked += OnLoginButtonClicked;
            //switch button choosing if you want your information public or private
            /*  Switch switcher = new Switch()
              {
                  HorizontalOptions = LayoutOptions.End
              };
              switcher.Toggled += Switchertoggled;*/
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
            /* var Switchergrid = new Grid
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
             };*/

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
                     new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)}
                }
            };

            messageLabel.BackgroundColor = Color.Gray;

            outergrid.Children.Add(imagegrid, 0, 0);
            // outergrid.Children.Add(Switchergrid, 0, 1);
            outergrid.Children.Add(innerGrid, 0, 2);

            //imagegrid contains the inicial image of the login page
            imagegrid.Children.Add(imageRobot, 0, 0);

            //Switcher grid containing the switcher used to choose between public and private accounts
            //Switchergrid.Children.Add(privacy, 0, 0);
            // Switchergrid.Children.Add(switcher, 1, 0);

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
            innerGrid.Children.Add(ContactButton, 0, 6);

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
            if (result)
            {
                try
                {
                    //could be passing to much information may have to simplify
                    await Navigation.PushAsync(new AccountsPage());
                }
                catch (Exception err)
                {
                    Debug.WriteLine("Caught error: {0}.", err);
                }
            }
            else
            {
                await DisplayAlert("Alert", "Login Failed", "OK");
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
            //go to the contact page where the information will be taken care of
            try
            {
                await Navigation.PushAsync(new ContactPage());
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }
    }
}