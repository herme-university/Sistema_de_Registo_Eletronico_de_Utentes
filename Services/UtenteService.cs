using RegistoUtentes.Common;
using RegistoUtentes.Models;
using RegistoUtentes.Models.Dtos;
using RegistoUtentes.Repositories;
using RegistoUtentes.Validation;

namespace RegistoUtentes.Services;

/// <summary>Regras de negócio sobre utentes: registo, consulta de dados e pesquisa.</summary>
public class UtenteService
{
    private readonly IUtenteRepository _utentes;
    private readonly AutorizacaoService _autorizacao;
    private readonly AuditoriaService _auditoria;

    public UtenteService(IUtenteRepository utentes, AutorizacaoService autorizacao, AuditoriaService auditoria)
    {
        _utentes = utentes;
        _autorizacao = autorizacao;
        _auditoria = auditoria;
    }

    public Resultado<Utente> Registar(NovoUtenteDto dto)
    {
        var permissao = _autorizacao.Exigir(Permissao.RegistarUtente);
        if (!permissao.Sucesso) return Resultado<Utente>.Falha(permissao.Erro!);

        if (string.IsNullOrWhiteSpace(dto.Nome) ||
            string.IsNullOrWhiteSpace(dto.Telefone) ||
            string.IsNullOrWhiteSpace(dto.Morada))
            return Resultado<Utente>.Falha("Nome, telefone e morada são obrigatórios.");

        if (!Validador.DataValida(dto.DataNascimento, out DateTime nascimento))
            return Resultado<Utente>.Falha("Data de nascimento inválida. Use DD/MM/AAAA (não pode ser futura).");

        if (!Validador.GeneroValido(dto.Genero))
            return Resultado<Utente>.Falha("Género inválido. Escolha M ou F.");

        string nif = dto.Nif.Trim().ToUpperInvariant();
        if (nif.Length > 0)
        {
            if (!Validador.NifValido(nif))
                return Resultado<Utente>.Falha("NIF inválido (use 9 a 14 caracteres alfanuméricos).");

            var existente = _utentes.ObterPorNif(nif);
            if (existente != null)
                return Resultado<Utente>.Falha($"Este NIF já está associado ao registo {existente.NumeroRegisto}.");
        }

        int ano = DateTime.Now.Year;
        var utente = new Utente
        {
            NumeroRegisto = $"UT-{ano}-{_utentes.ProximaSequencia(ano):D4}",
            Nome = dto.Nome.Trim(),
            DataNascimento = nascimento.ToString(Formatos.Data, Formatos.Pt),
            Genero = dto.Genero,
            Telefone = dto.Telefone.Trim(),
            Morada = dto.Morada.Trim(),
            ContactoEmergencia = dto.ContactoEmergencia.Trim(),
            Nif = nif,
            DataRegisto = Formatos.DataHoraAgora()
        };

        _utentes.Adicionar(utente);
        _auditoria.Registar("REGISTO_UTENTE", utente.NumeroRegisto);
        return Resultado<Utente>.Ok(utente);
    }

    public Resultado<Utente> ObterDados(string numeroRegisto)
    {
        var permissao = _autorizacao.Exigir(Permissao.VerDadosPessoais);
        if (!permissao.Sucesso) return Resultado<Utente>.Falha(permissao.Erro!);

        var utente = _utentes.ObterPorNumero(numeroRegisto.Trim().ToUpperInvariant());
        if (utente == null) return Resultado<Utente>.Falha("Utente não encontrado.");

        _auditoria.Registar("VER_DADOS", utente.NumeroRegisto);
        return Resultado<Utente>.Ok(utente);
    }

    public Resultado<Utente> PesquisarPorNif(string nif)
    {
        var permissao = _autorizacao.Exigir(Permissao.VerDadosPessoais);
        if (!permissao.Sucesso) return Resultado<Utente>.Falha(permissao.Erro!);

        var utente = _utentes.ObterPorNif(nif.Trim().ToUpperInvariant());
        if (utente == null) return Resultado<Utente>.Falha("Nenhum utente com esse NIF.");

        _auditoria.Registar("PESQUISA_NIF", utente.NumeroRegisto);
        return Resultado<Utente>.Ok(utente);
    }

    public Resultado<IReadOnlyList<Utente>> Listar()
    {
        var permissao = _autorizacao.Exigir(Permissao.ListarUtentes);
        if (!permissao.Sucesso) return Resultado<IReadOnlyList<Utente>>.Falha(permissao.Erro!);

        _auditoria.Registar("LISTAR_UTENTES");
        return Resultado<IReadOnlyList<Utente>>.Ok(_utentes.ObterTodos());
    }
}
