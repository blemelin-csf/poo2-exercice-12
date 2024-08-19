namespace Exercice12;

/// <summary>
/// Représente une clé de chiffrement pour le chiffrement de César.
/// </summary>
public sealed class CaesarCipherKey
{
    /// <summary>
    /// Source d'aléatoire ayant généré cette clé.
    /// </summary>
    public int Seed { get; }

    /// <summary>
    /// Lettres en entrées.
    /// </summary>
    public string Input { get; }

    /// <summary>
    /// Lettres en sortie.
    /// </summary>
    public string Output { get; }

    /// <summary>
    /// Crée une clé de chiffrement à partir d'un lot de lettre. Les lettres en sortie sont aléatoires.
    /// </summary>
    /// <param name="input">Lettres en entrée.</param>
    public static CaesarCipherKey Shuffle(string input)
    {
        return Shuffle(Random.Shared.Next(), input);
    }

    /// <summary>
    /// Crée une clé de chiffrement à partir d'un lot de lettre et d'une source d'aléatoire.
    /// </summary>
    /// <param name="seed">Source d'aléatoire.</param>
    /// <param name="input">Lettres en entrée.</param>
    public static CaesarCipherKey Shuffle(int seed, string input)
    {
        // Mélange l'entrée pour former la sortie (un simple Fisher–Yates).
        var random = new Random(seed);
        var shuffledInput = input.ToCharArray();
        for (var i = 0; i < shuffledInput.Length - 1; i++)
        {
            var j = random.Next(i, shuffledInput.Length);
            (shuffledInput[i], shuffledInput[j]) = (shuffledInput[j], shuffledInput[i]);
        }

        // Retourne la clé générée aléatoirement.
        return new CaesarCipherKey(seed, input, new string(shuffledInput));
    }

    private CaesarCipherKey(int seed, string input, string output)
    {
        // Ce constructeur est privé, et c'est voulu. Utilisez la méthode "Shuffle" pour créer une clé.
        Seed = seed;
        Input = input;
        Output = output;
    }

    /// <summary>
    /// Chiffre une lettre en une autre en utilisant la clé.
    /// </summary>
    /// <param name="character">Lettre à chiffrer.</param>
    /// <returns>Lettre chiffrée.</returns>
    public char? Encode(char character)
    {
        var index = Input.IndexOf(character);
        if (index < 0) return null;
        return Output[index];
    }

    /// <summary>
    /// Produit la clé inverse, c'est-à-dire une nouvelle clé qui inversera l'effet de cette clé.
    /// </summary>
    public CaesarCipherKey Reversed()
    {
        return new CaesarCipherKey(Seed, Output, Input);
    }

    private bool Equals(CaesarCipherKey other)
    {
        return Seed == other.Seed;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is CaesarCipherKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Seed;
    }

    public static bool operator ==(CaesarCipherKey? left, CaesarCipherKey? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(CaesarCipherKey? left, CaesarCipherKey? right)
    {
        return !Equals(left, right);
    }
}