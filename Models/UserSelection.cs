namespace BlueCraeftBowl.Models;

public class UserSelection
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public virtual ApplicationUser? User { get; set; }

    public int ParlayItemId { get; set; }
    public virtual ParlayItem? ParlayItem { get; set; }

    public string SelectedValue { get; set; } = string.Empty;
    public int PointsEarned { get; set; } = 0;
}
