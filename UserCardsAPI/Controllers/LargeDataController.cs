using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using UserCardsAPI.Models.DB.EntityEx;

namespace UserCardsAPI.Controllers
{
    [Route("api/LargeDataController")]
    [ApiController]
    public class LargeDataController : ControllerBase
    {
        [Route("GetHoldersDataPage")]
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult GetHoldersDataPage([FromHeader, Required] int pageNumber, [FromHeader, Required] int pageRowsNumber)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK,
                    JsonConvert.SerializeObject(DTOCardHolder.GetCardHoldersList()
                                                            .Skip(pageRowsNumber * pageNumber)
                                                            .Take(pageRowsNumber)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("GetHoldersPagesCount")]
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult GetHoldersPagesCount([FromHeader, Required] int pageRowsNumber)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, (DTOCardHolder.GetCardHoldersList().Count / pageRowsNumber).ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
