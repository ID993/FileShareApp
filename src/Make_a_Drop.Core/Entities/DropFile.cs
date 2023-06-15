using Make_a_Drop.Core.Common;

namespace Make_a_Drop.Core.Entities
{
    public class DropFile : BaseEntity
    {
        public string FileName { get; set; }

        public long Size { get; set; }

        public Drop Drop { get; set; }
    }
}
