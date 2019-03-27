using Microsoft.AspNet.Identity.EntityFramework;

namespace EmployeeWebAPI.Models
{
    public partial class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("AuthContext", throwIfV1Schema: false)
        {
        }

        public static AuthContext Create()
        {
            return new AuthContext();
        }
    }
}