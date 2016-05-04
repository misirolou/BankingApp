using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.REST
{
    internal interface IRESTService
    {
        Task<List<AccountInfo>> RefreshDataAsync();

        Task SaveInfoAsync(AccountInfo item, bool isNewItem);

        Task DeleteInfoAsync(string id);

    }
}