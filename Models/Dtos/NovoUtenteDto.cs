namespace RegistoUtentes.Models.Dtos;

/// <summary>Dados recolhidos pela interface para registar um utente.</summary>
public record NovoUtenteDto(
    string Nome,
    string DataNascimento,
    string Genero,
    string Telefone,
    string Morada,
    string ContactoEmergencia,
    string Nif);
