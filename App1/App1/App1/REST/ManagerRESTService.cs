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

        public async Task<string> CreateSession(Users user, Users pass)
        {
            return await restService.CreateSession(user, pass);
        }

        public async Task<string> GetWithToken(string url, int choice)
        {
            return await restService.GetWithToken(url, choice);
        }
    }
}