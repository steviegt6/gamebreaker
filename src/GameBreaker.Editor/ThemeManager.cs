using Avalonia;
using Avalonia.Styling;
using Avalonia.Themes.Simple;

namespace GameBreaker.Editor;

public sealed class ThemeManager {
    private static readonly SimpleTheme simple = new();

    public void SetTheme(ThemeVariant theme) {
        if (Application.Current != null)
            Application.Current.RequestedThemeVariant = theme;
    }

    public void Initialize(Application app) {
        app.Styles.Insert(0, simple);
    }
}
