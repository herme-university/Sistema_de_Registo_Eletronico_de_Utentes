namespace RegistoUtentes.Models.Dtos;

/// <summary>Dados recolhidos pela interface para registar uma consulta.</summary>
public record NovaConsultaDto(
    string Data,
    string Motivo,
    string Diagnostico,
    string Tratamento,
    string Observacoes);
