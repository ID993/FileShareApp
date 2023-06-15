using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Make_a_Drop.Application.Models.Drop
{
    public class UploadModel : BaseResponseModel
    {
        [Required]
        [DataType(DataType.Upload)]
        public IFormFile[] File { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string SecretKey { get; set; }

        public Guid? CollaborationId { get; set; }
    }
}
