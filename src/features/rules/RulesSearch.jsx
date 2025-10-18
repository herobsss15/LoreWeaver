import { useEffect, useMemo, useState } from 'react';
import PropTypes from 'prop-types';
import { CanonBadge } from '../../components/Badges.jsx';
import { copyText } from '../../utils/clipboard.js';

const API_BASE = 'https://www.dnd5eapi.co';

async function fetchJson(endpoint) {
  const response = await fetch(`${API_BASE}${endpoint}`);
  if (!response.ok) {
    throw new Error(`Falha na requisição: ${response.status}`);
  }
  return response.json();
}

async function searchCollection(collection, term) {
  const query = encodeURIComponent(term.trim());
  if (!query) return [];
  try {
    const data = await fetchJson(`/api/${collection}?name=${query}`);
    return (data.results ?? []).map((item) => ({ ...item, collection }));
  } catch (error) {
    console.warn(`Busca em ${collection} falhou`, error);
    return [];
  }
}

async function fetchDetail(collection, index) {
  try {
    const data = await fetchJson(`/api/${collection}/${index}`);
    return { ...data, collection };
  } catch (error) {
    if (error.message.includes('404')) {
      return null;
    }
    throw error;
  }
}

function toSummary(detail) {
  if (!detail) return '';
  const { desc, subsections } = detail;
  if (Array.isArray(desc) && desc.length) {
    return desc[0];
  }
  if (Array.isArray(subsections) && subsections.length) {
    return subsections[0]?.desc?.[0] ?? '';
  }
  return '';
}

function canonicalLink(detail) {
  if (!detail?.url) return undefined;
  return `${API_BASE}${detail.url}`;
}

function serializeResult(detail) {
  if (!detail) return '';
  const lines = [
    `${detail.name} (${detail.collection === 'rules' ? 'SRD Rule' : 'SRD Rule Section'})`,
    '',
    ...(detail.desc ?? [])
  ];
  return lines.join('\n');
}

