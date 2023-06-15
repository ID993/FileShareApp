using System.ComponentModel.DataAnnotations;

namespace Make_a_Drop.Application.Models.Drop
{
    public class DownloadModel : BaseResponseModel
    {
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string SecretKey { get; set; }

    }
}
