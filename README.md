# LoreWeaver

Ferramenta para a mesa de D&D 5e: fichas de personagem, mundos, busca de conteúdo SRD e sessão de jogo com vídeo/voz/tela em grupo. Projeto único em ASP.NET Core + Blazor Server, deploy em home server próprio via Cloudflare Tunnel.

## Stack

- **ASP.NET Core + Blazor Server** (.NET 10, renderização e lógica no servidor — nada de API pública exposta ao client)
- **PostgreSQL** via EF Core (Npgsql)
- **5e SRD API** (dnd5eapi.co) consultada via proxy/cache server-side — o client nunca fala com ela diretamente
- **LiveKit + coturn** (Docker, self-hosted no home server) — SFU para vídeo/voz/compartilhamento de tela da sessão de jogo, coturn para TURN/STUN (NAT traversal em rede residencial)

## Rodando localmente

```bash
# 1. Configurar a connection string (fora do repo, via user-secrets)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=loreweaver;Username=loreweaver;Password=<sua-senha>"

# 2. Aplicar as migrations num PostgreSQL local
dotnet ef database update

# 3. Rodar
dotnet run
```

Para testar a página `/session` (vídeo/voz/tela) localmente também é preciso um LiveKit acessível e as secrets correspondentes. Para um teste rápido, sem subir o stack completo de `deploy/livekit/`:

```bash
docker run --rm -p 7880:7880 -p 7881:7881 livekit/livekit-server \
  --dev --keys "chave-de-teste: segredo-de-teste-com-pelo-menos-32-bytes"

dotnet user-secrets set "LiveKit:ApiKey" "chave-de-teste"
dotnet user-secrets set "LiveKit:ApiSecret" "segredo-de-teste-com-pelo-menos-32-bytes"
dotnet user-secrets set "LiveKit:ApiUrl" "http://localhost:7880"
dotnet user-secrets set "LiveKit:WebSocketUrl" "ws://localhost:7880"
```

`ApiSecret` precisa ter pelo menos 256 bits (32 bytes) — o SDK .NET rejeita segredos mais curtos na inicialização.

## Deploy em produção

```bash
cp .env.example .env   # preencher POSTGRES_* (e LIVEKIT_* se a stack de LiveKit já estiver no ar)
docker compose up -d --build

# migrations não rodam sozinhas no container (sem dotnet-ef na imagem de runtime) —
# aplicar manualmente contra o Postgres do compose, ex.:
dotnet ef database update --connection "Host=localhost;Port=5432;Database=loreweaver;Username=loreweaver;Password=<senha-do-.env>"
```

`docker-compose.yml` na raiz sobe só a app + Postgres dela. LiveKit/coturn (`deploy/livekit/`) são uma stack separada — usam `network_mode: host` e por isso não entram nesse mesmo compose. A porta é controlada por uma única variável (`APP_PORT` no `.env`), repassada tanto para `ASPNETCORE_URLS` dentro do container quanto para o mapeamento de porta do host.

## Estrutura

- `Components/Pages/` — páginas Razor (Mundos, Personagens, Regras, NPCs, Encontros, Sessão)
- `Components/Comms/` — componente de chamada (`VideoCall.razor`, integração via `IJSRuntime` com `livekit-client`)
- `Features/` — lógica de domínio por área (`Characters`, `Npcs`, `Encounters`, `Rules`, `Comms`, `Common`)
- `Features/Characters/Catalog/` — dados de classes/raças/perícias/equipamento da SRD 2014, embutidos (ported de `5e-bits/5e-database`, sem dependência de rede — classes/raças/perícias como arrays C#, equipamento como JSON embutido dado o volume: 237 itens)
- `Data/` — `DbContext`, entidades e migrations do EF Core
- `Dockerfile`, `docker-compose.yml` — build multi-stage da app + serviço Postgres para o deploy em produção (ver seção acima)
- `deploy/livekit/` — `docker-compose.yml` do LiveKit + coturn para o home server (não usado localmente, só no deploy real)
- `wwwroot/vendor/livekit-client/` — bundle ESM do `livekit-client` vendorizado no repo (sem CDN, sem bundler no pipeline do Blazor Server)

## Funcionalidades

- **Busca de Regras:** proxy/cache server-side para a SRD (termo ou slug), com TTL de 6h e stale-while-revalidate de 5min.
- **Gerador de NPCs:** geração determinística local com seed e re-roll, rotulado como Homebrew.
- **Compositor de Encontros:** orçamento de XP aproximado (tabelas do DMG) e criaturas sugeridas com links canônicos da SRD.
- **Mundos e Personagens:** CRUD básico de mundos e ficha mecânica de personagem — atributos, multiclasse, PV/bônus de proficiência com override manual, perícias e salvaguardas. Classe/raça aceitam SRD ou texto livre (homebrew).
- **Inventário e CA:** itens de equipamento (SRD ou homebrew), slots (armadura/escudo/mãos), moeda nas 5 denominações. CA real calculada a partir da armadura/escudo equipados (leve/média/pesada, cap de DEX conforme categoria), com override manual sempre prevalecendo. Peso/carga não é rastreado (decisão explícita).
- **Sessão de jogo (vídeo/voz/tela):** um room novo no LiveKit é criado sob demanda por sessão (não existe room fixo do grupo), com simulcast habilitado para o compartilhamento de tela. A página `/session` ainda não tem controle de acesso próprio — qualquer um que chegue nela entra digitando um nome, sem gate (autenticação real fica para depois, ver Roadmap).

## Roadmap

- Magias / spellbook (incl. multiclass spellcasting)
- Autenticação simplificada para o grupo (convite/senha + Cloudflare Access) — hoje `/session` não tem controle de acesso próprio
- Música de ambientação: mixagem server-side via Spotify/YouTube não é viável (ambos bloqueiam captura/redirecionamento de áudio bruto por DRM/ToS) — abordagem ainda em aberto

## Histórico

Este projeto já passou por três tentativas anteriores (SPA React, API em camadas .NET, protótipo FastAPI) — todas preservadas no histórico do git (`git log --all`) caso valha revisitar alguma decisão de design.
