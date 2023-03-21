``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-QQFSYG : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                    Method |      N |        Mean |     Error |   StdDev |      Median | Allocated |
|-------------------------- |------- |------------:|----------:|---------:|------------:|----------:|
|        **BufferRegionCopyTo** |   **1000** |    **176.8 μs** |  **44.08 μs** | **130.0 μs** |    **162.6 μs** |     **600 B** |
| BufferRegionUnsafePointer |   1000 |  1,718.1 μs | 282.19 μs | 832.0 μs |  2,123.1 μs |     600 B |
|    BufferRegionUnsafeSimd |   1000 |    287.5 μs |  48.64 μs | 142.6 μs |    203.7 μs |     600 B |
|        **BufferRegionCopyTo** | **100000** | **12,910.9 μs** | **250.28 μs** | **350.9 μs** | **12,750.3 μs** |     **600 B** |
| BufferRegionUnsafePointer | 100000 | 44,941.9 μs | 194.12 μs | 172.1 μs | 44,989.7 μs |     600 B |
|    BufferRegionUnsafeSimd | 100000 | 14,719.6 μs | 278.08 μs | 246.5 μs | 14,639.7 μs |     600 B |
