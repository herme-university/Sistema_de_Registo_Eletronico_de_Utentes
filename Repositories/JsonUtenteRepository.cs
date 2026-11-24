using RegistoUtentes.Infrastructure;
using RegistoUtentes.Models;

namespace RegistoUtentes.Repositories;

public class JsonUtenteRepository : IUtenteRepository
{
    private readonly string _caminho;
    private readonly Dictionary<string, Utente> _utentes;

    public JsonUtenteRepository(string caminho)
    {
        _caminho = caminho;
        _utentes = ArmazemJson.Ler(caminho, () => new Dictionary<string, Utente>());
    }

    public IReadOnlyList<Utente> ObterTodos() =>
        _utentes.Values.OrderBy(u => u.NumeroRegisto, StringComparer.Ordinal).ToList();

    public Utente? ObterPorNumero(string numeroRegisto) =>
        _utentes.TryGetValue(numeroRegisto, out var utente) ? utente : null;

    public Utente? ObterPorNif(string nif) =>
        string.IsNullOrEmpty(nif) ? null : _utentes.Values.FirstOrDefault(u => u.Nif == nif);

    public int ProximaSequencia(int ano)
    {
        string prefixo = $"UT-{ano}-";
        int maior = _utentes.Keys
            .Where(k => k.StartsWith(prefixo, StringComparison.Ordinal))
            .Select(k => int.TryParse(k[prefixo.Length..], out int n) ? n : 0)
            .DefaultIfEmpty(0)
            .Max();
        return maior + 1;
    }

    public void Adicionar(Utente utente)
    {
        _utentes[utente.NumeroRegisto] = utente;
        Guardar();
    }

    public void Atualizar(Utente utente)
    {
        _utentes[utente.NumeroRegisto] = utente;
        Guardar();
    }

    private void Guardar() => ArmazemJson.Gravar(_caminho, _utentes);
}
