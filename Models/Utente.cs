namespace RegistoUtentes.Models;

public class Utente
{
    public string NumeroRegisto { get; set; } = "";
    public string Nome { get; set; } = "";
    public string DataNascimento { get; set; } = "";
    public string Genero { get; set; } = "";
    public string Telefone { get; set; } = "";
    public string Morada { get; set; } = "";
    public string ContactoEmergencia { get; set; } = "";
    public string Nif { get; set; } = "";
    public string DataRegisto { get; set; } = "";
    public List<Consulta> Historico { get; set; } = new();
}
