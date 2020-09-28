using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Practice.Domain.Concrete;
using Microsoft.Owin.Security.Google;

namespace PracticeWeb.WebUI
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            //對每個request都各創建一個實體(AppUserManager、AppIdentityDbContext、AppRoleManager)
            //這確保每個request都clean access to Identity，且不用擔心同步問題或是
            //較差的緩存資料庫問題
            app.CreatePerOwinContext<AppIdentityDbContext>(AppIdentityDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);

            //告訴identity如何用cookie來認證使用者身分
            //LoginPath為當使用者需要認證時會被重定向到該位址
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "",
                ClientSecret = ""
            });
        }
    }
}