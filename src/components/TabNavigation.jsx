import PropTypes from 'prop-types';

export default function TabNavigation({ tabs, activeTab, onChange }) {
  return (
    <nav className="tabs" aria-label="Navegação principal">
      {tabs.map((tab) => (
        <button
          key={tab.value}
          type="button"
          className="tab-button"
          role="tab"
          aria-selected={activeTab === tab.value}
          onClick={() => onChange(tab.value)}
        >
          {tab.label}
        </button>
      ))}
    </nav>
  );
}

TabNavigation.propTypes = {
  tabs: PropTypes.arrayOf(
    PropTypes.shape({
      value: PropTypes.string.isRequired,
      label: PropTypes.string.isRequired
    })
  ).isRequired,
  activeTab: PropTypes.string.isRequired,
  onChange: PropTypes.func.isRequired
};
