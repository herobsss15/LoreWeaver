namespace LoreWeaver.Features.Npcs;

public record Npc(string Name, string Role, string Trait, string Motivation, string Hook, string Context)
{
    public string ToText(int index) => $"{index}. {Name}, {Role} — traço: {Trait}; motivação: {Motivation}; gancho: {Hook}";
}
