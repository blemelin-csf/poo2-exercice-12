# Exercice 12 - MVVM

Vous aurez à remplacer une architecture *MVC* par une architecture en *MVVM*. L'application sert à effectuer du 
chiffrement par substitution (communément appellé *Chiffrement de César*).

> [!NOTE]  
> Cet exercice se fera en classe avec le professeur. Vous aurez à réaliser le même genre de travail dans le prochain
> exercice, mais par vous-même. Cela dit, si vous désirez refaire cet exercice, vous pouvez suivre les instructions
> de ce document.

## Travail à faire

Voici les tâches à faire pour cet exercice.

### Créer le *ViewModel*

Ouvrez le projet **Exercice12**. Pour l'instant, l'interface graphique utilise une architecture *MVC* pour faire la 
liaison entre la vue (le fichier `MainWindow.xaml`) et le modèle (le code dans le projet **Exercice12Model**). La 
première étape consiste à créer le *ViewModel*, c'est-à-dire une classe pour convertir les données du modèle en données
pour la vue (et vice-versa).

Créez une classe `MainWindowViewModel`. De manière générale, il y a un *ViewModel* par fichier `xaml` : c'est donc
une bonne idée que leurs noms sont similaires. Héritez de la classe `ViewModel`.

```csharp
public class MainWindowViewModel : ViewModel
{
    
}
```

> [!CAUTION]
> La classe `ViewModel` n'est pas fournie par Microsoft. Elle vous est donnée à même le projet de départ.
> 
> En temps normal, tout bon *framework* *MVVM* contient une classe `ViewModel` pour servir de base. La décision de 
> Microsoft de ne pas en fournir peut-être considérée comme une drôle de décision, mais c'est facilement rectifiable.

#### Exposer les données

Votre *ViewModel* doit exposer les données nécessaires à la vue. Il faut donc créer des propriétés. Actuellement,
la vue a besoin de savoir :

* Le numéro de la clé de chiffrement (voir le contrôle `seedNumberBox`).
* Le texte à chiffrer (voir le contrôle `inputTextBox`).
* Le texte déchiffré (voir le contrôle le `outputTextBox`).
* S'il y a erreur ou nom (voir le contrôle `errorTextBlock`).

Le *ViewModel* doit contenir une propriété chacune de ces informations. Par exemple, voici la propriété à ajouter dans 
la classe `MainWindowViewModel` pour exposer le numéro de la clé :

```csharp
private int seed;
    
public int Seed
{
    get => seed;
    set => UpdateProperty(value, ref seed);
}
```

Remarquez le *setter* de cette propriété. Elle utilise la méthode `UpdateProperty` de la classe parent. Cette méthode
met à jour la valeur de l'**attribut** `seed` en plus de notifier le *framework* *WPF* qu'il y a eu un changement dans
le *ViewModel*. 

Faites la même chose pour les autres informations, à savoir `Input`, `Output` et `IsInputInvalid`. Notez que la 
propriété `InInputInvalid` est de type `bool` et nous servira à afficher ou cacher le message d'erreur.

#### Exposer les commandes

En *MVVM*, le *ViewModel* expose des commandes que la vue peut déclencher. C'est très similaire à des événements, mais
cela n'utilise pas la même syntaxe. Actuellement, la vue permet de faire ces actions :

* Changer de clé via son numéro (`CreateKeyCommand`).
* Changer de clé aléatoirement (`ShuffleKeyCommand`).
* Chiffrer le texte saisi (`EncodeTextCommand`).
* Déchiffrer le texte saisi (`DecodeTextCommand`).
* Sauvegarder la clé de chiffrement (`SaveKeyCommand`).

À chaque commande correspond une méthode sur le *ViewModel*. Par exemple, pour la commande `EncodeTextCommand`, ajoutez
une méthode nommée `EncodeText` dans la classe `MainWindowViewModel` :

```csharp
public void EncodeText()
{
    // Code de la méthode ...
}
```

En plus de la méthode, ajoutez aussi une propriété indiquant que cette méthode est une commande. Cela se fait tout 
simplement ainsi, en indiquant le nom de la commande et la fonction à appeler lorsque la commande est déclenchée :

```csharp
public ICommand EncodeTextCommand => NewCommand(EncodeText);
```

Faites la même chose pour les autres commandes (à savoir `CreateKeyCommand`, `ShuffleKeyCommand`, `DecodeTextCommand`
et `SaveKeyCommand`). Pour l'instant, laissez les méthodes vides.

