using System.Collections.Generic;
using System.Json;
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

        public ManagerRESTService()
        {
        }

        public Task<List<AccountInfo>> GetTasksAsync()
        {
            return restService.RefreshDataAsync();
        }

        public Task SaveTaskAsync(AccountInfo item, bool isNewItem = false)
        {
            return restService.SaveInfoAsync(item, isNewItem);
        }

        public Task DeleteTaskAsync(AccountInfo item)
        {
            return restService.DeleteInfoAsync(item.AccountId);
        }

        public Task<string> CreateSession(string user, string pass)
        {
            return restService.CreateSession(user, pass);
        }

        public async Task<JsonValue> NewSession()
        {
            return await restService.NewSession();
        }

        public async Task<JsonValue> UserInContactPage()
        {
            return await restService.UserInContactPage();
        }
    }
}