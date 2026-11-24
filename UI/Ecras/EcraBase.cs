using RegistoUtentes.Common;
using RegistoUtentes.Services;

namespace RegistoUtentes.UI.Ecras;

public abstract class EcraBase
{
    private readonly AutorizacaoService _autorizacao;

    protected EcraBase(AutorizacaoService autorizacao) => _autorizacao = autorizacao;

    /// <summary>
    /// Verifica a permissão antes de pedir dados, para o utilizador não preencher
    /// um formulário inteiro e só depois ser recusado. Os serviços voltam a verificar.
    /// </summary>
    protected bool Autorizado(Permissao permissao)
    {
        var resultado = _autorizacao.Exigir(permissao);
        if (!resultado.Sucesso) EntradaConsola.Erro(resultado.Erro!);
        return resultado.Sucesso;
    }

    protected static string PedirNumeroRegisto() =>
        EntradaConsola.LerLinha("Número de registo (ex.: UT-2026-0001): ").ToUpperInvariant();
}
