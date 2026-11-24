using RegistoUtentes.Services;

namespace RegistoUtentes.UI.Ecras;

public class AutenticacaoEcra
{
    private const int MaximoTentativas = 3;
    private readonly AutenticacaoService _autenticacao;

    public AutenticacaoEcra(AutenticacaoService autenticacao) => _autenticacao = autenticacao;

    public bool Executar()
    {
        Console.WriteLine("\n=== AUTENTICAÇÃO ===");
        for (int tentativa = 0; tentativa < MaximoTentativas; tentativa++)
        {
            string login = EntradaConsola.LerTexto("Utilizador: ");
            string pin = EntradaConsola.LerSegredo("PIN: ");

            var resultado = _autenticacao.Autenticar(login, pin);
            if (resultado.Sucesso) return true;

            EntradaConsola.Erro(resultado.Erro!);
        }
        return false;
    }
}
