using Microsoft.AspNet.Identity.EntityFramework;

namespace Practice.Domain.Entities
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string name) : base(name) { }
    }
}
