using App1.Layout;
using App1.REST;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public class App : Application
    {
        public static ITextSpeech Speech { get; set; }

        private readonly IRESTService AuthenticationService;

        //the main Application and its functionalities
        public App()
        {
            AuthenticationService = DependencyService.Get<IRESTService>();
            var rest = new ManagerRESTService(new RESTService());
            // MainPage = new NavigationPage(new LoginPage());
            // NavigateAsync(FirstPage.Login);
            Debug.WriteLine("App testing userloggedIn");
            try
            {
                if (rest.IsAutheticated())
                {
                    Debug.WriteLine("userloggedIn is true");
                    MainPage = new NavigationPage(new AccountsPage());
                }
                else
                {
                    Debug.WriteLine("userloggedIn is false");
                    MainPage = new NavigationPage(new LoginPage());
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
            }
        }

        private static Application app;

        public static Application CurrentApp
        {
            get { return app; }
        }

        //in case the user is not connected to the internet
        public static async Task ExecuteIfConnected(Func<Task> actionToExecuteIfConnected)
        {
            IsConnected = true;
            if (IsConnected)
            {
                await actionToExecuteIfConnected();
            }
            else
            {
                await ShowNetworkConnectionAlert();
            }
        }

        private static async Task ShowNetworkConnectionAlert()
        {
            await CurrentApp.MainPage.DisplayAlert(
                "Alert Network issues", "Connect to internet", "OK"
                );
        }

        public static bool IsConnected { get; set; }

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