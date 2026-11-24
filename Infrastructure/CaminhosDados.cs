namespace RegistoUtentes.Infrastructure;

/// <summary>Localização dos ficheiros de dados (por omissão, a pasta do executável).</summary>
public record CaminhosDados(string Utentes, string Utilizadores, string Auditoria)
{
    public static CaminhosDados NaPasta(string pasta) => new(
        Path.Combine(pasta, "utentes.json"),
        Path.Combine(pasta, "utilizadores.json"),
        Path.Combine(pasta, "auditoria.log"));
}
