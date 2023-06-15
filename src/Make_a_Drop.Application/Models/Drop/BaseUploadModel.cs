using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Make_a_Drop.Application.Models.Drop
{
    public class BaseUploadModel : BaseResponseModel
    {
        [Required]
        [DataType(DataType.Upload)]
        [MinLength(1)]
        [MaxLength(10)]
        public IFormFile[] File { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string SecretKey { get; set; }

    }
}
