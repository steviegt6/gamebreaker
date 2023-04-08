``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|                  Method |  N |        Mean |     Error |    StdDev |        Gen0 |        Gen1 |       Gen2 |  Allocated |
|------------------------ |--- |------------:|----------:|----------:|------------:|------------:|-----------:|-----------:|
|     **GameBreakerReadWads** |  **1** |    **438.4 ms** |   **8.68 ms** |  **17.92 ms** |  **11000.0000** |  **10000.0000** |  **3000.0000** |  **239.49 MB** |
|      DogScepterReadWads |  1 |    411.4 ms |   8.22 ms |  10.09 ms |  10000.0000 |   9000.0000 |  3000.0000 |  238.12 MB |
| UndertaleModLibReadWads |  1 |  1,982.6 ms |  35.65 ms |  31.60 ms |  12000.0000 |  11000.0000 |  3000.0000 |  225.32 MB |
|     **GameBreakerReadWads** | **10** |  **4,605.1 ms** | **193.80 ms** | **571.42 ms** | **103000.0000** |  **95000.0000** | **23000.0000** | **2394.93 MB** |
|      DogScepterReadWads | 10 |  4,930.3 ms | 171.20 ms | 504.79 ms | 102000.0000 |  93000.0000 | 23000.0000 | 2381.24 MB |
| UndertaleModLibReadWads | 10 | 19,904.9 ms | 359.07 ms | 368.73 ms | 121000.0000 | 109000.0000 | 32000.0000 | 2253.13 MB |
