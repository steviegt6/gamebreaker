``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-GKATVZ : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                                            Method |       N |         Mean |       Error |      StdDev |      Gen0 |   Allocated |
|-------------------------------------------------- |-------- |-------------:|------------:|------------:|----------:|------------:|
|                                 **DirectBufferWrite** |    **1000** |     **492.4 μs** |     **3.47 μs** |     **2.71 μs** |         **-** |   **173.08 KB** |
|                     BitConverterGetBytesArrayCopy |    1000 |     498.4 μs |     4.57 μs |     3.82 μs |         - |   173.08 KB |
|               BitConverterGetBytesBufferBlockCopy |    1000 |     492.3 μs |     2.66 μs |     2.36 μs |         - |   173.08 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopy |    1000 |     493.0 μs |     1.56 μs |     1.38 μs |         - |   173.08 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy |    1000 |     489.4 μs |     2.31 μs |     1.93 μs |         - |   173.08 KB |
|                              UnsafeAsPointerWrite |    1000 |     487.6 μs |     1.93 μs |     2.37 μs |         - |   173.08 KB |
|                                 **DirectBufferWrite** |  **100000** |  **16,109.8 μs** |   **303.32 μs** |   **337.14 μs** |         **-** |  **17188.7 KB** |
|                     BitConverterGetBytesArrayCopy |  100000 |  16,317.1 μs |   201.89 μs |   188.85 μs |         - |  17188.7 KB |
|               BitConverterGetBytesBufferBlockCopy |  100000 |  16,206.5 μs |   202.91 μs |   179.87 μs |         - |  17188.7 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopy |  100000 |  16,346.7 μs |   314.95 μs |   309.32 μs |         - |  17188.7 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy |  100000 |  15,921.5 μs |    93.44 μs |    78.02 μs |         - |  17188.7 KB |
|                              UnsafeAsPointerWrite |  100000 |  16,035.4 μs |   230.70 μs |   215.79 μs |         - |  17188.7 KB |
|                                 **DirectBufferWrite** | **1000000** | **143,330.6 μs** | **1,239.75 μs** | **1,159.66 μs** | **7000.0000** | **171876.2 KB** |
|                     BitConverterGetBytesArrayCopy | 1000000 | 140,289.7 μs | 1,276.17 μs | 1,193.73 μs | 7000.0000 | 171876.2 KB |
|               BitConverterGetBytesBufferBlockCopy | 1000000 | 143,196.0 μs | 1,158.56 μs | 1,083.72 μs | 7000.0000 | 171876.2 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopy | 1000000 | 143,192.5 μs | 1,112.39 μs | 1,040.53 μs | 7000.0000 | 171876.2 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy | 1000000 | 143,839.6 μs | 2,623.85 μs | 2,454.35 μs | 7000.0000 | 171876.2 KB |
|                              UnsafeAsPointerWrite | 1000000 | 144,305.1 μs | 2,413.75 μs | 2,257.82 μs | 7000.0000 | 171876.2 KB |
