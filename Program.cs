// =============================================================================
// Sistema de Registo Eletrónico de Utentes  (Questão 2)
// Estudante: Hermenegildo Panzo
// Docente:   Atungulu Kabakaba
//
// Este ficheiro é apenas a "raiz de composição": cria os objetos de cada camada,
// liga-os entre si e arranca o menu. Toda a lógica está nas outras pastas:
//
//   Models/          classes de dados (Utente, Consulta, Utilizador, DTOs)
//   Repositories/    acesso aos dados (interfaces + implementação em JSON/ficheiro)
//   Services/        regras de negócio (utentes, consultas, autenticação, auditoria)
//   Security/        PIN (hash) e matriz de permissões por nível
//   Validation/      validação de NIF, datas e género
//   Infrastructure/  caminhos dos ficheiros e leitura/escrita JSON
//   Common/          tipos partilhados (Resultado, Permissao, NiveisAcesso, Formatos)
//   UI/              interface de consola (entrada, ecrãs e menu)
//
// Requer .NET 6 ou superior:  dotnet run
// =============================================================================

using System.Text;
using RegistoUtentes.Infrastructure;
using RegistoUtentes.Repositories;
using RegistoUtentes.Services;
using RegistoUtentes.UI;
using RegistoUtentes.UI.Ecras;

namespace RegistoUtentes;

public static class Program
{
    public static int Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        try
        {
            // Infraestrutura e repositórios
            var caminhos = CaminhosDados.NaPasta(AppContext.BaseDirectory);
            IUtenteRepository utentes = new JsonUtenteRepository(caminhos.Utentes);
            IUtilizadorRepository utilizadores = new JsonUtilizadorRepository(caminhos.Utilizadores);
            IAuditoriaRepository auditoriaRepositorio = new FicheiroAuditoriaRepository(caminhos.Auditoria);

            InicializadorDados.GarantirContasDemonstracao(utilizadores);

            // Serviços
            var sessao = new SessaoAtual();
            var auditoria = new AuditoriaService(auditoriaRepositorio, sessao);
            var autenticacao = new AutenticacaoService(utilizadores, sessao, auditoria);
            var autorizacao = new AutorizacaoService(sessao, auditoria);
            var utenteService = new UtenteService(utentes, autorizacao, auditoria);
            var consultaService = new ConsultaService(utentes, autorizacao, auditoria, sessao);

            // Interface
            var autenticacaoEcra = new AutenticacaoEcra(autenticacao);
            var menu = new MenuPrincipal(
                sessao,
                autenticacao,
                new UtenteEcra(utenteService, autorizacao),
                new ConsultaEcra(consultaService, autorizacao));

            if (!autenticacaoEcra.Executar())
            {
                Console.WriteLine("Demasiadas tentativas falhadas. A encerrar.");
                return 1;
            }

            menu.Executar();
            return 0;
        }
        catch (InvalidDataException ex)
        {
            Console.WriteLine($"Erro nos dados: {ex.Message}");
            Console.WriteLine("Corrija ou remova o ficheiro indicado e volte a executar.");
            return 2;
        }
    }
}
