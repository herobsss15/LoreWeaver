import { useState } from 'react';
import TabNavigation from './components/TabNavigation.jsx';
import HistoryPanel from './components/HistoryPanel.jsx';
import RulesSearch from './features/rules/RulesSearch.jsx';
import NPCGenerator from './features/npcs/NPCGenerator.jsx';
import EncounterComposer from './features/encounters/EncounterComposer.jsx';
import { useHistory } from './hooks/useHistory.js';

const TABS = [
  { value: 'regras', label: 'Regras' },
  { value: 'npcs', label: 'NPCs' },
  { value: 'encontros', label: 'Encontros' }
];

export default function App() {
  const [activeTab, setActiveTab] = useState('regras');
  const [restoreRequest, setRestoreRequest] = useState(null);
  const { items, addItem, clear } = useHistory();

  function handleHistoryRestore(item) {
    setActiveTab(item.type);
    setRestoreRequest(item);
  }

  function handleTabChange(tab) {
    setActiveTab(tab);
    setRestoreRequest(null);
  }

  return (
    <main>
      <header style={{ marginBottom: '2rem' }}>
        <h1 style={{ fontSize: '2rem', marginBottom: '0.5rem' }}>LoreWeaver v0</h1>
        <p className="notice">
          Ferramenta SRD-first para apoiar mesas de D&D 5e. Resultados homebrew são marcados.
        </p>
      </header>

      <TabNavigation tabs={TABS} activeTab={activeTab} onChange={handleTabChange} />

      {activeTab === 'regras' ? (
        <RulesSearch onSaveHistory={addItem} restoreRequest={restoreRequest} />
      ) : null}
      {activeTab === 'npcs' ? (
        <NPCGenerator onSaveHistory={addItem} restoreRequest={restoreRequest} />
      ) : null}
      {activeTab === 'encontros' ? (
        <EncounterComposer onSaveHistory={addItem} restoreRequest={restoreRequest} />
      ) : null}

      <HistoryPanel items={items} onRestore={handleHistoryRestore} onClear={clear} />
    </main>
  );
}