#### Ajouter le modèle

Tout comme un contrôleur, un *ViewModel* contient des objets de la couche *Model*. Dans le constructeur de la classe 
`MainWindowViewModel`, instanciez les objets de la couche modèle et conservez-les en attribut. Consultez le contrôleur
actuel en guise d'exemple.

```csharp
public MainWindowViewModel()
{
    cipherKeyRepository = new CaesarCipherKeyRepository("key.txt");
    cipherKey = cipherKeyRepository.Read() ?? CaesarCipherKey.Shuffle(Letters);
    caesarCipher = new CaesarCipher();
}
```

Toujours dans ce constructeur, prenez le temps d'initialiser une première fois les propriétés que vous avez créées 
précédemment. Utilisez soit des valeurs par défaut ou des données en provenance du modèle (comme la `Seed`).

```csharp
public MainWindowViewModel()
{
    // ...
    
    seed = cipherKey.Seed;
    input = "";
    output = "";
    isInputInvalid = false;
}
```

Implémentez les diverses commandes en utilisant la couche modèle. À noter que vous n'aurez pas directement accès à la 
vue. À la place, changez les valeurs des propriétés. Ce faisant, cela notifie le *framework* *WPF*, ce qui engendrera 
la mise à jour de la vue. Par exemple, pour le chiffrement :

```csharp
public void EncodeText()
{
    var textToEncode = Input;
    string encodedText = caesarCipher.Encode(textToEncode, cipherKey);

    if (encodedText != null)
    {
        Output = encodedText;
        IsInputInvalid = false;
    }
    else
    {
        Output = "";
        IsInputInvalid = true;
    }
}
```

### Relier la *Vue* au *ViewModel*

Il faut brancher la *Vue* sur le *ViewModel* que vous venez de créer. Pour ce faire, nous allons utiliser le *framework*
de *data binding* fourni avec *WPF*. Ouvrez donc le fichier `MainWindow.xaml`. Il y a deux choses à faire :

1. Instancier le *ViewModel*. Cela se fait en ajoutant une *ressource*. L'attribut `x:Key` sert à lui donner un nom.
2. Indiquer à la `<FluentWindow>` (la racine de la vue) quel *ViewModel* utiliser. L'attribut `DataContext` indique le nom du *ViewModel* à utiliser.

```xml
<ui:FluentWindow x:Class="Exercice12.MainWindow"
                 DataContext="{DynamicResource ViewModel}">
    
    <ui:FluentWindow.Resources>
        <local:MainWindowViewModel x:Key="ViewModel" />
    </ui:FluentWindow.Resources>
    
    <!-- Reste du fichier XAML. -->
</ui:FluentWindow>
```

Maintenant que la vue connait son *ViewModel*, nous pouvons y relier les contrôles. Commençons par le champ de texte
en entrée (le champ `inputTextBox`).

```xml
    <ui:TextBox Grid.Row="3" Grid.Column="0"
                Margin="16,8"
                Text="{Binding Input}" />
```

Remarquez les `{}` à l'intérieur de l'attribut `Text`. Cela indique que c'est une liaison (ou *binding* en anglais).
Dans le cas précédent, cela relie le champ de texte à la propriété `Input` du *ViewModel*. Si la valeur de la propriété
change, alors la vue sera mise à jour automatiquement. Dans notre cas, nous voulons aussi qu'un changement dans la vue
impacte les données du *ViewModel*. Il faut donc activer une liaison bidirectionnelle, comme ceci :

```xml
    <ui:TextBox Grid.Row="3" Grid.Column="0"
                Margin="16,8"
                Text="{Binding Input, Mode=TwoWay}" />
```

Faites la même chose pour le champ `outputTextBox` (qui n'a pas besoin d'être bidirectionnel) :

```xml
<ui:TextBox Grid.Row="4" Grid.Column="0"
            Margin="16,8"
            IsReadOnly="True"
            Text="{Binding Output}"/>
```

Nous allons aussi ajouter une liaison entre le champ `seedNumberBox` et la propriété `Seed` :

```xml
<ui:NumberBox Grid.Row="0" Grid.Column="0"
              Margin="0,0,4,0"
              Minimum="0"
              MaxDecimalPlaces="0"
              Value="{Binding Seed, UpdateSourceTrigger=PropertyChanged}"/>
```

