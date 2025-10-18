# GET /rules/query

## Visão Geral
Endpoint para busca de regras da SRD 5e com suporte a consulta por `ref`, `slug` ou texto (`q`). Todas as respostas seguem o envelope `{status, data?, warnings?, meta:{request_id, took_ms, cached, source}}`.

## Parâmetros de Consulta
| Nome | Tipo | Obrigatório | Limites/Validação | Descrição |
| --- | --- | --- | --- | --- |
| `domain` | `string` | não | enum `rules`, `rule-sections`; padrão: ambos | Restringe os domínios pesquisados. |
| `q` | `string` | condicional | 1–100 caracteres UTF-8; case-insensitive | Texto livre para busca contextual. Necessário se `slug` e `ref` ausentes. |
| `slug` | `string` | condicional | 1–80 caracteres; após normalização `[a-z0-9-]+` | Identificador canônico de regra. Necessário se `q` e `ref` ausentes. |
| `ref` | `string` | condicional | Deve iniciar com `/api/{rules|rule-sections}/`; 1–160 caracteres | Referência direta retornada pela SRD. Ignora `slug`/`q` se presente. |
| `limit` | `integer` | não | 1–10; padrão 5 | Número máximo de itens retornados em buscas por `slug` parcial ou `q`. |
| `include_raw` | `boolean` | não | padrão `false` | Quando verdadeiro, inclui transcrição completa (`raw`) da descrição fornecida pela SRD. |

### Regras de Normalização de Slug
1. Converter para minúsculas.
2. Substituir caracteres fora de `[a-z0-9-]` por `-`.
3. Compactar sequências de `-` para um único hífen.
4. Remover hífens das extremidades.

## Estratégia de Lookup
1. **Ref direto**: se `ref` presente, buscar exclusivamente por ele (`rules` ou `rule-sections`). Retorna `ok` quando encontrado; `not_found` caso contrário.
2. **Slug exato**: se `slug` informado, aplicar normalização e buscar cache → persistência → SRD. Retorna `ok` (item único) ou `not_found`.
3. **Prefixo de slug**: se `slug` sem match exato ou se `q` fornecido em conjunto, procurar por prefixos dentro dos domínios permitidos. Múltiplos resultados produzem `ambiguous` com `data.candidates` (limitado por `limit`).
4. **Busca textual (`q`)**: consulta full-text case-insensitive sobre `name` e `desc`. Ordenar por relevância e truncar por `limit`. Resultados parciais disparam `warnings.PARTIAL_MATCH`.

## Esquema da Resposta
```json
{
  "status": "ok",
  "data": [
    {
      "slug": "ability-checks",
      "name": "Ability Checks",
      "domain": "rules",
      "ref": "/api/rules/ability-checks",
      "url": "https://www.dnd5eapi.co/api/rules/ability-checks",
      "source": "5e-srd",
      "description": {
        "excerpt": "When you attempt to...",
        "raw": ["When you attempt to...", "Using the same ability..."]
      },
      "references": [
        {
          "title": "Ability Checks",
          "origin": "srd"
        }
      ],
      "last_synced_at": "2025-02-10T18:30:22Z"
    }
  ],
  "warnings": [
    {
      "code": "PARTIAL_MATCH",
      "message": "Consulta textual retornou resultados aproximados."
    }
  ],
  "meta": {
    "request_id": "c5f9a8d0-5c78-4fbe-9b5c-14c9c6ee3f3e",
    "took_ms": 42,
    "cached": false,
    "source": "srd"
  }
}
```

### Estruturas de Dados
- `description.excerpt`: string com até 2 parágrafos concatenados.
- `description.raw`: array de parágrafos originais da SRD; incluído apenas se `include_raw=true`.
- `references[]`: lista opcional. Quando a fonte não é 100% SRD (ex.: PHB), incluir `origin: "non-srd"` e manter campo opcional.

### Respostas Especiais
- `not_found` (`404`): `data` omitido. Opcional `warnings` com `NO_RESULTS`.
- `ambiguous` (`200`): `data` é `{ "candidates": [{"slug","name","domain","ref"}, ...] }`.
- `refuse` (`422`): parâmetros inválidos; `warnings` com `INVALID_PARAMS`.
- `error` (`502`): falha upstream ou timeout; `warnings` com `UPSTREAM_FAILURE` etc.

## Status HTTP x Status Lógico
| Status lógico | HTTP | Condição |
| --- | --- | --- |
| `ok` | 200 | Resultado encontrado. |
| `not_found` | 404 | Nenhum resultado para `ref`/`slug`/`q`. |
| `ambiguous` | 200 | Múltiplos candidatos para `slug` prefixado. |
| `refuse` | 422 | Validação ou política violada. |
| `error` | 502 | Erro interno ou dependência externa. |

## Política de Cache
- **Chave**: `rules:{domain|*}:{slug|""}:{hash(q)}:{limit}` (usar SHA1 em `q` normalizado; vazio para ausência).
- **TTL**: 6 horas.
- **Stale-While-Revalidate**: 5 minutos adicionais após expiração, permitindo servir dados obsoletos enquanto revalidação ocorre em background.
- **Invalidação**: manual via job de sincronização da SRD, auto-expiração ou purge seletivo quando `last_synced_at` for atualizado.
- **Pré-aquecimento**: carregar `advantage-and-disadvantage`, `ability-checks`, `conditions`.

## Arquivos a Criar/Editar
- `docs/contracts/rules_query.md`: contrato detalhado do endpoint.
- `src/app/presentation/api/routers/rules.py`: definição do router FastAPI para `/rules/query`.
- `src/app/application/services/rules_service.py`: lógica de orquestração de consultas, normalização e cache.
- `src/app/infrastructure/repositories/srd_client.py`: cliente da 5e-SRD API + camadas de busca/ref/slug.
- `src/app/infrastructure/cache/memory_cache.py`: implementação de cache em memória com TTL + stale-while-revalidate.
- `tests/integration/test_rules_query.py`: testes de integração do endpoint.
- `tests/manual/rules_query_checklist.md`: roteiro de verificação manual conforme critérios de aceitação.

## Checklist de Teste Manual
1. `GET /rules/query?ref=/api/rules/ability-checks` → `200 ok`, `data[0].ref` presente.
2. `GET /rules/query?slug=ability-checks` → mesmo resultado.
3. `GET /rules/query?q=ability&limit=3` → `200 ok`, `warnings.PARTIAL_MATCH` quando aplicável.
4. `GET /rules/query?slug=inexistente` → `404 not_found`.
5. `GET /rules/query?slug=abil` → `200 ambiguous` com `data.candidates[]`.
6. Segunda chamada com mesmos parâmetros → `meta.cached=true`.
7. `GET /rules/query?limit=50` → `422 refuse`.

## Riscos e Pendências
- Garantir consistência entre cache e dados da SRD durante janela stale-while-revalidate.
- Dependência da 5e-SRD API: variações de schema ou indisponibilidade podem aumentar ocorrência de `error`.
- Necessidade de job de pré-aquecimento das referências definidas.
- Definição futura de domínios adicionais exigirá ajuste no contrato.

## Perguntas Abertas
1. A política de pré-aquecimento (execução e agendamento) será responsabilidade deste serviço ou de um job externo?
2. Deseja logs específicos quando um item for servido no modo stale-while-revalidate?
