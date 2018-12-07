using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using Microsoft.AspNet.Identity;

namespace Inventory.BLL.Services
{
    public class AccountService : IAccountService
    {
        private const string DEFAULT_ROLE = "user";

        private IUnitOfWork _unitOfWork { get; set; }
        public AccountService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public async Task CreateUser(UserDTO userDTO)
        {
            ApplicationUser user = await _unitOfWork.UserManager.FindByEmailAsync(userDTO.UserName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = userDTO.UserName, Email = userDTO.Email };
                var result = await _unitOfWork.UserManager.CreateAsync(user, userDTO.Password);
                if (result.Succeeded)
                {
                    await _unitOfWork.UserManager.AddToRoleAsync(user.Id, (userDTO.Role ?? DEFAULT_ROLE));
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    if (result.Errors.Contains($"Name {user.UserName} is already taken."))
                        throw new UserAlreadyExistsException();
                    else if (result.Errors.Any(x => x.Contains("Password")))
                        throw new InsecurePasswordException();
                    else if (result.Errors.Count() > 0)
                        throw new System.Exception("Something went wrong.");
                }
            }
        }

        public async Task<ClaimsIdentity> AuthenticateUser(UserDTO userDTO)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await _unitOfWork.UserManager.FindAsync(userDTO.UserName, userDTO.Password);
            if (user != null)
                claim = await _unitOfWork.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            return claim;
        }

        public async Task<UserDTO> GetUser(string id)
        {
            ApplicationUser user = await _unitOfWork.UserManager.FindByIdAsync(id.ToString());

            return Mapper.Map<UserDTO>(user);
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            var roles = _unitOfWork.RoleManager.Roles.ToList();

            return Mapper.Map<IEnumerable<RoleDTO>>(roles);
        }

        public async Task UpdateEmail(UserDTO userDTO)
        {
            ApplicationUser user = await _unitOfWork.UserManager.FindByIdAsync(userDTO.Id.ToString());
            user.Email = userDTO.Email;
            await _unitOfWork.UserManager.UpdateAsync(user);

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdatePassword(ChangePasswordDTO changePasswordDTO)
        {
            ApplicationUser user = await _unitOfWork.UserManager.FindByIdAsync(changePasswordDTO.UserId);
            var oldPasswordConfirmation = await _unitOfWork.UserManager.CheckPasswordAsync(user, changePasswordDTO.OldPassword);
            if (!oldPasswordConfirmation)
                throw new OldPasswordIsWrongException();

            IdentityResult result = await _unitOfWork.UserManager.ChangePasswordAsync(
                changePasswordDTO.UserId,
                changePasswordDTO.OldPassword,
                changePasswordDTO.NewPassword);

            if (result.Errors.Any(error => error.Contains("Password")))
                throw new InsecurePasswordException();

            await _unitOfWork.SaveAsync();
        }
    }
}
