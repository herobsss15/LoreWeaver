# LoreWeaver

Ferramenta para a mesa de D&D 5e: fichas de personagem, mundos, busca de conteúdo SRD e (em breve) videoconferência em grupo. Projeto único em ASP.NET Core + Blazor Server, deploy em home server próprio via Cloudflare Tunnel.

## Stack

- **ASP.NET Core + Blazor Server** (.NET 10, renderização e lógica no servidor — nada de API pública exposta ao client)
- **PostgreSQL** via EF Core (Npgsql)
- **5e SRD API** (dnd5eapi.co) consultada via proxy/cache server-side — o client nunca fala com ela diretamente

## Rodando localmente

```bash
# 1. Configurar a connection string (fora do repo, via user-secrets)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=loreweaver;Username=loreweaver;Password=<sua-senha>"

# 2. Aplicar as migrations num PostgreSQL local
dotnet ef database update

# 3. Rodar
dotnet run
```

## Estrutura

- `Components/Pages/` — páginas Razor (Regras, NPCs, Encontros; fichas de personagem/mundos em desenvolvimento)
- `Features/` — lógica de domínio por área (`Npcs`, `Encounters`, `Rules`, `Common`)
- `Data/` — `DbContext`, entidades e migrations do EF Core

## Funcionalidades

- **Busca de Regras:** proxy/cache server-side para a SRD (termo ou slug), com TTL de 6h e stale-while-revalidate de 5min.
- **Gerador de NPCs:** geração determinística local com seed e re-roll, rotulado como Homebrew.
- **Compositor de Encontros:** orçamento de XP aproximado (tabelas do DMG) e criaturas sugeridas com links canônicos da SRD.
- **Mundos e Personagens:** entidades e persistência básicas (`Data/Entities`); a ficha mecânica completa (classe, raça, nível, atributos, PV, CA, inventário, magias) ainda não foi desenhada.

## Roadmap

- Ficha de personagem mecânica completa (CRUD)
- Autenticação simplificada para o grupo (convite/senha + Cloudflare Access)
- Videoconferência em grupo via LiveKit self-hosted (SFU) + coturn para TURN/STUN
- Streaming de música de ambientação mixado no mesmo room do LiveKit

## Histórico

Este projeto já passou por três tentativas anteriores (SPA React, API em camadas .NET, protótipo FastAPI) — todas preservadas no histórico do git (`git log --all`) caso valha revisitar alguma decisão de design.
