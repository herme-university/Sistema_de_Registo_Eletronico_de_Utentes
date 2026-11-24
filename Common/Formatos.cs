using System.Globalization;

namespace RegistoUtentes.Common;

/// <summary>Formatos e funções de datas partilhados por todo o sistema.</summary>
public static class Formatos
{
    public const string Data = "dd/MM/yyyy";
    public const string DataHora = "dd/MM/yyyy HH:mm";
    public static readonly CultureInfo Pt = new("pt-PT");

    public static string DataDeHoje() => DateTime.Now.ToString(Data, Pt);

    public static string DataHoraAgora() => DateTime.Now.ToString(DataHora, Pt);

    public static DateTime LerData(string texto) => DateTime.ParseExact(texto, Data, Pt);

    public static int Idade(string dataNascimento)
    {
        DateTime nascimento = LerData(dataNascimento);
        DateTime hoje = DateTime.Today;
        int idade = hoje.Year - nascimento.Year;
        if (nascimento.Date > hoje.AddYears(-idade)) idade--;
        return idade;
    }
}
