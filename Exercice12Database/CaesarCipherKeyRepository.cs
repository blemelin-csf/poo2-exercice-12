namespace Exercice12;

/// <summary>
/// Base de données de clés de chiffrement.
/// </summary>
public class CaesarCipherKeyRepository
{
    private readonly string path;

    /// <summary>
    /// Crée une nouvelle base de données à partir du chemin spécifié.
    /// </summary>
    /// <param name="path">Chemin vers la clé.</param>
    public CaesarCipherKeyRepository(string path)
    {
        this.path = path;
    }

    /// <summary>
    /// Lit à clé de chiffrement à partir du disque.
    /// </summary>
    /// <returns>Clé de chiffrement ou <c>null</c> si elle n'existe pas.</returns>
    public CaesarCipherKey? Read()
    {
        try
        {
            // Ouverture du fichier.
            // Peut échouer si le fichier n'existe pas ou ne peut pas être lu (permissions).
            using var lines = File.ReadLines(path).GetEnumerator();

            // Lire la source d'aléatoire.
            if (!lines.MoveNext()) return null;
            if (!int.TryParse(lines.Current, out var seed)) return null;

            // Lire les caractères d'entrée.
            if (!lines.MoveNext()) return null;
            var input = lines.Current;

            // Créer la clé à partir des caractères d'entrée et de sortie.
            // Peut échouer si les caractères sont invalides.
            return CaesarCipherKey.Shuffle(seed, input);
        }
        catch
        {
            // En cas d'échec, ignore l'erreur et retourne null.
            return null;
        }
    }

    /// <summary>
    /// Écrit la clé de chiffrement sur le disque.
    /// </summary>
    /// <param name="key">Clé de chiffrement à écrire.</param>
    public void Write(CaesarCipherKey key)
    {
        try
        {
            File.WriteAllLines(path, [$"{key.Seed}", key.Input]);
        }
        catch
        {
            // Échouer silencieusement. Ce n'est pas l'idéal, mais ce sera suffisant pour cet exercice.
        }
    }
}