using Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repositories;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiNewshore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewShoreController : ControllerBase
    {
        private readonly INewShoreRepository _newShoreRepository;
        private readonly IHandleNewShoreDbRepository _handleNewShoreDbRepository;
        private readonly ILogger<NewShoreController> _logger;
        public NewShoreController(INewShoreRepository newShoreRepository, 
            IHandleNewShoreDbRepository handleNewShoreDbRepository,
            ILogger<NewShoreController> logger)
        {
            _newShoreRepository = newShoreRepository;
            _handleNewShoreDbRepository = handleNewShoreDbRepository;
            _logger = logger;
        }

        [HttpPost]
        [Route("GetRoute")]
        public async Task<IActionResult>  GetRoute([FromBody] RequestJourneyDto requestJourney )
        {
            var result = new JourneyDto();
            try
            {
                result = await _newShoreRepository.GetRoutesNewShore(requestJourney);
                if (result != null)
                {
                    return Ok(result);
                }
                
            }
            catch (System.Exception ex)
            {

                _logger.LogError($"Fecha:{DateTime.Now}- Error: {ex} -Parameters: {JsonConvert.SerializeObject(requestJourney)}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            //Codigo 204
            return BadRequest("VUELO NO ENCONTRADO");
            
        }

    }
}
