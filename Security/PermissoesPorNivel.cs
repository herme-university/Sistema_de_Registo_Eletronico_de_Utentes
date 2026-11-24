using RegistoUtentes.Common;

namespace RegistoUtentes.Security;

/// <summary>Matriz de permissões: o que cada nível de acesso pode fazer.</summary>
public static class PermissoesPorNivel
{
    private static readonly Dictionary<string, HashSet<Permissao>> Mapa = new()
    {
        [NiveisAcesso.Recepcao] = new()
        {
            Permissao.RegistarUtente,
            Permissao.VerDadosPessoais
        },
        [NiveisAcesso.Enfermeiro] = new()
        {
            Permissao.VerDadosPessoais,
            Permissao.VerHistorico
        },
        [NiveisAcesso.Medico] = new()
        {
            Permissao.VerDadosPessoais,
            Permissao.VerHistorico,
            Permissao.RegistarConsulta
        },
        [NiveisAcesso.Admin] = new()
        {
            Permissao.RegistarUtente,
            Permissao.VerDadosPessoais,
            Permissao.VerHistorico,
            Permissao.RegistarConsulta,
            Permissao.ListarUtentes
        }
    };

    public static bool Permite(string nivel, Permissao permissao) =>
        Mapa.TryGetValue(nivel, out var permissoes) && permissoes.Contains(permissao);
}
