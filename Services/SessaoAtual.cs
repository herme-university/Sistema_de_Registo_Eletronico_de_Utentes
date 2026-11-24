using RegistoUtentes.Models;

namespace RegistoUtentes.Services;

/// <summary>Guarda o utilizador autenticado durante a execução do programa.</summary>
public class SessaoAtual
{
    public Utilizador? Utilizador { get; private set; }

    public bool Autenticada => Utilizador != null;

    /// <summary>Texto usado na auditoria, por exemplo "Dr. Médico (medico)".</summary>
    public string Descricao => Utilizador != null
        ? $"{Utilizador.Nome} ({Utilizador.Nivel})"
        : "desconhecido";

    public void Iniciar(Utilizador utilizador) => Utilizador = utilizador;

    public void Terminar() => Utilizador = null;
}
