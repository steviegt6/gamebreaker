``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-FBKXCO : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                   Method |       N |         Mean |       Error |      StdDev |       Median | Allocated |
|------------------------- |-------- |-------------:|------------:|------------:|-------------:|----------:|
|    **DirectBufferWriteByte** |    **1000** |     **6.742 μs** |   **0.1016 μs** |   **0.0793 μs** |     **6.700 μs** |     **600 B** |
| UnsafeAsPointerWriteByte |    1000 |     6.075 μs |   0.1227 μs |   0.2304 μs |     6.000 μs |     600 B |
|    **DirectBufferWriteByte** |  **100000** |   **433.723 μs** |   **2.0318 μs** |   **1.6966 μs** |   **433.900 μs** |     **600 B** |
| UnsafeAsPointerWriteByte |  100000 |   385.620 μs |   1.2579 μs |   1.1767 μs |   385.600 μs |     600 B |
|    **DirectBufferWriteByte** | **1000000** | **3,322.415 μs** |  **60.7631 μs** | **129.4910 μs** | **3,403.400 μs** |     **600 B** |
| UnsafeAsPointerWriteByte | 1000000 | 3,210.059 μs | 107.1792 μs | 293.4012 μs | 3,236.000 μs |     600 B |