> [!Warning]
> Il y a un bogue dans *WPF UI* qui nous oblige à ajouter `UpdateSourceTrigger=PropertyChanged` dans le cas des champs
> `<NumberBox>`. Voir [ce ticket](https://github.com/lepoco/wpfui/issues/945) pour les détails.

Nous pouvons afficher/cacher le message d'erreur `errorTextBlock` en fonction de la valeur de la propriété
`IsInputValid`. Le convertisseur `BooleanToVisibilityConverter` permet de convertir un booléen (`true`, `false`) en une
valeur de visibilité (`Visible`, `Collapsed`).

```xml
<ui:TextBlock Grid.Row="5" Grid.Column="0"
              Margin="16,8"
              Visibility="{Binding IsInputInvalid, Converter={StaticResource BooleanToVisibilityConverter}}"
              Text="Erreur : veuillez taper uniquement des lettres de l'alphabet."
              Foreground="Coral" />
```

Il ne reste qu'à indiquer les commandes à déclencher par les boutons. Par exemple, voici le bouton pour chiffrer le 
texte (regardez l'attribut `Command`). Faites la même chose pour les autres boutons.

```xml
<ui:Button
    Margin="0,0,4,0"
    Content="Chiffrer"
    Icon="{ui:SymbolIcon LockClosed24}"
    Command="{Binding EncodeTextCommand}"/>
```

Finalement, indiquez la commande à appeler lorsque la fenêtre se ferme. Dans ce cas, il est possible de spécifier la 
commande à appeler lorsque l'événement `Closed` se produit sur la fenêtre (voir la racine).

```xml
<ui:FluentWindow x:Class="Exercice12.MainWindow"
                 Closed="{local:EventBinding SaveKeyCommand}">
```

### Vider le contrôleur

Il ne nous reste qu'à vider le contrôleur. Ouvrez le fichier `MainWindow.xaml.cs`, et remplacez son contenu par 
celui-ci :

```csharp
using Wpf.Ui.Controls;

namespace Exercice12
{
    public partial class MainWindow : FluentWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeView();
        }
    }
}
```

> [!NOTE]
> L'architecture *MVVM* n'exclu pas nécessairement la présence d'un contrôleur. Dans ce cas, sa seule responsabilité
> est d'instancier la vue, mais il pourrait aussi répondre à des événements que le *ViewModel* n'est pas en mesure de 
> gérer.

### Commandes avec paramètres

Une commande peut recevoir un paramètre si désiré. En guise d'exemple, nous allons envoyer à la commande `EncodeText`
la chaine de caractères à chiffrer. Pour cela, commencez par ajouter un paramèter à la méthode `EncodeText` de votre 
*ViewModel*, comme ceci :

```csharp
public void EncodeText(string textToEncode)
{
    // Code de la méthode ...
}
```

Un peu plus haut dans votre fichier, localisez la propriété qui déclare la commande. Utilisez `NewCommandWithParam`
au lieu de `NewCommand`, tout en indiquant le type de paramètre attendu. Voici comment :

```csharp
public ICommand EncodeTextCommand => NewCommandWithParam<string>(EncodeText);
```

Finalement, il faut indiquer dans la vue la valeur du paramètre. Localisez le bouton exécutant la commande
`EncodeTextCommand` et ajoutez l'attribut `CommandParameter`.

```xml
<ui:Button
    Command="{Binding EncodeTextCommand}"
    CommandParameter="Hello World!"/>
```

Comme nous désirons encoder le texte saisi par l'utilisateur, remplacez le `Hello World` par un binding. Dans ce cas,
nous allons utiliser la valeur de la propriété `Input` du *ViewModel*.

```xml
<ui:Button
    Command="{Binding EncodeTextCommand}"
    CommandParameter="{Binding Input}"/>
```

Si souhaité, vous pouvez aussi faire la même chose pour les commandes `DecodeText` et `CreateKey`.

### Tester l'application

En temps normal, vous devriez avoir une application complète. Essayez-la pour vous assurer que tout fonctionne. 
N'hésitez pas à comparer le code avec la version *MVC* de l'application. Vous pouvez consulter l'historique *Git*.

## Modalités de remise

Remettez votre projet sur GitHub Classroom. N’oubliez pas de faire un *commit*, un *pull* et un *push* et de vérifier 
sur le site de *GitHub* que vos fichiers ont bien été téléversés.

[WPF]: https://learn.microsoft.com/fr-fr/dotnet/desktop/wpf/overview/