using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlueCraeftBowl.Data;
using BlueCraeftBowl.Models;

namespace BlueCraeftBowl.Pages;

public class LeaderboardModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public LeaderboardModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<LeaderboardEntry> Leaderboard { get; set; } = new();

    public class LeaderboardEntry
    {
        public string FullName { get; set; } = string.Empty;
        public int TotalPoints { get; set; }
    }

    public async Task OnGetAsync()
    {
        Leaderboard = await _context.Users
            .Where(u => u.IsActive)
            .Select(u => new LeaderboardEntry
            {
                FullName = u.FullName,
                TotalPoints = u.Selections.Sum(s => s.PointsEarned)
            })
            .OrderByDescending(e => e.TotalPoints)
            .ToListAsync();
    }
}
