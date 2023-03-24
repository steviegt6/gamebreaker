``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-JIABXR : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|              Method |       N |         Mean |      Error |     StdDev |       Median | Allocated |
|-------------------- |-------- |-------------:|-----------:|-----------:|-------------:|----------:|
| **BitConverterReadAll** |    **1000** |     **90.97 μs** |   **1.597 μs** |   **1.334 μs** |     **90.60 μs** |     **600 B** |
|  PointerCastReadAll |    1000 |     51.07 μs |   1.005 μs |   1.535 μs |     50.50 μs |     600 B |
| **BitConverterReadAll** |  **100000** |  **3,059.36 μs** |  **60.429 μs** |  **62.056 μs** |  **3,050.60 μs** |     **600 B** |
|  PointerCastReadAll |  100000 |  2,467.19 μs |  48.626 μs |  79.894 μs |  2,427.80 μs |     600 B |
| **BitConverterReadAll** | **1000000** | **28,857.10 μs** | **304.194 μs** | **269.660 μs** | **28,764.60 μs** |     **600 B** |
|  PointerCastReadAll | 1000000 | 24,317.58 μs | 190.332 μs | 158.936 μs | 24,319.70 μs |     600 B |
