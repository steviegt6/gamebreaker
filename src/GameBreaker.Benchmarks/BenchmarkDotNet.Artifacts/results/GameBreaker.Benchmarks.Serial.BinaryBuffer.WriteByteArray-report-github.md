``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-HUUWBV : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                   Method |      N |        Mean |     Error |   StdDev |      Median | Allocated |
|------------------------- |------- |------------:|----------:|---------:|------------:|----------:|
|       **ByteArrayArrayCopy** |   **1000** |    **167.4 μs** |  **41.52 μs** | **122.4 μs** |    **176.6 μs** |     **600 B** |
| ByteArrayBufferBlockCopy |   1000 |    186.8 μs |  45.90 μs | 135.3 μs |    201.2 μs |     600 B |
|   ByteArrayUnsafePointer |   1000 |  1,460.2 μs | 313.91 μs | 925.6 μs |  2,037.8 μs |     600 B |
| ByteArrayUnsafeCopyBlock |   1000 |    176.4 μs |  49.56 μs | 145.4 μs |    117.7 μs |     600 B |
|       **ByteArrayArrayCopy** | **100000** | **13,965.2 μs** | **267.39 μs** | **307.9 μs** | **13,972.9 μs** |     **600 B** |
| ByteArrayBufferBlockCopy | 100000 | 13,779.6 μs | 263.12 μs | 332.8 μs | 13,810.8 μs |     600 B |
|   ByteArrayUnsafePointer | 100000 | 24,361.0 μs | 394.36 μs | 349.6 μs | 24,196.0 μs |     600 B |
| ByteArrayUnsafeCopyBlock | 100000 | 17,400.1 μs | 340.31 μs | 559.1 μs | 17,401.7 μs |     600 B |
