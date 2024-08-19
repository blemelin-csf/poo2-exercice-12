namespace Exercice12;

public class AppView
{
    public int DisplayMainMenu()
    {
        Terminal.Clear();
        Terminal.WriteLine("""
                           ********************************
                           *     Chiffrement de César     *
                           ********************************
                           Menu principal:
                           1. Chiffrer
                           2. Déchiffrer
                           3. Quitter

                           """);
        return Terminal.PromptInt("Choix");
    }

    public string DisplayEncodeMenu()
    {
        return Terminal.PromptText("Veuillez saisir du texte à chiffrer");
    }

    public void DisplayEncodedText(string text)
    {
        Terminal.WriteLine("Voici le texte chiffré : ");
        Terminal.WriteLine(text);
        Terminal.PromptContinue();
    }
    
    public string DisplayDecodeMenu()
    {
        return Terminal.PromptText("Veuillez saisir du texte à déchiffrer");
    }
    
    public void DisplayDecodedText(string text)
    {
        Terminal.WriteLine("Voici le texte déchiffré : ");
        Terminal.WriteLine(text);
        Terminal.PromptContinue();
    }

    public void DisplayInvalidChoiceError(int choice)
    {
        DisplayError($"Erreur : {choice} n'est pas un choix valide.");
    }
    
    public void DisplayEncodingError()
    {
        DisplayError("Erreur : veuillez taper uniquement des lettres de l'alphabet.");
    }

    public void DisplayDecodingError()
    {
        DisplayError("Erreur : veuillez taper uniquement des lettres de l'alphabet.");
    }

    private void DisplayError(string error)
    {
        Terminal.WriteLine(error, ConsoleColor.Red);
    }
}