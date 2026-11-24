using System.Security.Cryptography;
using System.Text;

namespace RegistoUtentes.Security;

/// <summary>
/// Cálculo e verificação do resumo (hash) do PIN. Mantém SHA-256 para continuar
/// compatível com os utilizadores.json já existentes.
/// </summary>
public static class PinHasher
{
    public static string Hash(string pin)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(pin));
        return Convert.ToHexString(bytes);
    }

    public static bool Verificar(string pin, string hashGuardado)
    {
        byte[] a = Encoding.UTF8.GetBytes(Hash(pin));
        byte[] b = Encoding.UTF8.GetBytes(hashGuardado);
        return CryptographicOperations.FixedTimeEquals(a, b);
    }
}
