using System.Collections.Generic;
using System.IO;
using System.Xml;
using Avalonia;
using Avalonia.IconPacks.ViewModels;
using Avalonia.Media;

namespace GameBreaker.Editor;

public sealed class IconManager {
    public Dictionary<string, Drawing> Icons { get; } = new();

    public void Initialize(Application app) {
        foreach (var path in Directory.EnumerateFiles("Icons", "*.xaml")) {
            using var stream = File.Open(path, FileMode.Open);
            LoadIcons(stream);
        }
    }

    private void LoadIcons(Stream stream) {
        using var reader = XmlReader.Create(stream);
        reader.MoveToContent();

        while (reader.Read()) {
            if (reader.NodeType != XmlNodeType.Element)
                continue;

            if (reader.Name is not ("GeometryDrawing" or "DrawingGroup"))
                continue;

            var icon = new IconVM(reader.ReadOuterXml());
            Icons.Add(icon.Name, icon.Drawing);
        }
    }
}
