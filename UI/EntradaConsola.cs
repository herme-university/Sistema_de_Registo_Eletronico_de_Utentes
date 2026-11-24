using System.Text;
using RegistoUtentes.Common;
using RegistoUtentes.Validation;

namespace RegistoUtentes.UI;

/// <summary>Leitura validada de dados da consola. Não contém regras de negócio.</summary>
public static class EntradaConsola
{
    public static string LerLinha(string mensagem)
    {
        Console.Write(mensagem);
        return (Console.ReadLine() ?? "").Trim();
    }

    public static string LerTexto(string mensagem, bool obrigatorio = true)
    {
        while (true)
        {
            string valor = LerLinha(mensagem);
            if (valor.Length > 0 || !obrigatorio) return valor;
            Erro("Campo obrigatório.");
        }
    }

    public static string LerData(string mensagem, bool vazioEHoje = false)
    {
        while (true)
        {
            string valor = LerLinha(mensagem);
            if (vazioEHoje && valor.Length == 0) return Formatos.DataDeHoje();
            if (Validador.DataValida(valor, out DateTime data))
                return data.ToString(Formatos.Data, Formatos.Pt);
            Erro("Data inválida. Use o formato DD/MM/AAAA (não pode ser futura).");
        }
    }

    public static string LerOpcao(string mensagem, params string[] opcoes)
    {
        while (true)
        {
            string valor = LerLinha(mensagem).ToUpperInvariant();
            if (opcoes.Contains(valor)) return valor;
            Erro("Opção inválida. Escolha entre: " + string.Join("/", opcoes));
        }
    }

    /// <summary>Lê um PIN sem o mostrar no ecrã (asteriscos).</summary>
    public static string LerSegredo(string mensagem)
    {
        Console.Write(mensagem);

        // Com a entrada redirecionada (testes automáticos) não há teclas para mascarar.
        if (Console.IsInputRedirected)
            return (Console.ReadLine() ?? "").Trim();

        var texto = new StringBuilder();
        while (true)
        {
            ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);
            if (tecla.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return texto.ToString().Trim();
            }
            if (tecla.Key == ConsoleKey.Backspace)
            {
                if (texto.Length > 0)
                {
                    texto.Length--;
                    Console.Write("\b \b");
                }
                continue;
            }
            if (!char.IsControl(tecla.KeyChar))
            {
                texto.Append(tecla.KeyChar);
                Console.Write('*');
            }
        }
    }

    public static void Erro(string mensagem) => Console.WriteLine("  " + mensagem);
}
