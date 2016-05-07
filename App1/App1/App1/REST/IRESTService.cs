using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;

namespace App1.REST
{
    public interface IRESTService
    {
        Task<List<AccountInfo>> RefreshDataAsync();

        Task SaveInfoAsync(AccountInfo item, bool isNewItem);

        Task DeleteInfoAsync(string id);

        Task<string> CreateSession(string user, string pass);

        Task<JsonValue> NewSession();

        Task<JsonValue> UserInContactPage();
    }
}