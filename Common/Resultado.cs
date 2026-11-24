namespace RegistoUtentes.Common;

/// <summary>
/// Resultado de uma operação de negócio. Os serviços devolvem este tipo em vez de
/// escreverem na consola, o que mantém a lógica separada da interface.
/// </summary>
public class Resultado
{
    public bool Sucesso { get; }
    public string? Erro { get; }

    protected Resultado(bool sucesso, string? erro)
    {
        Sucesso = sucesso;
        Erro = erro;
    }

    public static Resultado Ok() => new(true, null);
    public static Resultado Falha(string erro) => new(false, erro);
}

public sealed class Resultado<T> : Resultado
{
    public T? Valor { get; }

    private Resultado(bool sucesso, T? valor, string? erro) : base(sucesso, erro)
    {
        Valor = valor;
    }

    public static Resultado<T> Ok(T valor) => new(true, valor, null);
    public static new Resultado<T> Falha(string erro) => new(false, default, erro);
}
