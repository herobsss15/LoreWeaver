import PropTypes from 'prop-types';
import { format } from '../utils/date.js';

export default function HistoryPanel({ items, onRestore, onClear }) {
  if (!items.length) {
    return (
      <section className="section-card history-panel" aria-live="polite">
        <h2 className="section-title">Histórico</h2>
        <p className="notice">Nenhum item salvo ainda.</p>
      </section>
    );
  }

  return (
    <section className="section-card history-panel" aria-live="polite">
      <div className="section-header">
        <h2 className="section-title">Histórico</h2>
        <button type="button" onClick={onClear}>
          Limpar histórico
        </button>
      </div>
      <ul className="history-list">
        {items.map((item) => (
          <li key={item.id} className="history-item">
            <div>
              <p style={{ margin: '0 0 0.35rem', fontWeight: 600 }}>{item.title}</p>
              <p style={{ margin: 0, color: '#8b949e' }}>{item.summary}</p>
              <p className="notice">{format(item.timestamp)}</p>
            </div>
            <button type="button" onClick={() => onRestore(item)}>
              Reabrir
            </button>
          </li>
        ))}
      </ul>
    </section>
  );
}

HistoryPanel.propTypes = {
  items: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.string.isRequired,
      type: PropTypes.string.isRequired,
      title: PropTypes.string.isRequired,
      summary: PropTypes.string.isRequired,
      timestamp: PropTypes.number.isRequired
    })
  ).isRequired,
  onRestore: PropTypes.func.isRequired,
  onClear: PropTypes.func.isRequired
};
