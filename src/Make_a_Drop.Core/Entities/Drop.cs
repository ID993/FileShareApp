using Make_a_Drop.Core.Common;
using Make_a_Drop.Core.Entities.Identity;

namespace Make_a_Drop.Core.Entities
{
    public class Drop : BaseEntity, IAuditedEntity
    {
        public string Name { get; set; }
        public string SecretKey { get; set; }
        public long Size { get; set; }
        public ApplicationUser User { get; set; }
        #nullable enable
        public Collaboration? Collaboration { get; set; }
        #nullable disable
        public ICollection<DropFile> DropFiles { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string CreatedBy { get ; set ; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
