using Microsoft.AspNetCore.Identity;

namespace BlueCraeftBowl.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = false;
    public bool IsAdmin { get; set; } = false;
    public virtual ICollection<UserSelection> Selections { get; set; } = new List<UserSelection>();
}
