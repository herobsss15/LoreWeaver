import { useEffect, useMemo, useState } from 'react';
import PropTypes from 'prop-types';
import { HomebrewBadge } from '../../components/Badges.jsx';
import { copyText } from '../../utils/clipboard.js';
import { createRng, pick } from '../../utils/random.js';
import {
  GIVEN_NAMES,
  HOOKS,
  MOTIVATIONS,
  ROLES,
  TONES,
  TRAITS,
  roleForBias
} from './npcData.js';

const TONE_OPTIONS = [
  { value: 'neutro', label: 'Neutro' },
  { value: 'sombrio', label: 'Sombrio' },
  { value: 'esperanca', label: 'Esperança' },
  { value: 'tenso', label: 'Tenso' }
];

const ROLE_BIAS_OPTIONS = [
  { value: '', label: 'Sem viés' },
  { value: 'urbano', label: 'Urbano' },
  { value: 'selvagem', label: 'Selvagem' },
  { value: 'militar', label: 'Militar' },
  { value: 'comunidade', label: 'Comunidade' },
  { value: 'sagrado', label: 'Sagrado' },
  { value: 'campo', label: 'Campo' },
  { value: 'costa', label: 'Costa' }
];

function sanitizeLocation(location) {
  return location?.trim() || 'local indefinido';
}

function npcToText(npc) {
  return `${npc.nome}, ${npc.papel} — traço: ${npc.traco}; motivação: ${npc.motivacao}; gancho: ${npc.gancho}`;
}

export default function NPCGenerator({ onSaveHistory, restoreRequest }) {
  const [quantity, setQuantity] = useState(1);
  const [tone, setTone] = useState('neutro');
  const [location, setLocation] = useState('');
  const [roleBias, setRoleBias] = useState('');
  const [seedBump, setSeedBump] = useState(0);
  const [npcs, setNpcs] = useState([]);

  useEffect(() => {
    if (restoreRequest?.type === 'npcs') {
      const { data } = restoreRequest;
      setQuantity(data?.quantity ?? 1);
      setTone(data?.tone ?? 'neutro');
      setLocation(data?.location ?? '');
      setRoleBias(data?.roleBias ?? '');
      setSeedBump(data?.seedBump ?? 0);
      setNpcs(data?.npcs ?? []);
    }
  }, [restoreRequest]);

  const seed = useMemo(
    () => `${quantity}-${tone}-${location}-${roleBias}-${seedBump}`,
    [quantity, tone, location, roleBias, seedBump]
  );

  useEffect(() => {
    if (!npcs.length) return;
    // ensure determinism when restored
    setSeedBump((prev) => prev);
  }, []);

  function generate() {
    const safeQuantity = Math.min(Math.max(quantity, 1), 10);
    const rng = createRng(seed);
    const toneTraits = TONES[tone] ?? TONES.neutro;
    const produced = Array.from({ length: safeQuantity }, () => {
      const name = pick(rng, GIVEN_NAMES);
      const biasRole = roleForBias(roleBias, rng);
      const role = biasRole ?? pick(rng, ROLES)?.label ?? 'Cidadão';
      const traitSource = [...TRAITS, ...toneTraits];
      const trait = pick(rng, traitSource);
      const motivation = pick(rng, MOTIVATIONS);
      const hook = pick(rng, HOOKS);
      return {
        nome: name,
        papel: role,
        traco: trait,
        motivacao: motivation,
        gancho: hook,
        contexto: sanitizeLocation(location)
      };
    });
    setNpcs(produced);
  }

  function handleSubmit(event) {
    event.preventDefault();
    generate();
  }

  function handleReroll() {
    setSeedBump((prev) => prev + 1);
    setTimeout(generate, 0);
  }

  async function handleCopy() {
    if (!npcs.length) return;
    const lines = npcs.map((npc, index) => `${index + 1}. ${npcToText(npc)}`);
    await copyText(lines.join('\n'));
  }

  function handleSave() {
    if (!npcs.length) return;
    const historyEntry = {
      id: `npc-${seed}-${Date.now()}`,
      type: 'npcs',
      title: `${npcs.length} NPC(s) — ${sanitizeLocation(location)}`,
      summary: npcToText(npcs[0]),
      timestamp: Date.now(),
      data: {
        quantity,
        tone,
        location,
        roleBias,
        seedBump,
        npcs
      }
    };
    onSaveHistory(historyEntry);
  }

  useEffect(() => {
    generate();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [seed]);

  return (
    <section className="section-card" aria-labelledby="tab-npcs">
      <div className="section-header">
        <h2 className="section-title" id="tab-npcs">
          Gerador de NPCs
        </h2>
        <HomebrewBadge label="Homebrew" />
      </div>
      <form onSubmit={handleSubmit} className="flex-row" aria-describedby="npc-limite">
        <div>
          <label htmlFor="npc-quantidade">Quantidade (1–10)</label>
          <input
            id="npc-quantidade"
            type="number"
            min="1"
            max="10"
            value={quantity}
            onChange={(event) => setQuantity(Number(event.target.value))}
          />
        </div>
        <div>
          <label htmlFor="npc-tom">Tom</label>
          <select id="npc-tom" value={tone} onChange={(event) => setTone(event.target.value)}>
            {TONE_OPTIONS.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </div>
        <div>
          <label htmlFor="npc-local">Local</label>
          <input
            id="npc-local"
            type="text"
            value={location}
            onChange={(event) => setLocation(event.target.value)}
            placeholder="Ex.: Porto brumoso"
          />
        </div>
        <div>
          <label htmlFor="npc-vies">Viés de papel</label>
          <select
            id="npc-vies"
            value={roleBias}
            onChange={(event) => setRoleBias(event.target.value)}
          >
            {ROLE_BIAS_OPTIONS.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </div>
        <div style={{ alignSelf: 'flex-end' }}>
          <button type="submit">Gerar</button>
        </div>
      </form>
      <p id="npc-limite" className="notice">
        Geração determinística local. Use "Re-rolar" para nova seed.
      </p>

      {npcs.map((npc, index) => (
        <article key={`${npc.nome}-${index}`} className="result-card" aria-live="polite">
          <header style={{ display: 'flex', justifyContent: 'space-between', gap: '1rem' }}>
            <strong>
              {index + 1}. {npc.nome}
            </strong>
            <span>{npc.papel}</span>
          </header>
          <p className="notice">Contexto: {npc.contexto}</p>
          <p>Traço: {npc.traco}.</p>
          <p>Motivação: {npc.motivacao}.</p>
          <p>Gancho: {npc.gancho}.</p>
        </article>
      ))}

      <div className="actions" style={{ marginTop: '1rem' }}>
        <button type="button" onClick={handleCopy}>
          Copiar
        </button>
        <button type="button" onClick={handleReroll}>
          Re-rolar
        </button>
        <button type="button" onClick={handleSave}>
          Salvar no histórico
        </button>
      </div>
    </section>
  );
}

NPCGenerator.propTypes = {
  onSaveHistory: PropTypes.func.isRequired,
  restoreRequest: PropTypes.shape({
    type: PropTypes.string,
    data: PropTypes.object
  })
};
