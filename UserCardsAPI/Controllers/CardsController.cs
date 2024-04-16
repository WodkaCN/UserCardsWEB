using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NickBuhro.Translit;
using System.ComponentModel.DataAnnotations;
using UserCardsAPI.Models.DB.Entity;
using UserCardsAPI.Models.DB.EntityEx;

namespace UserCardsAPI.Controllers
{
    [Route("api/CardsController")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        [Route("GetHolderCards")]
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult GetCards([FromHeader, Required] string UID)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(DTOCardsInfo.GetCardsInfoList(UID)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("AddCard")]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult AddCard([FromBody, Required] CardsInfo cardInfo)
        {
            try
            {
                if (DTOCardHolder.GetCardHoldersList(cardInfo.Uid).Count == 0)
                    return StatusCode(StatusCodes.Status404NotFound);

                var holderLatName = DTOCardsInfo.GetCardsInfoList(cardInfo.Uid).FirstOrDefault();

                cardInfo.Fiolat = holderLatName.Fiolat;
                DTOCardsInfo.AddCardsInfo(cardInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return StatusCode(StatusCodes.Status200OK, $"{cardInfo.Uid} || CardID:{cardInfo.Id}");
        }

        [Route("DeleteCard")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult DeleteCard([FromHeader, Required] long id)
        {
            try
            {
                if (DTOCardsInfo.GetCardsInfoList(null , id).Count == 0)
                    return StatusCode(StatusCodes.Status404NotFound);

                DTOCardsInfo.DeleteCardsInfo(id);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("UpdateCard")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult UpdateCard([FromBody, Required] CardsInfo cardInfo)
        {
            try
            {
                if (DTOCardsInfo.GetCardsInfoList(null, cardInfo.Id).Count == 0)
                    return StatusCode(StatusCodes.Status404NotFound);

                DTOCardsInfo.UpdateCardsInfo(cardInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
