# Checklist Manual - GET /rules/query

1. `GET /rules/query?ref=/api/rules/ability-checks`
   - Esperado: HTTP 200, `status=ok`, `data[0].ref` presente, `meta.cached` refletindo cache.
2. `GET /rules/query?slug=ability-checks`
   - Esperado: mesmo payload lógico da requisição com `ref`.
3. `GET /rules/query?q=ability&limit=3`
   - Esperado: HTTP 200, `status=ok`, `warnings` contém `PARTIAL_MATCH` quando resultados não exatos.
4. `GET /rules/query?slug=inexistente`
   - Esperado: HTTP 404, `status=not_found`, `data` omitido.
5. `GET /rules/query?slug=abil`
   - Esperado: HTTP 200, `status=ambiguous`, `data.candidates[]` preenchido com `ref` e `slug`.
6. Repetir qualquer cenário acima com mesmos parâmetros após primeira resposta
   - Esperado: `meta.cached=true` na segunda chamada.
7. `GET /rules/query?limit=50`
   - Esperado: HTTP 422, `status=refuse`, warning `INVALID_PARAMS`.
