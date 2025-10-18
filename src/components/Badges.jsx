import PropTypes from 'prop-types';

export function CanonBadge() {
  return <span className="badge badge--canon">SRD</span>;
}

export function HomebrewBadge({ label = 'Homebrew' }) {
  return <span className="badge badge--homebrew">{label}</span>;
}

HomebrewBadge.propTypes = {
  label: PropTypes.string
};
