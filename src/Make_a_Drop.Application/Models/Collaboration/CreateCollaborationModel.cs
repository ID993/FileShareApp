using System.ComponentModel.DataAnnotations;

namespace Make_a_Drop.Application.Models.Collaboration
{
    public class CreateCollaborationModel
    {
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Users { get; set; }
    }
}
