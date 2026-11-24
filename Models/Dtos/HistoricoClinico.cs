namespace RegistoUtentes.Models.Dtos;

/// <summary>Utente e as suas consultas, da mais recente para a mais antiga.</summary>
public record HistoricoClinico(Utente Utente, IReadOnlyList<Consulta> Consultas);
