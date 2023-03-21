``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-ZHYCAJ : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                   Method |       N |         Mean |      Error |      StdDev |       Median | Allocated |
|------------------------- |-------- |-------------:|-----------:|------------:|-------------:|----------:|
|    **DirectBufferWriteByte** |    **1000** |     **7.004 μs** |  **0.1410 μs** |   **0.3788 μs** |     **6.900 μs** |     **600 B** |
| UnsafeAsPointerWriteByte |    1000 |     6.108 μs |  0.1143 μs |   0.1525 μs |     6.100 μs |     600 B |
|    **DirectBufferWriteByte** |  **100000** |   **435.562 μs** |  **7.7572 μs** |   **6.4776 μs** |   **434.000 μs** |     **600 B** |
| UnsafeAsPointerWriteByte |  100000 |   383.592 μs |  2.7701 μs |   2.1627 μs |   383.450 μs |     600 B |
|    **DirectBufferWriteByte** | **1000000** | **3,333.735 μs** | **65.0795 μs** | **134.4005 μs** | **3,410.350 μs** |     **600 B** |
| UnsafeAsPointerWriteByte | 1000000 | 3,163.484 μs | 78.2635 μs | 208.9011 μs | 3,243.300 μs |     600 B |
