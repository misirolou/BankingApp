using Xamarin.Forms;

namespace App1.Layout
{
    //this page isnt called but was used to experiment animations and trying out an inicial splash page
    public class SplashPage : ContentPage
    {
        public SplashPage()
        {
            Button button = new Button();
            button.Clicked += (sender, args) =>
            {
                var width = Application.Current.MainPage.Width;

                var playingaround = new Animation();
                //rotates the button image once pressed
                var rotation = new Animation(callback: d => button.Rotation = d,
                                              start: button.Rotation,
                                              end: button.Rotation + 360,
                                              easing: Easing.SpringOut);

                //the image exits to the right
                var exitRight = new Animation(callback: d => button.TranslationX = d,
                                               start: 0,
                                               end: width,
                                               easing: Easing.SpringIn);

                //the image returns from the left
                var enterLeft = new Animation(callback: d => button.TranslationX = d,
                                               start: -width,
                                               end: 0,
                                               easing: Easing.BounceOut);

                playingaround.Add(0, 1, rotation);
                playingaround.Add(0, 0.5, exitRight);
                playingaround.Add(0.5, 1, enterLeft);

                playingaround.Commit(button, "Loop", length: 1400);
            };

            //Layout of the Home page(PrincipalPage.cs)
            Title = "Loading Screen";
            Icon = new FileImageSource() { File = "robot.png" };
            NavigationPage.SetBackButtonTitle(this, "go back");
            //this is the type of layout the grids will be specified in
            var stackLayout = new StackLayout
            {
                //Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Teal,
                Spacing = 10,
                Padding = 1,
                Children =
                {
                    button,
                    new Label() {Text = "click to see what happens", BackgroundColor = Color.Gray}
                }
            };
            this.Content = stackLayout;
        }
    }
}