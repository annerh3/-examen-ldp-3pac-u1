using Microsoft.AspNetCore.Identity;

namespace BlogUNAH.API.Helpers
{
    public class Employee : IdentityUser
    {
        public string employee_name {  get; set; }
        public DateOnly date_entry { get; set; }
    }
}
