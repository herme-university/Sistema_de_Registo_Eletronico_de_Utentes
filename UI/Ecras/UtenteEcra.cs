using RegistoUtentes.Common;
using RegistoUtentes.Models.Dtos;
using RegistoUtentes.Services;
using RegistoUtentes.Validation;

namespace RegistoUtentes.UI.Ecras;

public class UtenteEcra : EcraBase
{
    private readonly UtenteService _utentes;

    public UtenteEcra(UtenteService utentes, AutorizacaoService autorizacao) : base(autorizacao)
    {
        _utentes = utentes;
    }

    public void Registar()
    {
        if (!Autorizado(Permissao.RegistarUtente)) return;

        Console.WriteLine("\n--- Registo de novo utente ---");
        string nome = EntradaConsola.LerTexto("Nome completo: ");
        string dataNascimento = EntradaConsola.LerData("Data de nascimento (DD/MM/AAAA): ");
        string genero = EntradaConsola.LerOpcao("Género (M/F): ", "M", "F");
        string telefone = EntradaConsola.LerTexto("Telefone: ");
        string morada = EntradaConsola.LerTexto("Morada (bairro/município): ");
        string emergencia = EntradaConsola.LerTexto("Contacto de emergência (nome e telefone): ", obrigatorio: false);

        string nif = "";
        if (EntradaConsola.LerOpcao("O utente tem NIF/BI para associar? (S/N): ", "S", "N") == "S")
            nif = LerNif();

        var resultado = _utentes.Registar(
            new NovoUtenteDto(nome, dataNascimento, genero, telefone, morada, emergencia, nif));

        if (!resultado.Sucesso)
        {
            EntradaConsola.Erro(resultado.Erro!);
            return;
        }
        Console.WriteLine($"\n  Utente registado com sucesso! Número de registo: {resultado.Valor!.NumeroRegisto}");
    }

    public void VerDados()
    {
        if (!Autorizado(Permissao.VerDadosPessoais)) return;

        var resultado = _utentes.ObterDados(PedirNumeroRegisto());
        if (!resultado.Sucesso)
        {
            EntradaConsola.Erro(resultado.Erro!);
            return;
        }

        var u = resultado.Valor!;
        Console.WriteLine("\n--- Dados do utente ---");
        Console.WriteLine($"Nº de registo : {u.NumeroRegisto}");
        Console.WriteLine($"Nome          : {u.Nome}");
        Console.WriteLine($"Nascimento    : {u.DataNascimento} ({Formatos.Idade(u.DataNascimento)} anos)");
        Console.WriteLine($"Género        : {u.Genero}");
        Console.WriteLine($"Telefone      : {u.Telefone}");
        Console.WriteLine($"Morada        : {u.Morada}");
        Console.WriteLine($"Emergência    : {(u.ContactoEmergencia.Length > 0 ? u.ContactoEmergencia : "-")}");
        Console.WriteLine($"NIF           : {(u.Nif.Length > 0 ? u.Nif : "Não associado")}");
        Console.WriteLine($"Registado em  : {u.DataRegisto}");
        Console.WriteLine($"Nº consultas  : {u.Historico.Count}");
    }

    public void PesquisarPorNif()
    {
        if (!Autorizado(Permissao.VerDadosPessoais)) return;

        string nif = EntradaConsola.LerLinha("NIF a pesquisar: ");
        var resultado = _utentes.PesquisarPorNif(nif);
        if (!resultado.Sucesso)
        {
            EntradaConsola.Erro(resultado.Erro!);
            return;
        }
        Console.WriteLine($"  Encontrado: {resultado.Valor!.Nome} — registo {resultado.Valor.NumeroRegisto}");
    }

    public void Listar()
    {
        if (!Autorizado(Permissao.ListarUtentes)) return;

        var resultado = _utentes.Listar();
        if (!resultado.Sucesso)
        {
            EntradaConsola.Erro(resultado.Erro!);
            return;
        }

        Console.WriteLine($"\n--- Utentes registados ({resultado.Valor!.Count}) ---");
        foreach (var u in resultado.Valor)
            Console.WriteLine($"{u.NumeroRegisto}  {u.Nome,-35} NIF: {(u.Nif.Length > 0 ? u.Nif : "-")}");
    }

    private static string LerNif()
    {
        while (true)
        {
            string nif = EntradaConsola.LerLinha("NIF: ").ToUpperInvariant();
            if (Validador.NifValido(nif)) return nif;
            EntradaConsola.Erro("NIF inválido (use 9 a 14 caracteres alfanuméricos).");
        }
    }
}
