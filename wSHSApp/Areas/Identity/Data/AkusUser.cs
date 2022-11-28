using Microsoft.AspNetCore.Identity;

namespace wSHSApp.Areas.Identity.Data;

public class AkusUser : IdentityUser
{
    public string? Firstname { get; set; }
    public string? Surname { get; set; }

}

