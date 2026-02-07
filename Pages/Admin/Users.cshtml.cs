using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlueCraeftBowl.Data;
using BlueCraeftBowl.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlueCraeftBowl.Pages.Admin;

public class UsersModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public List<ApplicationUser> Users { get; set; } = new();

    public async Task OnGetAsync()
    {
        Users = await _context.Users.ToListAsync();
    }

    public async Task<IActionResult> OnPostToggleActiveAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            // Prevent deactivating the default admin
            if (user.Email == "admin@bluecraeftbowl.com")
            {
                return RedirectToPage();
            }

            user.IsActive = !user.IsActive;
            await _userManager.UpdateAsync(user);
            
            // Update claims for immediate effect if possible, but usually requires re-login
            var claims = await _userManager.GetClaimsAsync(user);
            var activeClaim = claims.FirstOrDefault(c => c.Type == "IsActive");
            if (activeClaim != null) await _userManager.RemoveClaimAsync(user, activeClaim);
            await _userManager.AddClaimAsync(user, new Claim("IsActive", user.IsActive.ToString()));
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostToggleAdminAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.IsAdmin = !user.IsAdmin;
            await _userManager.UpdateAsync(user);
            
            var claims = await _userManager.GetClaimsAsync(user);
            var adminClaim = claims.FirstOrDefault(c => c.Type == "IsAdmin");
            if (adminClaim != null) await _userManager.RemoveClaimAsync(user, adminClaim);
            await _userManager.AddClaimAsync(user, new Claim("IsAdmin", user.IsAdmin.ToString()));
        }
        return RedirectToPage();
    }
}
