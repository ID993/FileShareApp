using System.ComponentModel.DataAnnotations;

namespace Make_a_Drop.MVC.Models
{
    public class CollaborationAddUsersModel
    {
        [Required]
        public Guid CollaborationId { get; set; }
        [Required]
        public string Users { get; set; }
    }
}
