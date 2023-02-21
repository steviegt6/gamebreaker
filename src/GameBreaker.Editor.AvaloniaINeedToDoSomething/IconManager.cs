using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace GameBreaker.Editor;

public sealed class IconManager {
    private readonly Dictionary<string, Bitmap> iconCache = new();
    private IAssetLoader? loader;
    
    public Bitmap? this[string key] => GetIcon(key);

    public void Initialize(IAvaloniaDependencyResolver locator) {
        loader = locator.GetService<IAssetLoader>();
    }

    private Bitmap? GetIcon(string key) {
        if (loader is null)
            return null;

        if (iconCache.TryGetValue(key, out var icon))
            return icon;
        
        var path = $"avares://GameBreaker.Editor/Assets/Icons/{key}.png";
        return iconCache[key] = new Bitmap(loader.Open(new Uri(path)));
    }
}
