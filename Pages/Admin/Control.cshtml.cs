using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlueCraeftBowl.Data;
using BlueCraeftBowl.Models;
using BlueCraeftBowl.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BlueCraeftBowl.Pages.Admin;

public class ControlModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<LeaderboardHub> _hubContext;

    public ControlModel(ApplicationDbContext context, IHubContext<LeaderboardHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    public ParlaySession? Session { get; set; }
    public List<ParlayItem> Items { get; set; } = new();

    public async Task OnGetAsync()
    {
        Session = await _context.ParlaySessions.FirstOrDefaultAsync();
        Items = await _context.ParlayItems.ToListAsync();
    }

    public async Task<IActionResult> OnPostToggleLockAsync()
    {
        var session = await _context.ParlaySessions.FirstOrDefaultAsync();
        if (session != null)
        {
            session.IsLocked = !session.IsLocked;
            await _context.SaveChangesAsync();
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostResolveAsync(int id, string result)
    {
        if (string.IsNullOrEmpty(result)) return RedirectToPage();

        var item = await _context.ParlayItems.Include(i => i.Selections).FirstOrDefaultAsync(i => i.Id == id);
        if (item != null)
        {
            item.CorrectAnswer = result;
            item.IsResolved = true;

            foreach (var selection in item.Selections)
            {
                selection.PointsEarned = (selection.SelectedValue == result) ? 10 : 0;
            }

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveLeaderboardUpdate");
        }
        return RedirectToPage();
    }
}
