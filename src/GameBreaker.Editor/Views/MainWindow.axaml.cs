using System;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using ReactiveUI;

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

            var icon = App.Current?.IconManager["VSCodeDark.check"];

            if (icon is not null) {
                var image = new Image {
                    Source = icon,
                };
                themeItem.Icon = image;
            }

            themesMenu.Add(themeItem);
        }
    }

    private void ClickTheme(ThemeVariant theme) {
        App.Current?.ThemeManager.SetTheme(theme);
    }
}
