import { useEffect, useState } from 'react';
import { MONSTER_SOURCES } from './monsterData.js';
import { CR_XP } from '../../utils/xp.js';

const API_BASE = 'https://www.dnd5eapi.co';

const FRACTION_MAP = {
  0: '0',
  0.125: '1/8',
  0.25: '1/4',
  0.5: '1/2'
};

function toCrString(value) {
  if (value == null) return '?';
  if (FRACTION_MAP[value]) return FRACTION_MAP[value];
  if (Number.isInteger(value)) {
    return String(value);
  }
  return value.toString();
}

function normalizeMonster(monster, detail) {
  const crValue = toCrString(detail?.challenge_rating ?? monster.cr);
  const url = detail?.url ?? monster.source ?? `/api/monsters/${monster.index}`;
  const xp = detail?.xp ?? CR_XP[crValue] ?? 0;
  return {
    ...monster,
    name: detail?.name ?? monster.name,
    cr: crValue,
    xp,
    url
  };
}

export function useMonsterCatalog() {
  const [status, setStatus] = useState('idle');
  const [error, setError] = useState('');
  const [data, setData] = useState([]);

  useEffect(() => {
    let cancelled = false;

    async function load() {
      setStatus('loading');
      setError('');

      const requests = MONSTER_SOURCES.map(async (monster) => {
        try {
          const response = await fetch(`${API_BASE}${monster.source}`);
          if (!response.ok) {
            throw new Error(`Falha ao carregar ${monster.index}`);
          }
          const detail = await response.json();
          return normalizeMonster(monster, detail);
        } catch (requestError) {
          console.warn(requestError);
          return normalizeMonster(monster, null);
        }
      });

      try {
        const results = await Promise.all(requests);
        if (!cancelled) {
          setData(results);
          setStatus('success');
        }
      } catch (loadError) {
        if (!cancelled) {
          setStatus('error');
          setError('Não foi possível carregar a lista de criaturas SRD.');
        }
      }
    }

    load();

    return () => {
      cancelled = true;
    };
  }, []);

  return { status, error, data };
}
