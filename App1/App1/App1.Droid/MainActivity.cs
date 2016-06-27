using Android.App;
using Android.Content.PM;
using Android.OS;
using Syncfusion.SfChart.XForms.Droid;

namespace App1.Droid
{
    [Activity(Label = "App1", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.FormsMaps.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            new SfChartRenderer();
            //App.Speech = new Speech();
            LoadApplication(new App());
        }
    }
}