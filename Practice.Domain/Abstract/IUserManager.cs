using Practice.Domain.Entities;
using Practice.Domain.Concrete;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using System.Security.Claims;

namespace Practice.Domain.Abstract
{
    public interface IUserManager
    {
        AppUserManager UserManager { get; }
        AppUser CurrentUser { get; }
        IAuthenticationManager AuthManager { get; }
        bool IsAuthenticated { get; }
        Task<IdentityResult> UpdateAsync(AppUser user);
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        Task<AppUser> FindAsync(string userName, string password);
        Task<AppUser> FindByIdAsync(string userId);
        Task<IdentityResult> UserValidator_ValidateAsync(AppUser user);
        Task<IdentityResult> PasswordValidator_ValidateAsync(string item);
        Task<ClaimsIdentity> CreateIdentityAsync(AppUser user, string authenticationType);
        string PasswordHasher_HashPassword(string password);
        bool IsInRole(string userId, string role);
    }
}
