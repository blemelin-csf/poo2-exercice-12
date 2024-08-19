using System.Windows;
using Wpf.Ui.Controls;

namespace Exercice12
{
    public partial class MainWindow : FluentWindow
    {
        private const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ .";

        private readonly CaesarCipherKeyRepository cipherKeyRepository;
        private readonly CaesarCipher caesarCipher;
        private CaesarCipherKey cipherKey;

        public MainWindow()
        {
            // Initialize view.
            InitializeComponent();
            InitializeView();

            // Initialize model.
            cipherKeyRepository = new CaesarCipherKeyRepository("key.txt");
            cipherKey = cipherKeyRepository.Read() ?? CaesarCipherKey.Shuffle(Letters);
            caesarCipher = new CaesarCipher();
            
            // Update view.
            UpdateView();
        }

        private void UpdateView()
        {
            seedNumberBox.Value = cipherKey.Seed;
        }

        private void OnCreateKeyButtonClicked(object sender, RoutedEventArgs e)
        {
            var seed = (int)(seedNumberBox.Value ?? 0);
            
            cipherKey = CaesarCipherKey.Shuffle(seed, Letters);
            UpdateView();
        }

        private void OnShuffleKeyButtonClicked(object sender, RoutedEventArgs e)
        {
            cipherKey = CaesarCipherKey.Shuffle(Letters);
            UpdateView();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            cipherKeyRepository.Write(cipherKey);
        }

        private void OnEncodeButtonClicked(object sender, RoutedEventArgs e)
        {
            var input = inputTextBox.Text;
            
            var encoded = caesarCipher.Encode(input, cipherKey);
            if (encoded != null)
            {
                outputTextBox.Text = encoded;
                errorTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                outputTextBox.Text = "";
                errorTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void OnDecodeButtonClicked(object sender, RoutedEventArgs e)
        {
            var input = inputTextBox.Text;
            
            var decoded = caesarCipher.Decode(input, cipherKey);
            if (decoded != null)
            {
                outputTextBox.Text = decoded;
                errorTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                outputTextBox.Text = "";
                errorTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}