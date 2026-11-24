using RegistoUtentes.Models;

namespace RegistoUtentes.Repositories;

public interface IAuditoriaRepository
{
    void Registar(RegistoAuditoria registo);
}
