using App1.Cell;
using App1.Models;
using App1.REST;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace App1.Layout
{
    internal class PrincipalPage : ContentPage
    {
        private Label accountid, owner, iban, balance, bank, currency, typeaccount;

        private ListView listView;

        public PrincipalPage()
        {
            var AccountInfo = new AccountInfo.AccountInfoDetailed();
            //specifying labels and buttons utilized
            accountid = new Label()
            {
                Text = "id  ",
                BackgroundColor = Color.Gray,
            };
            //movement button should take you to the movements page
            owner = new Label()
            {
                Text = "Owner: ",
                BackgroundColor = Color.Gray
            };
            iban = new Label()
            {
                Text = "IBAN: ",
                BackgroundColor = Color.Gray
            };
            balance = new Label()
            {
                Text = "Balance: ",
                BackgroundColor = Color.Gray
            };
            bank = new Label()
            {
                Text = "Bank: ",
                BackgroundColor = Color.Gray
            };
            currency = new Label()
            {
                Text = "Currency: ",
                BackgroundColor = Color.Gray
            };
            typeaccount = new Label()
            {
                Text = "Type: ",
                BackgroundColor = Color.Gray
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

            ImageCell menu = new ImageCell()
            {
                ImageSource = Device.OnPlatform(
                    iOS: ImageSource.FromFile("menu.png"),
                    Android: ImageSource.FromFile("menu.png"),
                    WinPhone: ImageSource.FromFile("menu.png"))
            };
            menu.Tapped += async (sender, args) => await Navigation.PushAsync(new MenuPage());

            ImageCell exit = new ImageCell()
            {
                ImageSource = Device.OnPlatform(
                iOS: ImageSource.FromFile("menu.png"),
                Android: ImageSource.FromFile("menu.png"),
                WinPhone: ImageSource.FromFile("menu.png")),
            };
            exit.Tapped += async (sender, args) => await Navigation.PopToRootAsync();

            //Button to go back
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

            //Layout of the Home page(PrincipalPage.cs)
            Title = "Home";
            Icon = new FileImageSource() { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            //this is the type of layout the grids will be specified in
            var stackLayout = new StackLayout
            {
                //Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Padding = 1
            };

            ObservableCollection<AccountInfo> AccountInfoList = new ObservableCollection<AccountInfo>();

            listView = new ListView
            {
                ItemsSource = AccountInfoList,
                ItemTemplate = new DataTemplate(() =>
                {
                    var nativeCell = new AccountCell();
                    nativeCell.SetBinding(AccountCell.IDProperty, "id");
                    nativeCell.SetBinding(AccountCell.OwnerProperty, "owner");
                    nativeCell.SetBinding(AccountCell.BalanceProperty, "balance");
                    nativeCell.SetBinding(AccountCell.BankProperty, "bank");
                    nativeCell.SetBinding(AccountCell.IbanProperty, "IBAN");
                    nativeCell.SetBinding(AccountCell.CurrencyProperty, "currency");
                    nativeCell.SetBinding(AccountCell.TypeProperty, "type");
                    return nativeCell;
                })
            };

            //adding a table view for the images of the menu and exit images
            var tableview = new TableView()
            {
                Intent = TableIntent.Form,
                Root = new TableRoot()
                {
                    new TableSection()
                    {
                        menu, exit
                    }
                }
            };
            tableview.Root = new TableRoot { new TableSection() { new ViewCell() { View = stackLayout } } };

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

            //infogrid contining users info
            infoGrid.Children.Add(accountid, 0, 0);
            infoGrid.Children.Add(owner, 0, 1);
            infoGrid.Children.Add(iban, 0, 2);
            infoGrid.Children.Add(balance, 0, 3);
            infoGrid.Children.Add(bank, 0, 4);
            infoGrid.Children.Add(currency, 0, 5);
            infoGrid.Children.Add(typeaccount, 0, 6);
            //button grid containg buttons that alocate you to another page
            button2grid.Children.Add(transactionButton, 0, 0);
            button2grid.Children.Add(cardsbutton, 1, 0);

            stackLayout.Children.Add(tableview);
            stackLayout.Children.Add(menubar);
            stackLayout.Children.Add(listView);
            // stackLayout.Children.Add(infoGrid);
            stackLayout.Children.Add(button2grid);
            this.Content = stackLayout;
        }

        private void BackButtonClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        //should take te user to the transaction page
        private async void OntransactionButtonClicked(object sender, EventArgs e)
        {
            var rest = new ManagerRESTService(new RESTService());
            var Accounts = new Accounts.Account();
            Debug.WriteLine("Clicked transaction button");
            var token = new Token();
            var uri = String.Format(Constants.MovementUrl, Accounts.bank_id, Accounts.id);
            try
            {
                await rest.GetWithToken(uri, 3, token.token);
                await Navigation.PushAsync(new transactionPage());
                await Navigation.PopAsync();
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }

        //should take the user to the cards page
        private async void OncardsButtonClicked(object sender, EventArgs e)
        {
            var rest = new ManagerRESTService(new RESTService());
            Debug.WriteLine("Clicked cards button");
            var token = new Token();
            var uri = String.Format(Constants.BranchesUrl);
            try
            {
                await rest.GetWithToken(uri, 4, token.token);
                await Navigation.PushAsync(new cardPage());
                await Navigation.PopAsync();
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }
    }
}