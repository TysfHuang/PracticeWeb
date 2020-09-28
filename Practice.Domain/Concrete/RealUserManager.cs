using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Practice.Domain.Abstract;
using Practice.Domain.Entities;

namespace Practice.Domain.Concrete
{
    public class RealUserManager : IUserManager
    {
        private AppUserManager userManager;
        public AppUserManager UserManager
        {
            get
            {
                if (userManager == null)
                    userManager = HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
                return userManager;
            }
        }

        public AppUser CurrentUser
        {
            get
            {
                return UserManager.FindByName(HttpContext.Current.User.Identity.Name);
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }

        public async Task<IdentityResult> UpdateAsync(AppUser user)
        {
            return await UserManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            return await UserManager.CreateAsync(user, password);
        }

        public async Task<AppUser> FindAsync(string userName, string password)
        {
            return await UserManager.FindAsync(userName, password);
        }

        public async Task<AppUser> FindByIdAsync(string userId)
        {
            return await UserManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> UserValidator_ValidateAsync(AppUser user)
        {
            return await UserManager.UserValidator.ValidateAsync(user);
        }

        public async Task<IdentityResult> PasswordValidator_ValidateAsync(string item)
        {
            return await UserManager.PasswordValidator.ValidateAsync(item);
        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(AppUser user, string authenticationType)
        {
            return await UserManager.CreateIdentityAsync(user, authenticationType);
        }

        public string PasswordHasher_HashPassword(string password)
        {
            return UserManager.PasswordHasher.HashPassword(password);
        }

        public bool IsInRole(string userId, string role)
        {
            return UserManager.IsInRole(userId, role);
        }
    }
}
