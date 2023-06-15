using Make_a_Drop.Core.Common;

namespace Make_a_Drop.Application.Models.DropFile
{
    public class DropFileModel : BaseEntity
    {
        public string FileName { get; set; }

        public long Size { get; set; }
    }
}
