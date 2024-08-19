using System.Text;

namespace Exercice12;

/// <summary>
/// Chiffrement de César.
/// </summary>
public class CaesarCipher
{
    /// <summary>
    /// Chiffre une chaine de caractère en utilisant la clé fournie.
    /// </summary>
    /// <param name="text">Texte à chiffrer.</param>
    /// <param name="key">Clé à utiliser.</param>
    /// <returns>Texte chiffré avec la clé.</returns>
    public string? Encode(string text, CaesarCipherKey key)
    {
        var builder = new StringBuilder(text.Length);
        foreach (var character in text)
        {
            var encoded = key.Encode(character);
            if (encoded == null) return null;
            builder.Append(encoded.Value);
        }
        return builder.ToString();
    }

    /// <summary>
    /// Déchiffre une chaine de caractère en utilisant la clé fournie.
    /// </summary>
    /// <param name="text">Texte à déchiffrer.</param>
    /// <param name="key">Clé à utiliser.</param>
    /// <returns>Texte déchiffré avec la clé.</returns>
    public string? Decode(string text, CaesarCipherKey key)
    {
        return Encode(text, key.Reversed());
    }
}