export default function RulesSearch({ onSaveHistory, restoreRequest }) {
  const [term, setTerm] = useState('');
  const [slug, setSlug] = useState('');
  const [status, setStatus] = useState('idle');
  const [error, setError] = useState('');
  const [results, setResults] = useState([]);
  const [selected, setSelected] = useState(null);

  useEffect(() => {
    if (restoreRequest?.type === 'regras') {
      const { data } = restoreRequest;
      setTerm(data?.term ?? '');
      setSlug(data?.slug ?? '');
      if (data?.selected) {
        setSelected(data.selected);
        setResults(data.results ?? []);
        setStatus(data.status ?? 'success');
      }
    }
  }, [restoreRequest]);

  async function handleTermSearch(event) {
    event.preventDefault();
    if (!term.trim()) {
      setStatus('empty');
      setResults([]);
      setSelected(null);
      return;
    }

    setStatus('loading');
    setError('');
    setSelected(null);
    try {
      const [rules, sections] = await Promise.all([
        searchCollection('rules', term),
        searchCollection('rule-sections', term)
      ]);
      const combined = [...rules, ...sections];
      if (!combined.length) {
        setStatus('not-found');
        setResults([]);
        return;
      }

      if (combined.length > 1) {
        setStatus('multiple');
        setResults(combined);
        return;
      }

      const only = combined[0];
      const detail = await fetchDetail(only.collection, only.index);
      setSelected(detail);
      setResults([detail]);
      setStatus('success');
    } catch (searchError) {
      console.error(searchError);
      setError('Erro ao consultar a SRD. Tente novamente em instantes.');
      setStatus('error');
    }
  }

  async function handleSlugSearch(event) {
    event.preventDefault();
    if (!slug.trim()) {
      setStatus('empty');
      setResults([]);
      setSelected(null);
      return;
    }

    setStatus('loading');
    setError('');
    setResults([]);
    setSelected(null);

    const collections = ['rules', 'rule-sections'];
    for (const collection of collections) {
      try {
        const detail = await fetchDetail(collection, slug.trim());
        if (detail) {
          setSelected(detail);
          setResults([detail]);
          setStatus('success');
          return;
        }
      } catch (searchError) {
        console.warn(`Falha ao buscar slug ${slug} em ${collection}`, searchError);
      }
    }
    setStatus('not-found');
  }

  async function handleSelect(index, collection) {
    setStatus('loading');
    try {
      const detail = await fetchDetail(collection, index);
      setSelected(detail);
      setResults((prev) => prev.map((item) => (item.index === index ? detail : item)));
      setStatus('success');
    } catch (searchError) {
      console.error(searchError);
      setError('Não foi possível carregar o conteúdo desta entrada.');
      setStatus('error');
    }
  }

  function handleSave() {
    if (!selected) return;
    const historyEntry = {
      id: `regra-${selected.index}-${Date.now()}`,
      type: 'regras',
      title: selected.name,
      summary: toSummary(selected) || 'Sem resumo disponível.',
      timestamp: Date.now(),
      data: {
        term,
        slug,
        selected,
        results,
        status
      }
    };
    onSaveHistory(historyEntry);
  }

  const summary = useMemo(() => toSummary(selected), [selected]);
  const shareText = useMemo(() => serializeResult(selected), [selected]);

  async function handleCopy() {
    if (!shareText) return;
    await copyText(shareText);
  }

  return (
    <section className="section-card" aria-labelledby="tab-regras">
      <div className="section-header">
        <h2 className="section-title" id="tab-regras">
          Busca de Regras
        </h2>
        {selected ? <CanonBadge /> : null}
      </div>
      <form className="flex-row" onSubmit={handleTermSearch}>
        <div>
          <label htmlFor="term-search">Buscar por termo</label>
          <input
            id="term-search"
            type="search"
            value={term}
            onChange={(event) => setTerm(event.target.value)}
            placeholder="Ex.: initiative"
          />
        </div>
        <div style={{ alignSelf: 'flex-end' }}>
          <button type="submit">Buscar termo</button>
        </div>
      </form>
      <form className="flex-row" onSubmit={handleSlugSearch} style={{ marginTop: '1rem' }}>
        <div>
          <label htmlFor="slug-search">Buscar por slug (index SRD)</label>
          <input
            id="slug-search"
            type="text"
            value={slug}
            onChange={(event) => setSlug(event.target.value)}
            placeholder="Ex.: initiative"
          />
        </div>
        <div style={{ alignSelf: 'flex-end' }}>
          <button type="submit">Buscar slug</button>
        </div>
      </form>

      <div className="notice" role="status" aria-live="polite">
        {status === 'idle' && 'Pesquise por termo ou slug da SRD.'}
        {status === 'loading' && 'Carregando resultados...'}
        {status === 'empty' && 'Informe um termo ou slug válido.'}
        {status === 'not-found' && 'Nenhuma entrada canônica encontrada.'}
        {status === 'error' && error}
        {status === 'multiple' &&
          'Múltiplas entradas possíveis. Selecione uma para ver o detalhe.'}
      </div>

      {status === 'multiple' ? (
        <div className="result-card" role="list">
          {results.map((item) => (
            <article key={`${item.collection}-${item.index}`} role="listitem">
              <header style={{ display: 'flex', justifyContent: 'space-between', gap: '1rem' }}>
                <strong>{item.name}</strong>
                <button type="button" onClick={() => handleSelect(item.index, item.collection)}>
                  Abrir
                </button>
              </header>
              <p className="notice">{item.collection === 'rules' ? 'Regra SRD' : 'Seção SRD'}</p>
              <a href={`${API_BASE}${item.url}`} target="_blank" rel="noreferrer">
                Link canônico
              </a>
            </article>
          ))}
        </div>
      ) : null}

      {selected ? (
        <article className="result-card" aria-live="polite">
          <header style={{ display: 'flex', justifyContent: 'space-between', gap: '1rem' }}>
            <h3 style={{ margin: 0 }}>{selected.name}</h3>
            <a href={canonicalLink(selected)} target="_blank" rel="noreferrer">
              Abrir na SRD
            </a>
          </header>
          {summary ? <p className="notice">{summary}</p> : null}
          {(selected.desc ?? []).slice(0, 3).map((paragraph, index) => (
            <p key={index}>{paragraph}</p>
          ))}
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

RulesSearch.propTypes = {
  onSaveHistory: PropTypes.func.isRequired,
  restoreRequest: PropTypes.shape({
    type: PropTypes.string,
    data: PropTypes.object
  })
};
