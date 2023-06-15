using Make_a_Drop.Core.Common;
using Make_a_Drop.Core.Entities.Identity;

namespace Make_a_Drop.Core.Entities
{
    public class Comment : BaseEntity, IAuditedEntity
    {
        public string Text { get; set; }
        public Drop Drop { get; set; }
        public ApplicationUser User { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
