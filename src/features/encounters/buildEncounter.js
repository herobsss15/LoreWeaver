import { budgetFor, CR_XP } from '../../utils/xp.js';
import { MONSTER_LIBRARY, withXp } from './monsterData.js';

const BIOME_MAP = {
  Floresta: ['Floresta'],
  Campos: ['Campos'],
  Urbano: ['Urbano', 'Ruínas'],
  Montanha: ['Montanha'],
  Deserto: ['Deserto'],
  Costa: ['Costa'],
  Pântano: ['Pântano'],
  Ártico: ['Ártico'],
  Subterrâneo: ['Subterrâneo', 'Caverna'],
  Qualquer: []
};

function filterByBiome(biome) {
  const tags = BIOME_MAP[biome] ?? [];
  if (!tags.length) {
    return MONSTER_LIBRARY.map(withXp);
  }
  return MONSTER_LIBRARY.filter((monster) =>
    monster.environments.some((env) => tags.includes(env))
  ).map(withXp);
}

function fallbackMonster() {
  return [withXp(MONSTER_LIBRARY[0])];
}

export function composeEncounter({ levels, difficulty, biome }) {
  const budget = budgetFor(levels, difficulty);
  if (!budget) {
    return {
      budget,
      monsters: [],
      note: 'Informe níveis válidos do grupo para estimar o orçamento de XP.'
    };
  }

  const pool = filterByBiome(biome);
  if (!pool.length) {
    return {
      budget,
      monsters: fallbackMonster(),
      note: 'Sem criaturas SRD vinculadas a este bioma. Sugestão genérica apresentada.'
    };
  }

  const sorted = pool.sort((a, b) => b.xp - a.xp);
  const picks = [];
  let remaining = budget;
  let guard = 0;

  while (remaining > 0 && picks.length < 6 && guard < 50) {
    guard += 1;
    const candidate = sorted.find((monster) => monster.xp <= remaining) ?? sorted.at(-1);
    if (!candidate) {
      break;
    }

    const existing = picks.find((entry) => entry.index === candidate.index);
    if (existing) {
      existing.count += 1;
    } else {
      picks.push({ ...candidate, count: 1 });
    }
    remaining -= candidate.xp;

    if (candidate.xp > remaining && remaining > 0 && remaining < Math.min(...pool.map((m) => m.xp))) {
      break;
    }
  }

  const approximate = remaining > 0;

  return {
    budget,
    monsters: picks.length ? picks : fallbackMonster(),
    note: approximate
      ? 'Combinação aproximada; ajuste o encontro conforme necessário.'
      : undefined
  };
}

export function monsterLink(monster) {
  return `https://www.dnd5eapi.co${monster.source}`;
}

export function formatMonster(monster) {
  const xp = CR_XP[monster.cr] ?? monster.xp ?? 0;
  return `${monster.count ?? 1}× ${monster.name} (CR ${monster.cr}, ${xp} XP)`;
}
