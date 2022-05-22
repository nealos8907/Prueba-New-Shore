using Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface INewShoreRepository
    {
        Task<JourneyDto> GetRoutesNewShore(RequestJourneyDto requestJourney);
    }
}
