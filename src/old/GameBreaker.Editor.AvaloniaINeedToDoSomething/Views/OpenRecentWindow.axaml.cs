using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GameBreaker.Editor.Views; 

public partial class OpenRecentWindow : Window {
    public OpenRecentWindow() {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);
    }
}

