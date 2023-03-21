``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-RULYUL : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                    Method |      N |        Mean |     Error |   StdDev |      Median | Allocated |
|-------------------------- |------- |------------:|----------:|---------:|------------:|----------:|
|        **BufferRegionCopyTo** |   **1000** |    **182.8 μs** |  **46.12 μs** | **136.0 μs** |    **171.4 μs** |     **600 B** |
| BufferRegionUnsafePointer |   1000 |  1,726.8 μs | 289.85 μs | 854.6 μs |  2,162.6 μs |     600 B |
|        **BufferRegionCopyTo** | **100000** | **12,856.2 μs** | **183.29 μs** | **153.1 μs** | **12,843.8 μs** |     **600 B** |
| BufferRegionUnsafePointer | 100000 | 45,311.3 μs | 152.93 μs | 143.1 μs | 45,308.9 μs |     600 B |
