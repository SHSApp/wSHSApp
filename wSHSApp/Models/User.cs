using System.Collections.Generic;

namespace wSHSApp.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string? RoleId { get; set; }
        public Role Role { get; set; }
    }
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
        public Role()
        {
            Users = new List<User>();
        }
    }
}
