using Make_a_Drop.Application.Models.DropFile;
using Make_a_Drop.Core.Common;
using Make_a_Drop.Core.Entities.Identity;

namespace Make_a_Drop.Application.Models.Drop
{
    public class DropModel : BaseEntity
    {

        public string Name { get; set; }

        public string DownloadUrl { get; set; }

        public long Size { get; set; }

        public string SecretKey { get; set; }


    }

    public class DropResponseModel : BaseResponseModel
    {
        public string Name { get; set; }

        public long Size { get; set; }

        public string SecretKey { get; set; }

        public DateTime ExpirationDate { get; set; }

        public ApplicationUser User { get; set; }

    }


}
