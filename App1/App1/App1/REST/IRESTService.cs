using System.Threading.Tasks;

namespace App1.REST
{
    public interface IRESTService
    {
        Task<bool> CreateSession(Users user, Users pass);

        Task<bool> GetwithoutToken(string url, int choice);

        Task<bool> GetWithToken(string url, int choice);

        bool IsAutheticated { get; }
    }
}