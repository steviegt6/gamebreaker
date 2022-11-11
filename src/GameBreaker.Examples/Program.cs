using CliFx;

namespace GameBreaker;

public static class Program
{
    public static async Task<int> Main(string[] args) {
        await PrintLicense();
        await Console.Out.WriteLineAsync("\n\n");

        try {
            return await new CliApplicationBuilder()
                        .AddCommandsFromThisAssembly()
                        .Build()
                        .RunAsync(args);
        }
        catch (Exception e) {
            await Console.Error.WriteLineAsync("Exception thrown during execution:\n" + e);
            return 1;
        }
    }

    private static async Task PrintLicense() {
        await Console.Out.WriteLineAsync(@"
GameBreaker.Core.Examples - Example usages for GameBreaker.Core and GameBreaker.Core.
Copyright (C) 2022  Tomat (Steviegt6)

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
".Trim());
    }
}