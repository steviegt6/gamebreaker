``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-RULYUL : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                   Method |       N |         Mean |       Error |      StdDev |       Median | Allocated |
|------------------------- |-------- |-------------:|------------:|------------:|-------------:|----------:|
|    **DirectBufferWriteByte** |    **1000** |     **6.965 μs** |   **0.1178 μs** |   **0.2095 μs** |     **6.900 μs** |     **600 B** |
| UnsafeAsPointerWriteByte |    1000 |     6.500 μs |   0.2531 μs |   0.7139 μs |     6.100 μs |     600 B |
|    **DirectBufferWriteByte** |  **100000** |   **435.042 μs** |   **3.4035 μs** |   **2.6572 μs** |   **435.050 μs** |     **600 B** |
| UnsafeAsPointerWriteByte |  100000 |   387.200 μs |   4.9529 μs |   3.8669 μs |   385.950 μs |     600 B |
|    **DirectBufferWriteByte** | **1000000** | **3,357.251 μs** |  **66.9474 μs** | **162.9592 μs** | **3,434.600 μs** |     **600 B** |
| UnsafeAsPointerWriteByte | 1000000 | 3,368.541 μs | 149.1028 μs | 439.6330 μs | 3,274.750 μs |     600 B |
