using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserCardsAPI.Models.DB.Entity
{
    public partial class CardHolder
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Отсутствует UID")]
        public string Uid { get; set; } = null!;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Отсутствует Фамилия")]
        public string? F { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Отсутствует Имя")]
        public string? I { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string? O { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Отсутствует Дата рождения")]
        public DateTime BirthDate { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string? WorkPlace { get; set; }
    }
}
