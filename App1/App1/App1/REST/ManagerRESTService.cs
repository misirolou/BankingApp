using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.REST
{
    internal class ManagerRESTService
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
    }
}