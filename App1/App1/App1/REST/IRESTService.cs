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

        Task<string> NewSession();

        Task<JsonValue> UserInContactPage();

        Task<string> GetData(string id);

        Task<string> RegisterUserJsonRequest(Users user);

        Task<string> RegisterUserFormRequest(Users user);
    }
}