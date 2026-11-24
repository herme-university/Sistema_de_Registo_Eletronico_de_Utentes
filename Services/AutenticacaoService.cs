using RegistoUtentes.Common;
using RegistoUtentes.Repositories;
using RegistoUtentes.Security;

namespace RegistoUtentes.Services;

public class AutenticacaoService
{
    private readonly IUtilizadorRepository _utilizadores;
    private readonly SessaoAtual _sessao;
    private readonly AuditoriaService _auditoria;

    public AutenticacaoService(IUtilizadorRepository utilizadores, SessaoAtual sessao, AuditoriaService auditoria)
    {
        _utilizadores = utilizadores;
        _sessao = sessao;
        _auditoria = auditoria;
    }

    public Resultado Autenticar(string login, string pin)
    {
        var utilizador = _utilizadores.ObterPorLogin(login.Trim());
        if (utilizador == null || !PinHasher.Verificar(pin, utilizador.PinHash))
            return Resultado.Falha("Credenciais inválidas.");

        _sessao.Iniciar(utilizador);
        _auditoria.Registar("LOGIN");
        return Resultado.Ok();
    }

    public void TerminarSessao()
    {
        _auditoria.Registar("LOGOUT");
        _sessao.Terminar();
    }
}
