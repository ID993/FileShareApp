using Make_a_Drop.Application.Models.User;
using Make_a_Drop.Core.Entities.Identity;

namespace Make_a_Drop.Application.Services;

public interface IUserService
{
    Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel);
    Task<List<ApplicationUser>> GetAllAsync();
    Task<UserModel> GetByIdAsync(string userId);
    Task DeleteAsync(string userId);
    Task<UpdateModel> UpdateAsync(UpdateModel user);
    Task<UpdateModel> ChangePasswordAsync(string userId, ChangePasswordModel changePasswordModel);
 
}
