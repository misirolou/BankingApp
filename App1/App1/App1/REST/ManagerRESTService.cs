using System.Threading.Tasks;
using App1.Models;

namespace App1.REST
{
    public class ManagerRESTService
    {
        private IRESTService restService;

        public ManagerRESTService(IRESTService service)
        {
            restService = service;
        }

        public async Task<string> GetwithoutToken()
        {
            return await restService.GetwithoutToken();
        }

        public async Task<string> CreateSession(Users user, Users pass)
        {
            return await restService.CreateSession(user, pass);
        }

        public async Task<string> GetWithToken()
        {
            return await restService.GetWithToken();
        }
    }
}