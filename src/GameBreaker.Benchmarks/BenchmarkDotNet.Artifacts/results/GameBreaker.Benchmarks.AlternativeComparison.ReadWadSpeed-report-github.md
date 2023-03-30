``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|                  Method |  N |        Mean |     Error |    StdDev |        Gen0 |        Gen1 |       Gen2 |  Allocated |
|------------------------ |--- |------------:|----------:|----------:|------------:|------------:|-----------:|-----------:|
|     **GameBreakerReadWads** |  **1** |    **408.3 ms** |   **8.05 ms** |   **8.61 ms** |  **11000.0000** |  **10000.0000** |  **3000.0000** |  **239.49 MB** |
|      DogScepterReadWads |  1 |    346.3 ms |   6.90 ms |   8.22 ms |  10000.0000 |   9000.0000 |  3000.0000 |  238.12 MB |
| UndertaleModLibReadWads |  1 |  1,786.3 ms |  22.25 ms |  20.81 ms |  12000.0000 |  11000.0000 |  3000.0000 |  225.32 MB |
|     **GameBreakerReadWads** | **10** |  **3,990.1 ms** |  **29.33 ms** |  **26.00 ms** | **103000.0000** |  **95000.0000** | **23000.0000** | **2394.99 MB** |
|      DogScepterReadWads | 10 |  3,935.4 ms |  64.38 ms |  63.23 ms | 100000.0000 |  91000.0000 | 21000.0000 | 2381.25 MB |
| UndertaleModLibReadWads | 10 | 18,420.2 ms | 301.05 ms | 295.67 ms | 118000.0000 | 106000.0000 | 28000.0000 | 2253.12 MB |
