using Microsoft.AspNetCore.Identity;

namespace wSHSApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AkusUser class
public class AkusUser : IdentityUser
{
    public string Firstname { get; set; }
    public string Surname { get; set; }

}

