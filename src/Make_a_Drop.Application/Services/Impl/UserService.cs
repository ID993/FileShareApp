using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Make_a_Drop.Application.Exceptions;
using Make_a_Drop.Application.Models.User;
using Make_a_Drop.Core.Entities.Identity;

namespace Make_a_Drop.Application.Services.Impl;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITemplateService _templateService;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(IMapper mapper,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        ITemplateService templateService,
        IEmailService emailService)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _templateService = templateService;
        _emailService = emailService;
    }

    public async Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel)
    {
        var user = _mapper.Map<ApplicationUser>(createUserModel);

        user.EmailConfirmed = true;

        var result = await _userManager.CreateAsync(user, createUserModel.Password);

        if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);

        return new CreateUserResponseModel
        {
            Id = Guid.Parse((await _userManager.FindByNameAsync(user.UserName)).Id)
        };
    }

    public async Task<List<ApplicationUser>> GetAllAsync()
    {
       return await _userManager.Users.ToListAsync();
    }

    public async Task<UserModel> GetByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user == null ? throw new NotFoundException($"User not found.") : _mapper.Map<UserModel>(user);
    }

    public async Task DeleteAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString()) ?? 
            throw new NotFoundException($"User not found.");
        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);
    }

    public async Task<UpdateModel> UpdateAsync(UpdateModel user)
    {
        var userToUpdate = await _userManager.FindByIdAsync(user.Id.ToString());
        userToUpdate.FirstName = user.FirstName ?? userToUpdate.FirstName;
        userToUpdate.LastName = user.LastName ?? userToUpdate.LastName;
        userToUpdate.UserName = user.UserName ?? userToUpdate.UserName;
        userToUpdate.Email = user.Email ?? userToUpdate.Email;
        var result = await _userManager.UpdateAsync(userToUpdate);
        
        if (!result.Succeeded)
            throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);

        return new UpdateModel
        {
            Id = Guid.Parse(userToUpdate.Id)
        };
    }

    public async Task<UpdateModel> ChangePasswordAsync(string userId, ChangePasswordModel changePasswordModel)
    {
        var user =
            await _userManager.FindByIdAsync(userId) ?? throw new NotFoundException($"User not found.");
        var result =
            await _userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword,
                changePasswordModel.NewPassword);

        if (!result.Succeeded)
            throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);

        return new UpdateModel
        {
            Id = Guid.Parse(user.Id)
        };
    }

}
