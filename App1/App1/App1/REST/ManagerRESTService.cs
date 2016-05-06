using System.Collections.Generic;
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

        public Task CreateSession(AccountInfo item)
        {
            return restService.CreateSession(item.token);
        }
    }
}