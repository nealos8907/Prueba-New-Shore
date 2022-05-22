using Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IHandleNewShoreDbRepository
    {
        Task<bool> SaveRoute(JourneyDto journey);

        Task<JourneyDto> GetRoute(RequestJourneyDto requestJourney);
    }
}
