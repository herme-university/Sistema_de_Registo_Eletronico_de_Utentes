# Sistema de Registo Eletrónico de Utentes

Docente Email: doudouatungulu@gmail.com
Estudante: Hermenegildo Panzo — Docente: Atungulu Kabakaba

Aplicação de consola em C# (.NET 6 ou superior) para registar utentes, as suas consultas e controlar o acesso por nível, com registo de auditoria.

## Como executar

```
dotnet run
```

Contas de demonstração (criadas na primeira execução):

| Utilizador | PIN  | Nível      |
|------------|------|------------|
| admin      | 1234 | admin      |
| recepcao   | 1111 | recepcao   |
| enfermeiro | 2222 | enfermeiro |
| medico     | 3333 | medico     |

Os dados ficam em `utentes.json`, `utilizadores.json` e `auditoria.log`, na pasta do executável.

## Arquitetura

```
RegistoUtentes/
├── Program.cs              Raiz de composição: cria e liga os objetos
├── Models/                 Classes de dados
│   ├── Utente, Consulta, Utilizador, RegistoAuditoria
│   └── Dtos/               NovoUtenteDto, NovaConsultaDto, HistoricoClinico
├── Repositories/           Acesso aos dados (interface + implementação)
│   ├── IUtenteRepository      → JsonUtenteRepository
│   ├── IUtilizadorRepository  → JsonUtilizadorRepository
│   └── IAuditoriaRepository   → FicheiroAuditoriaRepository
├── Services/               Regras de negócio
│   ├── UtenteService, ConsultaService
│   ├── AutenticacaoService, AutorizacaoService, AuditoriaService
│   ├── SessaoAtual         Utilizador autenticado
│   └── InicializadorDados  Contas de demonstração
├── Security/               PinHasher e PermissoesPorNivel
├── Validation/             Validador (NIF, datas, género)
├── Infrastructure/         CaminhosDados e ArmazemJson
├── Common/                 Resultado, Permissao, NiveisAcesso, Formatos
└── UI/                     Interface de consola
    ├── EntradaConsola, MenuPrincipal
    └── Ecras/              AutenticacaoEcra, UtenteEcra, ConsultaEcra
```

### Regras de dependência

```
UI  →  Services  →  Repositories (interfaces)  →  Infrastructure
                 →  Security, Validation, Models, Common
```

- A **UI** só pede dados e mostra resultados; não contém regras de negócio.
- Os **Services** não escrevem na consola; devolvem um `Resultado` com sucesso ou erro.
- Os **Services** dependem das *interfaces* dos repositórios, por isso o armazenamento (JSON) pode ser trocado por uma base de dados sem alterar as regras.
