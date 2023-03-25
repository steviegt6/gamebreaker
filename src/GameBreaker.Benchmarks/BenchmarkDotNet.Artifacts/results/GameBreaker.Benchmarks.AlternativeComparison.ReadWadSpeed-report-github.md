``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|                  Method | N |       Mean |    Error |   StdDev |       Gen0 |       Gen1 |      Gen2 | Allocated |
|------------------------ |-- |-----------:|---------:|---------:|-----------:|-----------:|----------:|----------:|
|     GameBreakerReadWads | 1 |   408.7 ms |  6.66 ms |  6.23 ms | 11000.0000 | 10000.0000 | 3000.0000 | 239.49 MB |
| UndertaleModLibReadWads | 1 | 1,872.8 ms | 35.95 ms | 42.80 ms | 12000.0000 | 11000.0000 | 3000.0000 | 225.31 MB |
