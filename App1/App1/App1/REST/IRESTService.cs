using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.REST
{
    interface IRESTService
    {
        Task<List<AccountInfo>> RefreshDataAsync();

        Task SaveInfoAsync(AccountInfo item, bool isNewItem);

        Task DeleteInfoAsync(string id);
    }
}
