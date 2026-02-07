namespace BlueCraeftBowl.Models;

public enum ParlayType
{
    AB,
    OverUnder
}

public class ParlayItem
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public virtual ParlaySession? Session { get; set; }

    public string Question { get; set; } = string.Empty;
    public ParlayType Type { get; set; }
    
    public string? OptionA { get; set; }
    public string? OptionB { get; set; }
    
    public float? Threshold { get; set; }
    
    public string? CorrectAnswer { get; set; }
    public bool IsResolved { get; set; } = false;

    public virtual ICollection<UserSelection> Selections { get; set; } = new List<UserSelection>();
}
