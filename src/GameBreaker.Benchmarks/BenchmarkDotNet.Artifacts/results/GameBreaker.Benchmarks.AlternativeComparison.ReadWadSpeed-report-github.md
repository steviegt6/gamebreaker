``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2


```
|              Method |     Mean |    Error |   StdDev |   Median |
|-------------------- |---------:|---------:|---------:|---------:|
| GameBreakerReadWads | 528.8 ms | 16.19 ms | 47.73 ms | 515.9 ms |
