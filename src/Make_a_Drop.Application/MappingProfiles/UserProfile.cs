using AutoMapper;
using Make_a_Drop.Application.Models.User;
using Make_a_Drop.Core.Entities.Identity;

namespace Make_a_Drop.Application.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserModel, ApplicationUser>();
        CreateMap<ApplicationUser, CreateUserModel>();
        CreateMap<ApplicationUser, UpdateModel>();
        CreateMap<UpdateModel, ApplicationUser>();
        CreateMap<ApplicationUser, UserModel>();
        CreateMap<UserModel, ApplicationUser>();
        CreateMap<UserModel, UpdateModel>();
        CreateMap<UpdateModel, UserModel>();
    }
}
