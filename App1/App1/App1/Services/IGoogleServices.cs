using App1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.Services
{
    public interface IGoogleServices
    {
        //Change Users not this
        Task<List<atmlist>> GetCoordinatesMapAsync(Locationatm latitude, Locationatm longitude);
    }
}