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
            cardsbutton.Clicked += OncardsButtonClicked;
            //image
            var menu = new Image()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
            };
            //specifying location for each platform
            menu.Source = Device.OnPlatform(
                iOS: ImageSource.FromFile("menu.png"),
                Android: ImageSource.FromFile("menu.png"),
                WinPhone: ImageSource.FromFile("menu.png"));

            var exit = new Image()
            {
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Start,
            };
            //specifying location for each platform
            exit.Source = Device.OnPlatform(
                iOS: ImageSource.FromFile("robot.png"),
                Android: ImageSource.FromFile("robot.png"),
                WinPhone: ImageSource.FromFile("robot.png"));
            
            //Layout of the Home page(PrincipalPage.cs)
            Title = "Home";
            Icon = new FileImageSource() {File = "robot.png"};
            //this is the type of layout the grids will be specified in 
            var stackLayout = new StackLayout
            {
                //Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Padding = 1
            };
            //specification of the switcher grid layout
                        var menubar = new Grid
                        {
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalOptions = LayoutOptions.StartAndExpand,
                            RowDefinitions =
                            {
                                new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
                            },
                            ColumnDefinitions =
                            {
                                new ColumnDefinition { Width = GridLength.Auto},
                                new ColumnDefinition { Width = GridLength.Auto},
                                new ColumnDefinition { Width = GridLength.Auto}
                            }
                        };

                        //specification of the innergrid layout
                        var infoGrid = new Grid
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
                                new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                                new RowDefinition {Height =  new GridLength(1, GridUnitType.Auto)},
                            }
                        };

                        //specfication of the button grids layout
                        var button2grid = new Grid
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
                                new ColumnDefinition {Width = new GridLength(1, GridUnitType.Auto)}
                            }
                        };

                        //imagegrid contains the inicial image of the login page
                        menubar.Children.Add(menu, 0, 0);
                        menubar.Children.Add(lastaccess, 1, 0);
                        menubar.Children.Add(exit, 2, 0);

                        //innergrid contaning login for user to provide login info
                        infoGrid.Children.Add(accountid,0,0);
                        infoGrid.Children.Add(owner, 0, 1);
                        infoGrid.Children.Add(iban, 0, 2);
                        infoGrid.Children.Add(balance, 0, 3);
                        infoGrid.Children.Add(bank, 0, 4);
                        infoGrid.Children.Add(currency, 0, 5);
                        infoGrid.Children.Add(typeaccount, 0, 6);
                        //button grid containg buttons that alocate you to another page
                        button2grid.Children.Add(transactionButton, 0, 0);
                        button2grid.Children.Add(cardsbutton, 1, 0);
                        
                        // stackLayout.Children.Add(outerGrid);
                        stackLayout.Children.Add(menubar);
                        stackLayout.Children.Add(infoGrid);
                        stackLayout.Children.Add(button2grid);
                        this.Content = stackLayout;
        }

        //should take te user to the transaction page
        async void OntransactionButtonClicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new transactionPage(), this);
            await Navigation.PopAsync();
        }

        //should take the user to the cards page
        async void OncardsButtonClicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new cardPage(), this);
            await Navigation.PopAsync();
        }

    }
}
