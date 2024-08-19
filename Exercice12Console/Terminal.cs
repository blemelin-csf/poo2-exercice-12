namespace Exercice12;

/// <summary>
/// Représente un terminal.
///
/// Fourni quelques utilitaires de plus que la classe <c>Console</c>.
/// </summary>
public static class Terminal
{
    /// <summary>
    /// Vide le terminal.
    /// </summary>
    public static void Clear()
    {
        Console.Clear();
    }

    /// <summary>
    /// Écrit du texte dans le terminal (sans sauter de ligne).
    /// </summary>
    /// <param name="text">Texte à écrire.</param>
    /// <param name="color">Couleur du texte à utiliser.</param>
    public static void Write(string text, ConsoleColor? color = null)
    {
        if (color.HasValue) Console.ForegroundColor = color.Value;
        Console.Write(text);
        Console.ResetColor();
    }

    /// <summary>
    /// Écrit du texte dans le terminal (avec un saut de ligne à la fin).
    /// </summary>
    /// <param name="text">Texte à écrire.</param>
    /// <param name="color">Couleur du texte à utiliser.</param>
    public static void WriteLine(string text, ConsoleColor? color = null)
    {
        Write(text, color);
        Console.WriteLine();
    }

    /// <summary>
    /// Lit une touche dans le terminal.
    /// </summary>
    /// <returns>Touche appuyée.</returns>
    public static char ReadKey()
    {
        return Console.ReadKey().KeyChar;
    }

    /// <summary>
    /// Lit une ligne de texte dans le terminal.
    /// </summary>
    /// <returns>Ligne de texte lue.</returns>
    public static string ReadLine()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        var input = Console.ReadLine()!;
        Console.ResetColor();
        return input;
    }

    /// <summary>
    /// Demande à l'utilisateur d'appuyer sur une touche pour continuer.
    /// </summary>
    public static void PromptContinue()
    {
        WriteLine("Appuyez sur une touche pour continuer...");
        ReadKey();
    }

    /// <summary>
    /// Demande à l'utilisateur d'écrire une ligne de texte non vide.
    /// </summary>
    /// <param name="prompt">Texte de la demande.</param>
    /// <returns>Texte tapé par l'utilisateur (non vide).</returns>
    public static string PromptText(string prompt)
    {
        while (true)
        {
            Write($"{prompt} : ");
            var input = ReadLine();

            if (input.Length > 0)
                return input;
            else
                WriteLine("Erreur : entrée vide.", ConsoleColor.Red);
        }
    }

    /// <summary>
    /// Demande à l'utilisateur d'écrire un entier.
    /// </summary>
    /// <param name="prompt">Texte de la demande.</param>
    /// <returns>Entier tapé par l'utilisateur.</returns>
    public static int PromptInt(string prompt)
    {
        while (true)
        {
            var input = PromptText(prompt);

            if (int.TryParse(input, out var number))
                return number;
            else
                WriteLine("Erreur : pas un nombre.", ConsoleColor.Red);
        }
    }
}