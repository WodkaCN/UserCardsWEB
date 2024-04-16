using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NickBuhro.Translit;
using System.ComponentModel.DataAnnotations;
using UserCardsAPI.Models.DB.Entity;
using UserCardsAPI.Models.DB.EntityEx;

namespace UserCardsAPI.Controllers
{
    [Route("api/HoldersController")]
    [ApiController]
    public class HoldersController : ControllerBase
    {
        [Route("GetHolder")]
        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult GetHolder([FromHeader, Required] string UID)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, JsonConvert.SerializeObject(DTOCardHolder.GetCardHoldersList(UID)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Route("AddHolder")]
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult AddHolder([FromBody, Required] CardHolder cardHolder)
        {
            try
            {
                cardHolder.Uid = Guid.NewGuid().ToString().ToUpper();
                DTOCardHolder.AddCardHolder(cardHolder);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return StatusCode(StatusCodes.Status200OK, cardHolder.Uid);
        }

        [Route("DeleteHolder")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult DeleteHolder([FromHeader, Required] string uid)
        {
            try
            {
                if (DTOCardHolder.GetCardHoldersList(uid).Count == 0)
                    return StatusCode(StatusCodes.Status404NotFound);

                foreach (var card in DTOCardsInfo.GetCardsInfoList(uid))
                    DTOCardsInfo.DeleteCardsInfo((long)card.Id);

                DTOCardHolder.DeleteCardHolder(uid);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return StatusCode(StatusCodes.Status200OK);
        }

        [Route("UpdateHolder")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult UpdateHolder([FromBody, Required] CardHolder cardHolder)
        {
            try
            {
                if (DTOCardHolder.GetCardHoldersList(cardHolder.Uid).Count == 0)
                    return StatusCode(StatusCodes.Status404NotFound);

                var fioLatin = Transliteration.CyrillicToLatin($"{cardHolder.F} {cardHolder.I} {cardHolder.O}");

                DTOCardHolder.UpdateCardHolder(cardHolder);

                foreach (var card in DTOCardsInfo.GetCardsInfoList(cardHolder.Uid))
                {
                    card.Fiolat = fioLatin;
                    DTOCardsInfo.UpdateCardsInfo(card);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return StatusCode(StatusCodes.Status200OK);
        }
    }
}