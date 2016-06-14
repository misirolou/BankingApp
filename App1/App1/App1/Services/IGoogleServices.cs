using App1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.Services
{
    public interface IGoogleServices
    {
        Task<List<atmlist>> GetCoordinatesMapAsync(Locationatm latitude, Locationatm longitude);
    }
}