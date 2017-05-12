using Microsoft.AspNet.Identity.EntityFramework;

namespace Reusable.Auth
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext() : base("CAPAINVConn") { }
    }
}