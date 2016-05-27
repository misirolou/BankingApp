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

        public async Task<string> GetwithoutToken(string url, int choice)
        {
            return await restService.GetwithoutToken(url, choice);
        }

        public async Task<bool> CreateSession(Users user, Users pass)
        {
            return await restService.CreateSession(user, pass);
        }

        public async Task<bool> GetWithToken(string url, int choice, string token)
        {
            return await restService.GetWithToken(url, choice, token);
        }

        public bool IsAutheticated()
        {
            return restService.IsAutheticated;
        }
    }
}