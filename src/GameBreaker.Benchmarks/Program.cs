using BenchmarkDotNet.Running;

namespace GameBreaker.Benchmarks; 

internal static class Program {
    public static void Main(string[] args) {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}