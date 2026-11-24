using RegistoUtentes.Common;
using RegistoUtentes.Security;

namespace RegistoUtentes.Services;

/// <summary>Verifica se o utilizador da sessão pode executar uma operação e audita as recusas.</summary>
public class AutorizacaoService
{
    private readonly SessaoAtual _sessao;
    private readonly AuditoriaService _auditoria;

    public AutorizacaoService(SessaoAtual sessao, AuditoriaService auditoria)
    {
        _sessao = sessao;
        _auditoria = auditoria;
    }

    public Resultado Exigir(Permissao permissao)
    {
        var utilizador = _sessao.Utilizador;
        if (utilizador != null && PermissoesPorNivel.Permite(utilizador.Nivel, permissao))
            return Resultado.Ok();

        _auditoria.Registar("ACESSO_NEGADO", permissao.ToString());
        return Resultado.Falha("Acesso negado: o seu nível não permite esta operação.");
    }
}
