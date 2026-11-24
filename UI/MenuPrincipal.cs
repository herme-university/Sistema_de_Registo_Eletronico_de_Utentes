using RegistoUtentes.Services;
using RegistoUtentes.UI.Ecras;

namespace RegistoUtentes.UI;

public class MenuPrincipal
{
    private readonly SessaoAtual _sessao;
    private readonly AutenticacaoService _autenticacao;
    private readonly List<(string Titulo, Action Executar)> _acoes;

    public MenuPrincipal(
        SessaoAtual sessao,
        AutenticacaoService autenticacao,
        UtenteEcra utentes,
        ConsultaEcra consultas)
    {
        _sessao = sessao;
        _autenticacao = autenticacao;
        _acoes = new()
        {
            ("Registar novo utente", utentes.Registar),
            ("Ver dados pessoais de um utente", utentes.VerDados),
            ("Pesquisar utente por NIF", utentes.PesquisarPorNif),
            ("Registar consulta", consultas.Registar),
            ("Consultar histórico clínico", consultas.VerHistorico),
            ("Listar todos os utentes (admin)", utentes.Listar)
        };
    }

    public void Executar()
    {
        while (true)
        {
            var utilizador = _sessao.Utilizador!;
            Console.WriteLine($"\n===== REGISTO ELETRÓNICO DE UTENTES | {utilizador.Nome} [{utilizador.Nivel}] =====");
            for (int i = 0; i < _acoes.Count; i++)
                Console.WriteLine($" {i + 1}. {_acoes[i].Titulo}");
            Console.WriteLine(" 0. Sair");

            string escolha = EntradaConsola.LerLinha("Opção: ");

            if (escolha == "0")
            {
                _autenticacao.TerminarSessao();
                Console.WriteLine("Sessão terminada.");
                return;
            }

            if (int.TryParse(escolha, out int n) && n >= 1 && n <= _acoes.Count)
                _acoes[n - 1].Executar();
            else
                EntradaConsola.Erro("Opção inválida.");
        }
    }
}
