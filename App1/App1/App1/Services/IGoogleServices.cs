using App1.Models;
using App1.REST;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App1.Services
{
    public interface IGoogleServices
    {
        //Change Users not this
        Task<List<Users>> GetCoordinatesMapAsync(Location location);
    }
}