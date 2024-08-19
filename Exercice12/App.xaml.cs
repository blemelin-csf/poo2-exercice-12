using Wpf.Ui.Appearance;

namespace Exercice12;

#region Détails d'implémentation. Vous n'avez rien à faire dans ce fichier.

public partial class App;

public partial class MainWindow
{
    private void InitializeView()
    {
        SystemThemeWatcher.Watch(this);
    }
}

#endregion