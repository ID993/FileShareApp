using Make_a_Drop.Application.Models.Collaboration;

namespace Make_a_Drop.Application.Models.User
{
    public class UserModel : BaseResponseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<CollaborationModel> Collaborations { get; set; }

    }
}
