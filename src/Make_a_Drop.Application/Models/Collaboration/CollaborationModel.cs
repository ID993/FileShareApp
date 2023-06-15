using Make_a_Drop.Core.Common;
using Make_a_Drop.Core.Entities.Identity;

namespace Make_a_Drop.Application.Models.Collaboration
{
    public class CollaborationModel : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public string OwnerId { get; set; }
        public ApplicationUser Owner { get; set; }
        public ICollection<Make_a_Drop.Core.Entities.Drop> Drops { get; set; }
    }
}
