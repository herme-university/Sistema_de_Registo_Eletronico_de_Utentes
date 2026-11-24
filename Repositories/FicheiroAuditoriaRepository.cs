using System.Text;
using RegistoUtentes.Models;

namespace RegistoUtentes.Repositories;

/// <summary>Guarda a auditoria em texto simples, uma linha por evento (auditoria.log).</summary>
public class FicheiroAuditoriaRepository : IAuditoriaRepository
{
    private readonly string _caminho;

    public FicheiroAuditoriaRepository(string caminho) => _caminho = caminho;

    public void Registar(RegistoAuditoria registo)
    {
        string linha =
            $"{registo.DataHora:yyyy-MM-dd HH:mm:ss} | {registo.Utilizador} | {registo.Accao} | {registo.Detalhe}{Environment.NewLine}";
        File.AppendAllText(_caminho, linha, Encoding.UTF8);
    }
}
