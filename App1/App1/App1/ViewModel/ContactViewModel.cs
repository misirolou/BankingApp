using App1.Models;
using App1.REST;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace App1.ViewModel
{
    internal class ContactViewModel : BaseViewModel
    {
        private async Task Test()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var rest = new ManagerRESTService(new RESTService());
                var uri = string.Format(Constants.BankUrl);
                var some = await rest.GetwithoutToken<Banklist>(uri);
                Debug.WriteLine("done request this is the response {0}", some);
                //var banklist = JsonConvert.DeserializeObject<Banklist>();
                foreach (var item in some.banks)
                {
                    Debug.WriteLine("itemid :{0}", item.id);
                }
                IsBusy = false;
                OnPropertyChanged();
            }
            catch (Exception err)
            {
                Debug.WriteLine("Caught error: {0}.", err);
                IsBusy = false;
            }
        }
    }
}