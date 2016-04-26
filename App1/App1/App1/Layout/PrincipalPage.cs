using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Layout
{
    class PrincipalPage : ContentPage
    {
        Label accountid, lastaccess, owner, iban, balance, bank, currency, typeaccount;

        public PrincipalPage() 
        {
            //specifying labels and buttons utilized
            accountid = new Label();
           //movement button should take you to the movements page
            lastaccess = new Label()
            {
                Text = "last access: ",
                HorizontalOptions = LayoutOptions.Start
            };
            owner = new Label()
            {
                Text = "owner: "
            };
            iban = new Label()
            {
                Text = "IBAN: "
            };
            balance = new Label()
            {
                Text = "Balance: "
            };
            bank = new Label()
            {
                Text = "Bank: "
            };
            currency = new Label()
            {
                Text = "Currency: "
            };
            typeaccount = new Label()
            {
                Text = "Type: "
            };
            var transactionButton = new Button
            {
                Text = "Transactions",
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Start
            };
            transactionButton.Clicked += OntransactionButtonClicked;
            var cardsbutton = new Button()
            {
                Text = "Cards",
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.End
            };

            //image
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
            /*
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
                        imagegrid.Children.Add(ImageRobot, 0, 0);

                        //innergrid contaning login for user to provide login info
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

                        //button grid containg buttons that alocate you to another page
                        buttongrid.Children.Add(ContactButton, 0, 0);
                        buttongrid.Children.Add(BalcaoButton, 1, 0);
                        buttongrid.Children.Add(AtmButton, 2, 0);

                        // stackLayout.Children.Add(outerGrid);
                        stackLayout.Children.Add(imagegrid);
                        stackLayout.Children.Add(innerGrid);
                        stackLayout.Children.Add(buttongrid);
                        this.Content = stackLayout;*/
        }

        private void OntransactionButtonClicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        
    }
}
