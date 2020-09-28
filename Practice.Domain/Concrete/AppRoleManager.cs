using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Practice.Domain.Entities;

namespace Practice.Domain.Concrete
{
    public class AppRoleManager : RoleManager<AppRole>, IDisposable
    {

        public AppRoleManager(RoleStore<AppRole> store)
            : base(store)
        {
        }

        //對每個請求都創建一個實例
        public static AppRoleManager Create(
                IdentityFactoryOptions<AppRoleManager> options,
                IOwinContext context)
        {
            return new AppRoleManager(new
                RoleStore<AppRole>(context.Get<AppIdentityDbContext>()));
        }
    }
}
