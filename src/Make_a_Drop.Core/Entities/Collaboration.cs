using Make_a_Drop.Core.Common;
using Make_a_Drop.Core.Entities.Identity;

namespace Make_a_Drop.Core.Entities
{
    public class Collaboration : BaseEntity, IAuditedEntity
    {
        public Collaboration()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        public string Name { get; set; }
        public string OwnerId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Drop> Drops { get; set; }
    }
}
