using System.Threading.Tasks;

namespace App1.REST
{
    public interface IRESTService
    {
        Task<bool> CreateSession(Users user, Users pass);

        Task<bool> MakePayment();

        Task<T> GetwithoutToken<T>(string url);

        Task<string> GetWithToken(string url);

        bool IsAutheticated { get; }
    }
}