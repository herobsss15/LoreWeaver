# LoreWeaver

Aplicativo SPA em React voltado para mesas de D&D 5e, consumindo diretamente a [5e SRD API](https://www.dnd5eapi.co/).

## Scripts

- `npm run dev` – ambiente de desenvolvimento com Vite.
- `npm run build` – build de produção.
- `npm run preview` – pré-visualização do build (também usado no Railway).

## Deploy no Railway

O arquivo [`railway.json`](railway.json) configura uma fase de build com Vite e inicializa o serviço com `vite preview` escutando em `0.0.0.0`. Basta criar um projeto Node no Railway e apontar para este repositório.

## Funcionalidades

- **Busca de Regras:** pesquisa por termo ou slug na SRD, com estados de carregamento e múltiplos resultados.
- **Gerador de NPCs:** geração determinística local com seed e re-roll, rotulando como Homebrew.
- **Compositor de Encontros:** orçamento de XP aproximado e criaturas sugeridas com links canônicos carregados da API da SRD.
- **Histórico Local:** últimos 10 resultados salvos com reabertura.

O tema padrão é escuro e componentes possuem rótulos para acessibilidade básica.
