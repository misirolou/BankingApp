using System.Threading.Tasks;

namespace App1.REST
{
    public interface IRESTService
    {
        Task<bool> CreateSession(Users user, Users pass);

        Task<T> GetwithoutToken<T>(string url);

        Task<bool> GetWithToken(string url, int choice);

        Task<T> getResponse<T>(string url);

        bool IsAutheticated { get; }
    }
}