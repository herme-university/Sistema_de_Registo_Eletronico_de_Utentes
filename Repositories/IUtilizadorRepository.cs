using RegistoUtentes.Models;

namespace RegistoUtentes.Repositories;

public interface IUtilizadorRepository
{
    IReadOnlyList<Utilizador> ObterTodos();
    Utilizador? ObterPorLogin(string login);
    void Adicionar(Utilizador utilizador);
}
