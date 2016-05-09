using Xamarin.Forms;

namespace App1.Layout
{
    internal class BaseContentPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!App.UserLoggedIn)
            {
                Navigation.PushModalAsync(new LoginPage());
            }
        }
    }
}