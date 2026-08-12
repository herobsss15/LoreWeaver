using LoreWeaver.Features.Common;

namespace LoreWeaver.Features.Npcs;

public record NpcRole(string Label, string[] Tags);

public static class NpcData
{
    public static readonly string[] GivenNames =
    [
        "Aelar", "Brina", "Cairon", "Dessa", "Edrin", "Fiora", "Garrik", "Halia",
        "Igan", "Jorunn", "Kaela", "Loric", "Miri", "Neris", "Orlan", "Pavel",
        "Queli", "Rurik", "Syla", "Thamior", "Ulric", "Vanya", "Westra", "Yorik", "Zanna"
    ];

    public static readonly NpcRole[] Roles =
    [
        new("Artesão", ["urbano", "campo"]),
        new("Batedor", ["selvagem", "militar"]),
        new("Capitão da guarda", ["urbano", "militar"]),
        new("Curandeiro", ["comunidade", "sagrado"]),
        new("Erudito", ["urbano", "sagrado"]),
        new("Ferreiro", ["urbano", "campo"]),
        new("Guia local", ["selvagem", "campo"]),
        new("Marinheiro veterano", ["costa"]),
        new("Mercador ambulante", ["urbano", "campo"]),
        new("Patrulheiro", ["selvagem", "militar"]),
        new("Sacerdote leigo", ["sagrado"]),
        new("Taverneiro", ["urbano"])
    ];

    public static readonly IReadOnlyDictionary<string, string[]> Tones = new Dictionary<string, string[]>
    {
        ["neutro"] = ["discreto", "pragmático", "paciente", "observador", "solidário"],
        ["sombrio"] = ["cansado", "desconfiado", "melancólico", "misterioso", "cínico"],
        ["esperanca"] = ["otimista", "amigável", "inspirador", "encorajador", "resoluto"],
        ["tenso"] = ["alerta", "ansioso", "apressado", "resignado", "ferido"]
    };

    public static readonly string[] Motivations =
    [
        "proteger a família", "pagar uma dívida antiga", "conseguir recursos para a comunidade",
        "descobrir segredos perdidos", "derrotar um inimigo específico", "recuperar um item roubado",
        "provar seu valor", "fugir de uma ameaça", "apaziguar uma entidade local", "manter uma tradição viva"
    ];

    public static readonly string[] Hooks =
    [
        "oferece mapas confiáveis do entorno", "precisa de escolta até um ponto perigoso",
        "ouviu rumores sobre um artefato na região", "negocia acesso a um contato influente",
        "pede ajuda para lidar com um problema sobrenatural", "pode interceder junto à guarda local",
        "tem informações sobre uma rota secreta", "conhece fraquezas de uma criatura próxima",
        "está reunindo voluntários para uma missão urgente", "possui um item útil em troca de um favor"
    ];

    public static readonly string[] Traits =
    [
        "fala com voz suave e ritmada", "mantém um amuleto ancestral sempre visível",
        "coleciona histórias sobre viajantes", "anota tudo em um pequeno diário de couro",
        "carrega ferramentas impecavelmente organizadas", "toca tambor silenciosamente quando está nervoso",
        "usa perfume de flores locais", "calcula riscos antes de agir",
        "demonstra gentileza com estranhos", "observa padrões climáticos com precisão"
    ];

    public static NpcRole? RoleForBias(string? bias, SeededRandom rng)
    {
        var lowered = bias?.ToLowerInvariant();
        if (string.IsNullOrEmpty(lowered))
        {
            return null;
        }

        var filtered = Roles.Where(role => role.Tags.Contains(lowered)).ToArray();
        return filtered.Length == 0 ? null : rng.Pick(filtered);
    }
}
