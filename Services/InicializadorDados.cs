using RegistoUtentes.Common;
using RegistoUtentes.Models;
using RegistoUtentes.Repositories;
using RegistoUtentes.Security;

namespace RegistoUtentes.Services;

/// <summary>Cria as contas de demonstração na primeira execução.</summary>
public static class InicializadorDados
{
    public static void GarantirContasDemonstracao(IUtilizadorRepository utilizadores)
    {
        if (utilizadores.ObterTodos().Count > 0) return;

        var contas = new[]
        {
            new Utilizador { Login = "admin",      Nome = "Administrador", Nivel = NiveisAcesso.Admin,      PinHash = PinHasher.Hash("1234") },
            new Utilizador { Login = "recepcao",   Nome = "Recepcionista", Nivel = NiveisAcesso.Recepcao,   PinHash = PinHasher.Hash("1111") },
            new Utilizador { Login = "enfermeiro", Nome = "Enfermeiro",    Nivel = NiveisAcesso.Enfermeiro, PinHash = PinHasher.Hash("2222") },
            new Utilizador { Login = "medico",     Nome = "Dr. Médico",    Nivel = NiveisAcesso.Medico,     PinHash = PinHasher.Hash("3333") }
        };

        foreach (var conta in contas)
            utilizadores.Adicionar(conta);
    }
}
