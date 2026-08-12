namespace LoreWeaver.Data.Entities;

public class AbilityScoreValue
{
    public int Score { get; set; } = 10;
    public int? ModifierOverride { get; set; }

    // floor((Score-10)/2): plain int division truncates toward zero and
    // silently breaks for odd scores below 10 (9 -> 0 instead of -1).
    public int Modifier => ModifierOverride ?? (int)Math.Floor((Score - 10) / 2.0);
}
