using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using GameBreaker.Editor.Util.Extensions;

namespace GameBreaker.Editor.Views;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
    }

    private void InitializeComponent() {
        AvaloniaXamlLoader.Load(this);

        AddThemes();
    }

    private void AddThemes() {
        var themesControl = this.FindControl<MenuItem>("ThemesControl");
        if (themesControl is null)
            return;

        var themesMenu = new AvaloniaList<MenuItem>();
        themesControl.Items = themesMenu;

        var themes = new[] {
            ThemeVariant.Default,
            ThemeVariant.Light,
            ThemeVariant.Dark,
        };

        foreach (var theme in themes) {
            var themeItem = new MenuItem {
                Header = theme.ToString(),
                Command = new SimpleCommand<ThemeVariant>(ClickTheme),
                CommandParameter = theme,
            };

            themesMenu.Add(themeItem);
        }

        if (App.Current is null)
            return;

        void onThemeChanged(object? _, ThemeVariant theme) {
            var icon = App.Current?.IconManager["VSCodeDark.check"];

            foreach (var themeItem in themesMenu!) {
                if (themeItem.CommandParameter is ThemeVariant themeVariant)
                    themeItem.Icon = themeVariant == theme
                        ? icon?.AsImage()
                        : null;
            }
        }

        var app = App.Current;
        var currTheme = app.RequestedThemeVariant ?? app.ActualThemeVariant;
        app.ThemeManager.ThemeChanged += onThemeChanged;
        onThemeChanged(null, currTheme);
    }

    private static void ClickTheme(ThemeVariant theme) {
        App.Current?.ThemeManager.SetTheme(theme);
    }
}
