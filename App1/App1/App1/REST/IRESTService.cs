using App1.Models;
using System.Threading.Tasks;

namespace App1.REST
{
    public interface IRESTService
    {
        Task<bool> CreateSession(Users user, Users pass);

        Task<bool> MakePayment(Payments.To accountTo, Payments.To bankTo, Payments.Value currencyTo, Payments.Value amountTo, Payments.Body descriptionTo);

        Task<T> GetwithoutToken<T>(string url);

        Task<string> GetWithToken(string url);

        bool IsAutheticated { get; }
    }
}