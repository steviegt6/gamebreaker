using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Themes.Simple;
using GameBreaker.Editor.ViewModels;
using GameBreaker.Editor.Views;

namespace GameBreaker.Editor;

public partial class App : Application {
    public new static App? Current => (App?)Application.Current;

    public ThemeManager ThemeManager { get; } = new();

    public IconManager IconManager { get; } = new();

    public override void Initialize() {
        ThemeManager.Initialize(this);
        IconManager.Initialize(AvaloniaLocator.Current);
        AvaloniaXamlLoader.Load(this);
        ThemeManager.SetTheme(ThemeVariant.Default);
    }

    public override void OnFrameworkInitializationCompleted() {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
            desktop) {
            desktop.MainWindow = new MainWindow {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
