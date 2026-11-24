using RegistoUtentes.Infrastructure;
using RegistoUtentes.Models;

namespace RegistoUtentes.Repositories;

public class JsonUtilizadorRepository : IUtilizadorRepository
{
    private readonly string _caminho;
    private readonly List<Utilizador> _utilizadores;

    public JsonUtilizadorRepository(string caminho)
    {
        _caminho = caminho;
        _utilizadores = ArmazemJson.Ler(caminho, () => new List<Utilizador>());
    }

    public IReadOnlyList<Utilizador> ObterTodos() => _utilizadores.ToList();

    public Utilizador? ObterPorLogin(string login) =>
        _utilizadores.FirstOrDefault(u => string.Equals(u.Login, login, StringComparison.OrdinalIgnoreCase));

    public void Adicionar(Utilizador utilizador)
    {
        _utilizadores.Add(utilizador);
        ArmazemJson.Gravar(_caminho, _utilizadores);
    }
}
