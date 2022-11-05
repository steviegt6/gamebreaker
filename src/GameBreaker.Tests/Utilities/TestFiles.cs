// Copyright (c) Tomat. Licensed under the GPL License, version 2.
// See the LICENSE file in the repository root for full terms and conditions.

using System.IO;
using System.Reflection;

namespace GameBreaker.Tests.Utilities;

public static class TestFiles
{
    public static Stream GetEmbeddedBytes(string path) {
        path = "GameBreaker.Tests.TestFiles." + path;
        Assembly asm = typeof(TestFiles).Assembly;
        Stream? stream = asm.GetManifestResourceStream(path);
        if (stream == null) throw new FileNotFoundException($"Could not find embedded resource \"{path}\"!");
        return stream;
    }
}