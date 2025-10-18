export const GIVEN_NAMES = [
  'Aelar',
  'Brina',
  'Cairon',
  'Dessa',
  'Edrin',
  'Fiora',
  'Garrik',
  'Halia',
  'Igan',
  'Jorunn',
  'Kaela',
  'Loric',
  'Miri',
  'Neris',
  'Orlan',
  'Pavel',
  'Queli',
  'Rurik',
  'Syla',
  'Thamior',
  'Ulric',
  'Vanya',
  'Westra',
  'Yorik',
  'Zanna'
];

export const ROLES = [
  { label: 'Artesão', tags: ['urbano', 'campo'] },
  { label: 'Batedor', tags: ['selvagem', 'militar'] },
  { label: 'Capitão da guarda', tags: ['urbano', 'militar'] },
  { label: 'Curandeiro', tags: ['comunidade', 'sagrado'] },
  { label: 'Erudito', tags: ['urbano', 'sagrado'] },
  { label: 'Ferreiro', tags: ['urbano', 'campo'] },
  { label: 'Guia local', tags: ['selvagem', 'campo'] },
  { label: 'Marinheiro veterano', tags: ['costa'] },
  { label: 'Mercador ambulante', tags: ['urbano', 'campo'] },
  { label: 'Patrulheiro', tags: ['selvagem', 'militar'] },
  { label: 'Sacerdote leigo', tags: ['sagrado'] },
  { label: 'Taverneiro', tags: ['urbano'] }
];

export const TONES = {
  neutro: ['discreto', 'pragmático', 'paciente', 'observador', 'solidário'],
  sombrio: ['cansado', 'desconfiado', 'melancólico', 'misterioso', 'cínico'],
  esperanca: ['otimista', 'amigável', 'inspirador', 'encorajador', 'resoluto'],
  tenso: ['alerta', 'ansioso', 'apressado', 'resignado', 'ferido']
};

export const MOTIVATIONS = [
  'proteger a família',
  'pagar uma dívida antiga',
  'conseguir recursos para a comunidade',
  'descobrir segredos perdidos',
  'derrotar um inimigo específico',
  'recuperar um item roubado',
  'provar seu valor',
  'fugir de uma ameaça',
  'apaziguar uma entidade local',
  'manter uma tradição viva'
];

export const HOOKS = [
  'oferece mapas confiáveis do entorno',
  'precisa de escolta até um ponto perigoso',
  'ouviu rumores sobre um artefato na região',
  'negocia acesso a um contato influente',
  'pede ajuda para lidar com um problema sobrenatural',
  'pode interceder junto à guarda local',
  'tem informações sobre uma rota secreta',
  'conhece fraquezas de uma criatura próxima',
  'está reunindo voluntários para uma missão urgente',
  'possui um item útil em troca de um favor'
];

export const TRAITS = [
  'fala com voz suave e ritmada',
  'mantém um amuleto ancestral sempre visível',
  'coleciona histórias sobre viajantes',
  'anota tudo em um pequeno diário de couro',
  'carrega ferramentas impecavelmente organizadas',
  'toca tambor silenciosamente quando está nervoso',
  'usa perfume de flores locais',
  'calcula riscos antes de agir',
  'demonstra gentileza com estranhos',
  'observa padrões climáticos com precisão'
];

export function roleForBias(bias, rng) {
  const lowered = bias?.toLowerCase();
  if (!lowered) {
    return null;
  }
  const filtered = ROLES.filter((role) => role.tags.includes(lowered));
  if (!filtered.length) {
    return null;
  }
  const index = Math.floor(rng() * filtered.length);
  return filtered[index];
}
