using RegistoUtentes.Models;

namespace RegistoUtentes.Repositories;

public interface IUtenteRepository
{
    IReadOnlyList<Utente> ObterTodos();
    Utente? ObterPorNumero(string numeroRegisto);
    Utente? ObterPorNif(string nif);

    /// <summary>Próximo número de sequência disponível para o ano indicado (UT-AAAA-NNNN).</summary>
    int ProximaSequencia(int ano);

    void Adicionar(Utente utente);
    void Atualizar(Utente utente);
}
