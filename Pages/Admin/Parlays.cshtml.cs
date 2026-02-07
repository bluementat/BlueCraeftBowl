using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlueCraeftBowl.Data;
using BlueCraeftBowl.Models;

namespace BlueCraeftBowl.Pages.Admin;

public class ParlaysModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ParlaysModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<ParlayItem> Items { get; set; } = new();

    [BindProperty]
    public ParlayItem NewItem { get; set; } = new();

    public async Task OnGetAsync()
    {
        Items = await _context.ParlayItems.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // For simplicity, we assume there's only one session for now
        var session = await _context.ParlaySessions.FirstOrDefaultAsync();
        if (session == null)
        {
            session = new ParlaySession { Name = "Main Game", GameStartTime = DateTime.Now.AddDays(1) };
            _context.ParlaySessions.Add(session);
            await _context.SaveChangesAsync();
        }

        NewItem.SessionId = session.Id;
        _context.ParlayItems.Add(NewItem);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var item = await _context.ParlayItems.FindAsync(id);
        if (item != null)
        {
            _context.ParlayItems.Remove(item);
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }
}
