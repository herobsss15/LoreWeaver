import { budgetFor, CR_XP } from '../../utils/xp.js';

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

function filterByBiome(catalog, biome) {
  if (!Array.isArray(catalog) || !catalog.length) {
    return [];
  }
  const tags = BIOME_MAP[biome] ?? [];
  if (!tags.length) {
    return [...catalog];
  }
  return catalog.filter((monster) =>
    (monster.environments ?? []).some((env) => tags.includes(env))
  );
}

function fallbackMonster(catalog) {
  if (!catalog?.length) {
    return [];
  }
  return [{ ...catalog[0], count: 1 }];
}

export function composeEncounter({ levels, difficulty, biome, catalog }) {
  const budget = budgetFor(levels, difficulty);
  if (!budget) {
    return {
      budget,
      monsters: [],
      note: 'Informe níveis válidos do grupo para estimar o orçamento de XP.'
    };
  }

  const pool = filterByBiome(catalog, biome);
  if (!pool.length) {
    return {
      budget,
      monsters: fallbackMonster(catalog),
      note: 'Sem criaturas SRD vinculadas a este bioma. Sugestão genérica apresentada.'
    };
  }

  const sorted = [...pool].sort((a, b) => b.xp - a.xp);
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

    const cheapest = Math.min(...pool.map((monster) => monster.xp || Infinity));
    if (candidate.xp > remaining && remaining > 0 && remaining < cheapest) {
      break;
    }
  }

  const approximate = remaining > 0;

  return {
    budget,
    monsters: picks.length ? picks : fallbackMonster(catalog),
    note: approximate
      ? 'Combinação aproximada; ajuste o encontro conforme necessário.'
      : undefined
  };
}

export function monsterLink(monster) {
  const path = monster.url ?? monster.source ?? `/api/monsters/${monster.index}`;
  if (path.startsWith('http')) {
    return path;
  }
  return `https://www.dnd5eapi.co${path}`;
}

export function formatMonster(monster) {
  const xp = monster.xp ?? CR_XP[monster.cr] ?? 0;
  return `${monster.count ?? 1}× ${monster.name} (CR ${monster.cr}, ${xp} XP)`;
}
