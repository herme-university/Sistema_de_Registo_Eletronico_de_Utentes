using RegistoUtentes.Common;
using RegistoUtentes.Models.Dtos;
using RegistoUtentes.Services;

namespace RegistoUtentes.UI.Ecras;

public class ConsultaEcra : EcraBase
{
    private readonly ConsultaService _consultas;

    public ConsultaEcra(ConsultaService consultas, AutorizacaoService autorizacao) : base(autorizacao)
    {
        _consultas = consultas;
    }

    public void Registar()
    {
        if (!Autorizado(Permissao.RegistarConsulta)) return;

        var busca = _consultas.ObterUtente(PedirNumeroRegisto());
        if (!busca.Sucesso)
        {
            EntradaConsola.Erro(busca.Erro!);
            return;
        }
        var utente = busca.Valor!;

        Console.WriteLine($"\n--- Nova consulta para {utente.Nome} ---");
        var dto = new NovaConsultaDto(
            Data: EntradaConsola.LerData("Data da consulta (DD/MM/AAAA, Enter = hoje): ", vazioEHoje: true),
            Motivo: EntradaConsola.LerTexto("Motivo da consulta: "),
            Diagnostico: EntradaConsola.LerTexto("Diagnóstico: "),
            Tratamento: EntradaConsola.LerTexto("Tratamento/prescrição: ", obrigatorio: false),
            Observacoes: EntradaConsola.LerTexto("Outras informações clínicas: ", obrigatorio: false));

        var resultado = _consultas.RegistarConsulta(utente.NumeroRegisto, dto);
        if (!resultado.Sucesso)
        {
            EntradaConsola.Erro(resultado.Erro!);
            return;
        }
        Console.WriteLine("  Consulta registada com sucesso.");
    }

    public void VerHistorico()
    {
        if (!Autorizado(Permissao.VerHistorico)) return;

        var resultado = _consultas.ObterHistorico(PedirNumeroRegisto());
        if (!resultado.Sucesso)
        {
            EntradaConsola.Erro(resultado.Erro!);
            return;
        }

        var historico = resultado.Valor!;
        Console.WriteLine($"\n--- Histórico clínico: {historico.Utente.Nome} ({historico.Utente.NumeroRegisto}) ---");
        if (historico.Consultas.Count == 0)
        {
            Console.WriteLine("  Sem consultas registadas.");
            return;
        }

        for (int i = 0; i < historico.Consultas.Count; i++)
        {
            var c = historico.Consultas[i];
            Console.WriteLine($"\n[{i + 1}] {c.Data}  —  {c.Profissional}");
            Console.WriteLine($"    Motivo       : {c.Motivo}");
            Console.WriteLine($"    Diagnóstico  : {c.Diagnostico}");
            Console.WriteLine($"    Tratamento   : {(c.Tratamento.Length > 0 ? c.Tratamento : "-")}");
            Console.WriteLine($"    Observações  : {(c.Observacoes.Length > 0 ? c.Observacoes : "-")}");
        }
    }
}
