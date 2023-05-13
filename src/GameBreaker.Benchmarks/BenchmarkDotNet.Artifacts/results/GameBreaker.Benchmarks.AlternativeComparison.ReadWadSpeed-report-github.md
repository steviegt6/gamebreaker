``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|                  Method |  N |        Mean |     Error |    StdDev |      Median |        Gen0 |        Gen1 |       Gen2 |  Allocated |
|------------------------ |--- |------------:|----------:|----------:|------------:|------------:|------------:|-----------:|-----------:|
|     **GameBreakerReadWads** |  **1** |    **563.2 ms** |  **28.01 ms** |  **82.60 ms** |    **510.0 ms** |  **12000.0000** |  **11000.0000** |  **4000.0000** |  **239.49 MB** |
|      DogScepterReadWads |  1 |    521.0 ms |  25.31 ms |  74.63 ms |    486.0 ms |  10000.0000 |   9000.0000 |  3000.0000 |  238.12 MB |
| UndertaleModLibReadWads |  1 |  2,393.0 ms | 137.98 ms | 406.83 ms |  2,330.7 ms |  12000.0000 |  11000.0000 |  3000.0000 |  225.32 MB |
|     **GameBreakerReadWads** | **10** |  **4,043.3 ms** |  **80.61 ms** | **153.38 ms** |  **4,041.6 ms** | **103000.0000** |  **95000.0000** | **23000.0000** | **2394.97 MB** |
|      DogScepterReadWads | 10 |  4,340.9 ms |  85.15 ms | 225.81 ms |  4,397.7 ms | 103000.0000 |  94000.0000 | 24000.0000 | 2381.32 MB |
| UndertaleModLibReadWads | 10 | 20,063.9 ms | 348.58 ms | 309.01 ms | 20,076.6 ms | 123000.0000 | 111000.0000 | 33000.0000 | 2253.17 MB |
