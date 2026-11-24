namespace RegistoUtentes.Models;

/// <summary>Uma linha do registo de auditoria: quem fez o quê e quando.</summary>
public record RegistoAuditoria(DateTime DataHora, string Utilizador, string Accao, string Detalhe);
