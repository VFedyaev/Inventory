using AutoMapper;
using Inventory.BLL.DTO;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.BLL.Services
{
    public class UserService : IUserService
    {
        private const string DEFAULT_ROLE = "user";

        private IUnitOfWork _unitOfWork { get; set; }

        public UserService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            List<ApplicationUser> users = _unitOfWork.UserManager.Users.ToList();

            return Mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<string> GetUserRole(string userId)
        {
            IList<string> roles = await _unitOfWork.UserManager.GetRolesAsync(userId);
            string roleName = roles.FirstOrDefault();

            return roleName;
        }

        public async Task ChangeUserRole(ChangeRoleDTO changeRoleDTO)
        {
            if (!string.IsNullOrEmpty(changeRoleDTO.OldRole))
                await _unitOfWork.UserManager.RemoveFromRoleAsync(changeRoleDTO.UserId, changeRoleDTO.OldRole);

            if (!string.IsNullOrEmpty(changeRoleDTO.Role))
                await _unitOfWork.UserManager.AddToRoleAsync(changeRoleDTO.UserId, changeRoleDTO.Role);

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException();

            var user = await _unitOfWork.UserManager.FindByIdAsync(userId);
            var userRoles = await _unitOfWork.UserManager.GetRolesAsync(userId);

            if (userRoles.Count() > 0)
                foreach (var role in userRoles)
                    await _unitOfWork.UserManager.RemoveFromRoleAsync(userId, role);

            await _unitOfWork.UserManager.DeleteAsync(user);
            await _unitOfWork.SaveAsync();
        }
    }
}
