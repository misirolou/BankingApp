using System.Threading.Tasks;

namespace App1.REST
{
    public interface IRESTService
    {
        Task<bool> CreateSession(Users user, Users pass);

        Task<T> GetwithoutToken<T>(string url);

        Task<T> GetWithToken<T>(string url);

        Task<string> GetWithToken(string url);

        bool IsAutheticated { get; }
    }
}