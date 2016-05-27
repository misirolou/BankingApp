using System.Threading.Tasks;

namespace App1.REST
{
    public interface IRESTService
    {
        Task<bool> CreateSession(Users user, Users pass);

        Task<string> GetwithoutToken(string url, int choice);

        Task<bool> GetWithToken(string url, int choice, string tokenreceived);

        bool IsAutheticated { get; }
    }
}