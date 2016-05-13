using System.Threading.Tasks;

namespace App1.REST
{
    public interface IRESTService
    {
        Task<string> CreateSession(Users user, Users pass);

        Task<string> GetwithoutToken();

        Task<string> GetWithToken();
    }
}