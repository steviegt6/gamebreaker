using System;
using Avalonia;
using Avalonia.Styling;
using Avalonia.Themes.Simple;

namespace GameBreaker.Editor;

public sealed class ThemeManager {
    private static readonly SimpleTheme simple = new();
    
    public event EventHandler<ThemeVariant>? ThemeChanged;

    public void SetTheme(ThemeVariant theme) {
        if (Application.Current != null)
            Application.Current.RequestedThemeVariant = theme;
        
        ThemeChanged?.Invoke(this, theme);
    }

    public void Initialize(Application app) {
        app.Styles.Insert(0, simple);
    }
}
