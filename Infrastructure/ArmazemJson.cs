using System.Text.Json;

namespace RegistoUtentes.Infrastructure;

/// <summary>Leitura e escrita de ficheiros JSON, partilhada pelos repositórios.</summary>
public static class ArmazemJson
{
    private static readonly JsonSerializerOptions Opcoes = new() { WriteIndented = true };

    public static T Ler<T>(string caminho, Func<T> valorPorOmissao)
    {
        if (!File.Exists(caminho)) return valorPorOmissao();

        try
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(caminho)) ?? valorPorOmissao();
        }
        catch (JsonException ex)
        {
            // Não se continua com dados vazios: a próxima gravação apagaria o ficheiro original.
            throw new InvalidDataException(
                $"O ficheiro '{Path.GetFileName(caminho)}' está corrompido: {ex.Message}", ex);
        }
    }

    public static void Gravar<T>(string caminho, T dados)
    {
        // Grava num ficheiro temporário e só depois substitui o original,
        // para que uma falha a meio não deixe o ficheiro incompleto.
        string temporario = caminho + ".tmp";
        File.WriteAllText(temporario, JsonSerializer.Serialize(dados, Opcoes));
        File.Move(temporario, caminho, overwrite: true);
    }
}
