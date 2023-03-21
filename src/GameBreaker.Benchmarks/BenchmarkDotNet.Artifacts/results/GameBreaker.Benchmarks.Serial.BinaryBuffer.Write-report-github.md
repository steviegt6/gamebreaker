``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-OWGRBG : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                                            Method |       N |         Mean |       Error |      StdDev |       Median |      Gen0 |   Allocated |
|-------------------------------------------------- |-------- |-------------:|------------:|------------:|-------------:|----------:|------------:|
|                                 **DirectBufferWrite** |    **1000** |     **516.1 μs** |     **1.43 μs** |     **1.27 μs** |     **516.6 μs** |         **-** |   **173.08 KB** |
|                     BitConverterGetBytesArrayCopy |    1000 |     528.2 μs |     3.72 μs |     4.57 μs |     526.8 μs |         - |   173.08 KB |
|               BitConverterGetBytesBufferBlockCopy |    1000 |     515.0 μs |     1.49 μs |     1.24 μs |     515.2 μs |         - |   173.08 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopy |    1000 |     516.0 μs |     2.09 μs |     2.23 μs |     515.6 μs |         - |   173.08 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy |    1000 |     529.0 μs |    10.32 μs |    17.52 μs |     521.9 μs |         - |   173.08 KB |
|                              UnsafeAsPointerWrite |    1000 |     518.6 μs |     4.65 μs |     4.12 μs |     518.6 μs |         - |   173.08 KB |
|                                 **DirectBufferWrite** |  **100000** |  **15,289.9 μs** |   **304.39 μs** |   **687.06 μs** |  **14,977.0 μs** |         **-** |  **17188.7 KB** |
|                     BitConverterGetBytesArrayCopy |  100000 |  15,071.4 μs |   299.10 μs |   590.39 μs |  14,865.0 μs |         - |  17188.7 KB |
|               BitConverterGetBytesBufferBlockCopy |  100000 |  14,814.3 μs |   226.56 μs |   260.90 μs |  14,706.8 μs |         - |  17188.7 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopy |  100000 |  16,025.8 μs |   319.28 μs |   812.66 μs |  16,392.7 μs |         - |  17188.7 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy |  100000 |  16,170.5 μs |   319.39 μs |   437.19 μs |  16,323.6 μs |         - |  17188.7 KB |
|                              UnsafeAsPointerWrite |  100000 |  16,041.0 μs |   320.62 μs |   730.22 μs |  16,259.3 μs |         - |  17188.7 KB |
|                                 **DirectBufferWrite** | **1000000** | **149,825.0 μs** | **2,544.35 μs** | **2,124.65 μs** | **149,600.2 μs** | **7000.0000** | **171876.2 KB** |
|                     BitConverterGetBytesArrayCopy | 1000000 | 154,022.0 μs | 2,907.72 μs | 3,231.93 μs | 154,876.3 μs | 7000.0000 | 171876.2 KB |
|               BitConverterGetBytesBufferBlockCopy | 1000000 | 147,603.1 μs | 1,035.09 μs |   917.58 μs | 147,619.3 μs | 7000.0000 | 171876.2 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopy | 1000000 | 147,392.4 μs | 2,215.66 μs | 1,964.12 μs | 146,670.5 μs | 7000.0000 | 171876.2 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy | 1000000 | 148,671.9 μs | 2,883.22 μs | 2,696.96 μs | 147,408.7 μs | 7000.0000 | 171876.2 KB |
|                              UnsafeAsPointerWrite | 1000000 | 145,871.5 μs | 2,555.78 μs | 2,390.68 μs | 146,283.3 μs | 7000.0000 | 171876.2 KB |
