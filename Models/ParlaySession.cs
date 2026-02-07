namespace BlueCraeftBowl.Models;

public class ParlaySession
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsLocked { get; set; } = false;
    public DateTime GameStartTime { get; set; }
    public virtual ICollection<ParlayItem> Items { get; set; } = new List<ParlayItem>();
}
