using System.ComponentModel.DataAnnotations;

namespace UserCardsAPI.Models.DB.Entity
{
    public partial class CardsInfo
    {
        public long? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Отсутствует UID")]
        public string? Uid { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Отсутствует ФИО в латинице")]
        public string? Fiolat { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Отсутствует Номер карты")]
        public string? CardNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Отсутствует Номер счета")]
        public string? CardAccount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Отсутствует Дата истечения строка карты")]
        public DateTime Expire { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Отсутствует CVC-код")]
        public string? Cvc { get; set; }
    }
}
