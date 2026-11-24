namespace RegistoUtentes.Models;

public class Utilizador
{
    public string Login { get; set; } = "";
    public string Nome { get; set; } = "";
    public string Nivel { get; set; } = "";
    public string PinHash { get; set; } = "";
}
