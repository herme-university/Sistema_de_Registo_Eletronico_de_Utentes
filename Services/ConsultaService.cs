using RegistoUtentes.Common;
using RegistoUtentes.Models;
using RegistoUtentes.Models.Dtos;
using RegistoUtentes.Repositories;
using RegistoUtentes.Validation;

namespace RegistoUtentes.Services;

/// <summary>Regras de negócio sobre consultas e histórico clínico.</summary>
public class ConsultaService
{
    private readonly IUtenteRepository _utentes;
    private readonly AutorizacaoService _autorizacao;
    private readonly AuditoriaService _auditoria;
    private readonly SessaoAtual _sessao;

    public ConsultaService(
        IUtenteRepository utentes,
        AutorizacaoService autorizacao,
        AuditoriaService auditoria,
        SessaoAtual sessao)
    {
        _utentes = utentes;
        _autorizacao = autorizacao;
        _auditoria = auditoria;
        _sessao = sessao;
    }

    public Resultado<Utente> ObterUtente(string numeroRegisto)
    {
        var utente = _utentes.ObterPorNumero(numeroRegisto.Trim().ToUpperInvariant());
        return utente == null
            ? Resultado<Utente>.Falha("Utente não encontrado.")
            : Resultado<Utente>.Ok(utente);
    }

    public Resultado RegistarConsulta(string numeroRegisto, NovaConsultaDto dto)
    {
        var permissao = _autorizacao.Exigir(Permissao.RegistarConsulta);
        if (!permissao.Sucesso) return permissao;

        var busca = ObterUtente(numeroRegisto);
        if (!busca.Sucesso) return Resultado.Falha(busca.Erro!);
        var utente = busca.Valor!;

        if (!Validador.DataValida(dto.Data, out DateTime data))
            return Resultado.Falha("Data inválida. Use DD/MM/AAAA (não pode ser futura).");

        if (string.IsNullOrWhiteSpace(dto.Motivo) || string.IsNullOrWhiteSpace(dto.Diagnostico))
            return Resultado.Falha("Motivo e diagnóstico são obrigatórios.");

        utente.Historico.Add(new Consulta
        {
            Data = data.ToString(Formatos.Data, Formatos.Pt),
            Profissional = _sessao.Utilizador!.Nome,
            Motivo = dto.Motivo.Trim(),
            Diagnostico = dto.Diagnostico.Trim(),
            Tratamento = dto.Tratamento.Trim(),
            Observacoes = dto.Observacoes.Trim()
        });

        _utentes.Atualizar(utente);
        _auditoria.Registar("REGISTO_CONSULTA", utente.NumeroRegisto);
        return Resultado.Ok();
    }

    public Resultado<HistoricoClinico> ObterHistorico(string numeroRegisto)
    {
        var permissao = _autorizacao.Exigir(Permissao.VerHistorico);
        if (!permissao.Sucesso) return Resultado<HistoricoClinico>.Falha(permissao.Erro!);

        var busca = ObterUtente(numeroRegisto);
        if (!busca.Sucesso) return Resultado<HistoricoClinico>.Falha(busca.Erro!);
        var utente = busca.Valor!;

        _auditoria.Registar("VER_HISTORICO", utente.NumeroRegisto);

        // Da consulta mais recente para a mais antiga
        var consultas = utente.Historico
            .OrderByDescending(c => Formatos.LerData(c.Data))
            .ToList();

        return Resultado<HistoricoClinico>.Ok(new HistoricoClinico(utente, consultas));
    }
}
