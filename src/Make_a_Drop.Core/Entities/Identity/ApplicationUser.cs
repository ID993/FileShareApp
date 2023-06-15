using Microsoft.AspNetCore.Identity;

namespace Make_a_Drop.Core.Entities.Identity;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        this.Collaborations = new HashSet<Collaboration>();
    }

    public string FirstName { get; set; } 
    public string LastName { get; set; }
    public ICollection<Drop> Drops { get; set; }
    public virtual ICollection<Collaboration> Collaborations { get; set; }
    public ICollection<Comment> Comments { get; set; }
}
