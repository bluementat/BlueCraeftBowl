using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlueCraeftBowl.Data;
using BlueCraeftBowl.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BlueCraeftBowl.Pages.Parlays;

[Authorize(Policy = "ActiveUser")]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public ParlaySession? Session { get; set; }
    public List<ParlayItem> Items { get; set; } = new();
    public List<UserSelection> UserSelections { get; set; } = new();

    [BindProperty]
    public List<SelectionInput> Selections { get; set; } = new();

    public class SelectionInput
    {
        public int ParlayItemId { get; set; }
        public string SelectedValue { get; set; } = string.Empty;
    }

    public async Task OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return;

        Session = await _context.ParlaySessions.FirstOrDefaultAsync();
        Items = await _context.ParlayItems.ToListAsync();
        UserSelections = await _context.UserSelections
            .Where(s => s.UserId == user.Id)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToPage();

        var session = await _context.ParlaySessions.FirstOrDefaultAsync();
        if (session?.IsLocked == true) return RedirectToPage();

        foreach (var input in Selections)
        {
            var existing = await _context.UserSelections
                .FirstOrDefaultAsync(s => s.UserId == user.Id && s.ParlayItemId == input.ParlayItemId);

            if (existing != null)
            {
                existing.SelectedValue = input.SelectedValue;
            }
            else
            {
                _context.UserSelections.Add(new UserSelection
                {
                    UserId = user.Id,
                    ParlayItemId = input.ParlayItemId,
                    SelectedValue = input.SelectedValue
                });
            }
        }

        await _context.SaveChangesAsync();
        return RedirectToPage();
    }
}
