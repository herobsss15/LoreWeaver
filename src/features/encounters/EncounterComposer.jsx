import { useEffect, useMemo, useState } from 'react';
import PropTypes from 'prop-types';
import { CanonBadge } from '../../components/Badges.jsx';
import { copyText } from '../../utils/clipboard.js';
import { composeEncounter, formatMonster, monsterLink } from './buildEncounter.js';
import { parseLevels } from '../../utils/xp.js';

const DIFFICULTY_OPTIONS = [
  { value: 'facil', label: 'Fácil' },
  { value: 'media', label: 'Média' },
  { value: 'dificil', label: 'Difícil' }
];

const BIOMES = [
  'Floresta',
  'Campos',
  'Urbano',
  'Montanha',
  'Deserto',
  'Costa',
  'Pântano',
  'Ártico',
  'Subterrâneo',
  'Qualquer'
];

export default function EncounterComposer({ onSaveHistory, restoreRequest }) {
  const [levelsInput, setLevelsInput] = useState('5,5,5,5');
  const [difficulty, setDifficulty] = useState('media');
  const [biome, setBiome] = useState('Floresta');
  const [result, setResult] = useState(null);

  useEffect(() => {
    if (restoreRequest?.type === 'encontros') {
      const { data } = restoreRequest;
      setLevelsInput(data?.levelsInput ?? '');
      setDifficulty(data?.difficulty ?? 'media');
      setBiome(data?.biome ?? 'Floresta');
      setResult(data?.result ?? null);
    }
  }, [restoreRequest]);

  const levels = useMemo(() => parseLevels(levelsInput), [levelsInput]);

  function handleCompose(event) {
    event.preventDefault();
    if (!levels.length) {
      setResult({
        budget: 0,
        monsters: [],
        note: 'Insira níveis válidos separados por vírgula (ex.: 5,5,5,5).'
      });
      return;
    }
    const composition = composeEncounter({ levels, difficulty, biome });
    setResult(composition);
  }

  async function handleCopy() {
    if (!result) return;
    const lines = [
      `Orçamento estimado: ${result.budget} XP (${difficulty})`,
      ...(result.monsters ?? []).map((monster) => formatMonster(monster)),
      result.note ? `Nota: ${result.note}` : null
    ].filter(Boolean);
    await copyText(lines.join('\n'));
  }

  function handleSave() {
    if (!result) return;
    const historyEntry = {
      id: `encontro-${Date.now()}`,
      type: 'encontros',
      title: `Encontro ${difficulty} em ${biome}`,
      summary: result.monsters?.length
        ? formatMonster(result.monsters[0])
        : 'Sem criaturas sugeridas.',
      timestamp: Date.now(),
      data: {
        levelsInput,
        difficulty,
        biome,
        result
      }
    };
    onSaveHistory(historyEntry);
  }

  useEffect(() => {
    if (!result) {
      const initial = composeEncounter({ levels, difficulty, biome });
      setResult(initial);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <section className="section-card" aria-labelledby="tab-encontros">
      <div className="section-header">
        <h2 className="section-title" id="tab-encontros">
          Compositor de Encontros
        </h2>
        <CanonBadge />
      </div>
      <form className="flex-row" onSubmit={handleCompose} aria-describedby="levels-help">
        <div>
          <label htmlFor="levels-input">Níveis do grupo</label>
          <input
            id="levels-input"
            type="text"
            value={levelsInput}
            onChange={(event) => setLevelsInput(event.target.value)}
            placeholder="Ex.: 5,5,5,5"
          />
        </div>
        <div>
          <label htmlFor="difficulty-select">Dificuldade</label>
          <select
            id="difficulty-select"
            value={difficulty}
            onChange={(event) => setDifficulty(event.target.value)}
          >
            {DIFFICULTY_OPTIONS.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </div>
        <div>
          <label htmlFor="biome-select">Bioma</label>
          <select id="biome-select" value={biome} onChange={(event) => setBiome(event.target.value)}>
            {BIOMES.map((option) => (
              <option key={option} value={option}>
                {option}
              </option>
            ))}
          </select>
        </div>
        <div style={{ alignSelf: 'flex-end' }}>
          <button type="submit">Compor</button>
        </div>
      </form>
      <p id="levels-help" className="notice">
        Os limites de XP seguem a tabela SRD para encontros por dificuldade. Resultados são aproximações.
      </p>

      {result ? (
        <article className="result-card" aria-live="polite">
          <p><strong>Orçamento de XP:</strong> {result.budget || '—'} XP</p>
          {result.monsters?.length ? (
            <ul>
              {result.monsters.map((monster) => (
                <li key={monster.index}>
                  <a href={monsterLink(monster)} target="_blank" rel="noreferrer">
                    {monster.name}
                  </a>{' '}
                  — CR {monster.cr} · {monster.xp} XP · {monster.count ?? 1} unidade(s)
                </li>
              ))}
            </ul>
          ) : (
            <p>Sem criaturas sugeridas para este cenário.</p>
          )}
          {result.note ? <p className="notice">{result.note}</p> : null}
          <div className="actions">
            <button type="button" onClick={handleCopy}>
              Copiar
            </button>
            <button type="button" onClick={handleSave}>
              Salvar no histórico
            </button>
          </div>
        </article>
      ) : null}
    </section>
  );
}

EncounterComposer.propTypes = {
  onSaveHistory: PropTypes.func.isRequired,
  restoreRequest: PropTypes.shape({
    type: PropTypes.string,
    data: PropTypes.object
  })
};
