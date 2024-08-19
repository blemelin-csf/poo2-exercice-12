namespace Exercice12;

public class AppController
{
    private const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ .";

    // Vue
    private readonly AppView view;
    
    // Modèle (données et traitements).
    private readonly CaesarCipherKeyRepository cipherKeyRepository;
    private readonly CaesarCipherKey cipherKey;
    private readonly CaesarCipher caesarCipher;
    
    public AppController()
    {
        // Initialisation de la vue.
        view = new AppView();

        // Initialisation du modèle.
        cipherKeyRepository = new CaesarCipherKeyRepository("key.txt");
        cipherKey = cipherKeyRepository.Read() ?? CaesarCipherKey.Shuffle(Letters);
        caesarCipher = new CaesarCipher();
    }

    public void Run()
    {
        var @continue = true;
        while (@continue)
        {
            var choice = view.DisplayMainMenu();

            switch (choice)
            {
                case 1:
                    EncodeText();
                    break;
                case 2:
                    DecodeText();
                    break;
                case 3:
                    @continue = false;
                    break;
                default:
                    view.DisplayInvalidChoiceError(choice);
                    break;
            }
        }

        // Avant de quitter, écrire la clé de chiffrement sur le disque.
        cipherKeyRepository.Write(cipherKey);
    }

    private void EncodeText()
    {
        // Demande à l'utilisateur du texte à chiffrer.
        var text = view.DisplayEncodeMenu();
        
        // Chiffrement du texte.
        var encodedText = caesarCipher.Encode(text, cipherKey);

        // Si le chiffrement a réussi, afficher le texte.
        if (encodedText != null)
        {
            view.DisplayEncodedText(encodedText);
        }
        // Si l'encodage échoue (parce qu'une lettre n'est pas supportée), afficher une erreur.
        else
        {
            view.DisplayEncodingError();
        }
    }

    private void DecodeText()
    {
        // Demande à l'utilisateur du texte à déchiffrer.
        var text = view.DisplayDecodeMenu();
        var decodedText = caesarCipher.Decode(text, cipherKey);

        // Si le décodage réussi, afficher le texte.
        if (decodedText != null)
        {
            view.DisplayDecodedText(decodedText);
        }
        // Si le décodage échoue (parce qu'une lettre n'est pas supportée), afficher une erreur.
        else
        {
            view.DisplayDecodingError();
        }
    }
}