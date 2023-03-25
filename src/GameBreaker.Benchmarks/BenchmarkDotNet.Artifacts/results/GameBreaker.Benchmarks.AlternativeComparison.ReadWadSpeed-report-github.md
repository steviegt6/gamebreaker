``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|                  Method | N |       Mean |    Error |    StdDev |     Median |       Gen0 |       Gen1 |      Gen2 | Allocated |
|------------------------ |-- |-----------:|---------:|----------:|-----------:|-----------:|-----------:|----------:|----------:|
|     GameBreakerReadWads | 1 |   582.9 ms | 11.61 ms |  27.36 ms |   581.8 ms | 11000.0000 | 10000.0000 | 3000.0000 | 239.49 MB |
|      DogScepterReadWads | 1 |   482.9 ms |  9.62 ms |  19.21 ms |   485.1 ms | 10000.0000 |  9000.0000 | 3000.0000 | 238.12 MB |
| UndertaleModLibReadWads | 1 | 1,967.7 ms | 40.62 ms | 117.21 ms | 1,927.7 ms | 12000.0000 | 11000.0000 | 3000.0000 | 225.32 MB |
