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

        public async Task<bool> GetWithToken(string url, int choice)
        {
            return await restService.GetWithToken(url, choice);
        }

        public async Task<T> getResponse<T>(string url)
        {
            return await restService.getResponse<T>(url);
        }

        public bool IsAutheticated()
        {
            return restService.IsAutheticated;
        }
    }
}