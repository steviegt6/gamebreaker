using System.Diagnostics;
using System.Windows;

namespace GameBreaker.Editor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
        }

        private void OpenGitHub(object sender, RoutedEventArgs e) {
            Process.Start(
                new ProcessStartInfo(
                    "https://github.com/steviegt6/gamebreaker"
                ) {
                    UseShellExecute = true,
                }
            );
        }

        private void OpenDiscord(object sender, RoutedEventArgs e) {
            Process.Start(
                new ProcessStartInfo("https://discord.gg/KvqKGQNbhr") {
                    UseShellExecute = true,
                }
            );
        }
    }
}
