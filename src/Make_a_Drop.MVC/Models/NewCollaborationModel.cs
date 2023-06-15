using System.ComponentModel.DataAnnotations;

namespace Make_a_Drop.MVC.Models
{
    public class NewCollaborationModel
    {
        public string? Users { get; set; }
        [Required]
        public string? Name { get; set; }

        
    }
}
