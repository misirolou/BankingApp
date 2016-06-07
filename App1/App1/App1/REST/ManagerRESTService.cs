using System.Threading.Tasks;

namespace App1.REST
{
    public class ManagerRESTService
    {
        private IRESTService restService;

        public ManagerRESTService(IRESTService service)
        {
            restService = service;
        }

        public async Task<T> GetwithoutToken<T>(string url)
        {
            return await restService.GetwithoutToken<T>(url);
        }

        public async Task<bool> CreateSession(Users user, Users pass)
        {
            return await restService.CreateSession(user, pass);
        }

        public async Task<string> GetWithToken(string url)
        {
            return await restService.GetWithToken(url);
        }

        public async Task<T> GetWithToken<T>(string url)
        {
            return await restService.GetWithToken<T>(url);
        }

        public bool IsAutheticated()
        {
            return restService.IsAutheticated;
        }
    }
}