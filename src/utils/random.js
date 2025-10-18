import seedrandom from 'seedrandom';

export function createRng(seed) {
  return seedrandom(seed);
}

export function pick(rng, list) {
  if (!list.length) return undefined;
  const index = Math.floor(rng() * list.length);
  return list[index];
}

export function pickMany(rng, list, count) {
  const pool = [...list];
  const results = [];
  for (let i = 0; i < count && pool.length; i += 1) {
    const index = Math.floor(rng() * pool.length);
    results.push(pool.splice(index, 1)[0]);
  }
  return results;
}
