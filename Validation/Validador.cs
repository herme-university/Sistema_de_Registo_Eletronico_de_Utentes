using System.Globalization;
using System.Text.RegularExpressions;
using RegistoUtentes.Common;

namespace RegistoUtentes.Validation;

/// <summary>Regras de validação de dados, sem qualquer acesso a ficheiros ou consola.</summary>
public static class Validador
{
    private static readonly Regex NifRegex = new("^[A-Za-z0-9]{9,14}$", RegexOptions.Compiled);

    /// <summary>NIF/BI: 9 a 14 caracteres alfanuméricos.</summary>
    public static bool NifValido(string nif) => NifRegex.IsMatch(nif);

    /// <summary>Data no formato DD/MM/AAAA e que não seja futura.</summary>
    public static bool DataValida(string texto, out DateTime data) =>
        DateTime.TryParseExact(texto, Formatos.Data, Formatos.Pt, DateTimeStyles.None, out data)
        && data <= DateTime.Now;

    public static bool GeneroValido(string genero) => genero is "M" or "F";
}
