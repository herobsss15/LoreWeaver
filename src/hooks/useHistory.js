import { useCallback, useEffect, useState } from 'react';

const STORAGE_KEY = 'loreweaver-history-v0';
const MAX_ITEMS = 10;

function load() {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    return raw ? JSON.parse(raw) : [];
  } catch (error) {
    console.warn('Não foi possível carregar o histórico', error);
    return [];
  }
}

export function useHistory() {
  const [items, setItems] = useState(() => load());

  useEffect(() => {
    try {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(items));
    } catch (error) {
      console.warn('Não foi possível persistir o histórico', error);
    }
  }, [items]);

  const addItem = useCallback((item) => {
    setItems((previous) => {
      const merged = [item, ...previous.filter((entry) => entry.id !== item.id)];
      return merged.slice(0, MAX_ITEMS);
    });
  }, []);

  const clear = useCallback(() => {
    setItems([]);
  }, []);

  return { items, addItem, clear };
}
