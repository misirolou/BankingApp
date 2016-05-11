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

        public Task<string> CreateSession(string user, string pass)
        {
            return restService.CreateSession(user, pass);
        }

        public async Task<string> NewSession()
        {
            return await restService.NewSession();
        }

        public async Task<string> UserInContactPage()
        {
            return await restService.UserInContactPage();
        }
    }
}