using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.REST
{
    public interface IRESTService
    {
        Task<List<AccountInfo>> RefreshDataAsync();

        Task SaveInfoAsync(AccountInfo item, bool isNewItem);

        Task DeleteInfoAsync(string id);

        Task<object> CreateSession(AccountInfo item);
    }
}