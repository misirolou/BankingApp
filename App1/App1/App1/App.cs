using App1.Layout;
using App1.REST;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public class App : Application
    {
        private Dictionary<FirstPage, NavigationPage> Pages { get; set; }
        public static ITextSpeech Speech { get; set; }

        private readonly IRESTService AuthenticationService;

        //the main Application and its functionalities
        public App()
        {
            AuthenticationService = DependencyService.Get<IRESTService>();
            var rest = new ManagerRESTService(new RESTService());
            MainPage = new NavigationPage(new LoginPage());
            // NavigateAsync(FirstPage.Login);
            Debug.WriteLine("App testing userloggedIn");
            try
            {
                if (rest.IsAutheticated())
                {
                    Debug.WriteLine("authentication {0}", AuthenticationService.IsAutheticated);
                    Debug.WriteLine("userloggedIn is true");
                    MainPage = new NavigationPage(new PrincipalPage());
                }
                else
                {
                    Debug.WriteLine("authentication {0}", AuthenticationService.IsAutheticated);
                    Debug.WriteLine("userloggedIn is false");
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }

        //  Pages = new Dictionary<FirstPage, NavigationPage>();

        private void SetDetailIfNull(Page page)
        {
            if (Detail == null && page != null)
                Detail = page;
        }

        public async Task NavigateAsync(FirstPage id)
        {
            Page newPage;
            if (!Pages.ContainsKey(id))
            {
                switch (id)
                {
                    case FirstPage.Login:
                        var page = new NavigationPage(new LoginPage()
                        {
                            Title = "Login",
                            Icon = new FileImageSource { File = "robot.png" }
                        });
                        SetDetailIfNull(page);
                        Pages.Add(id, page);
                        break;

                    case FirstPage.Contacts:
                        page = new NavigationPage(new ContactPage()
                        {
                            Title = "Contacts",
                            Icon = new FileImageSource { File = "robot.png" }
                        });
                        SetDetailIfNull(page);
                        Pages.Add(id, page);
                        break;

                    case FirstPage.Bank:
                        page = new NavigationPage(new BalcaoPage()
                        {
                            Title = "Bank Localization",
                            Icon = new FileImageSource { File = "robot.png" }
                        });
                        SetDetailIfNull(page);
                        Pages.Add(id, page);
                        break;

                    case FirstPage.Atm:
                        page = new NavigationPage(new AtmPage()
                        {
                            Title = "ATM localization",
                            Icon = new FileImageSource { File = "robot.png" },
                        });
                        SetDetailIfNull(page);
                        Pages.Add(id, page);
                        break;

                    case FirstPage.MainPage:
                        page = new NavigationPage(new PrincipalPage()
                        {
                            Title = "Main",
                            Icon = new FileImageSource { File = "robot.png" },
                        });
                        SetDetailIfNull(page);
                        Pages.Add(id, page);
                        break;

                    case FirstPage.Cards:
                        page = new NavigationPage(new cardPage()
                        {
                            Title = "Cards",
                            Icon = new FileImageSource { File = "robot.png" },
                        });
                        SetDetailIfNull(page);
                        Pages.Add(id, page);
                        break;

                    case FirstPage.transaction:
                        page = new NavigationPage(new transactionPage()
                        {
                            Title = "Transactions",
                            Icon = new FileImageSource { File = "robot.png" },
                        });
                        SetDetailIfNull(page);
                        Pages.Add(id, page);
                        break;

                    case FirstPage.Products:
                        page = new NavigationPage(new ProductPage()
                        {
                            Title = "Products",
                            Icon = new FileImageSource { File = "robot.png" },
                        });
                        SetDetailIfNull(page);
                        Pages.Add(id, page);
                        break;
                }
            }
            newPage = Pages[id];
            if (newPage == null)
                return;

            //pop to root for Windows Phone
            if (Detail != null && Device.OS == TargetPlatform.WinPhone)
            {
                await Detail.Navigation.PopToRootAsync();
            }

            Detail = newPage;
        }

        public enum FirstPage
        {
            Login,
            Contacts,
            Bank,
            Atm,
            MainPage,
            Products,
            Cards,
            transaction
        }

        public Page Detail { get; set; }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}