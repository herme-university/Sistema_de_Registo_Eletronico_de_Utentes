using RegistoUtentes.Models;
using RegistoUtentes.Repositories;

namespace RegistoUtentes.Services;

/// <summary>Regista quem fez o quê e quando, usando o utilizador da sessão atual.</summary>
public class AuditoriaService
{
    private readonly IAuditoriaRepository _repositorio;
    private readonly SessaoAtual _sessao;

    public AuditoriaService(IAuditoriaRepository repositorio, SessaoAtual sessao)
    {
        _repositorio = repositorio;
        _sessao = sessao;
    }

    public void Registar(string accao, string detalhe = "") =>
        _repositorio.Registar(new RegistoAuditoria(DateTime.Now, _sessao.Descricao, accao, detalhe));
}
