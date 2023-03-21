``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-VZGRLD : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                                            Method |       N |         Mean |       Error |      StdDev |       Median |
|-------------------------------------------------- |-------- |-------------:|------------:|------------:|-------------:|
|                                 **DirectBufferWrite** |    **1000** |     **590.5 μs** |    **22.58 μs** |    **66.58 μs** |     **593.1 μs** |
|                     BitConverterGetBytesArrayCopy |    1000 |     582.9 μs |    28.47 μs |    82.13 μs |     553.1 μs |
|               BitConverterGetBytesBufferBlockCopy |    1000 |     584.0 μs |    21.88 μs |    64.17 μs |     603.5 μs |
|       BitConverterTryWriteBytesByteArrayArrayCopy |    1000 |     595.9 μs |    27.58 μs |    79.56 μs |     598.5 μs |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy |    1000 |     584.5 μs |    25.59 μs |    74.25 μs |     574.2 μs |
|                              UnsafeAsPointerWrite |    1000 |     588.1 μs |    27.88 μs |    81.77 μs |     575.1 μs |
|                                 **DirectBufferWrite** |  **100000** |  **17,636.5 μs** |   **343.46 μs** |   **395.53 μs** |  **17,571.4 μs** |
|                     BitConverterGetBytesArrayCopy |  100000 |  17,637.8 μs |   344.02 μs |   337.88 μs |  17,637.8 μs |
|               BitConverterGetBytesBufferBlockCopy |  100000 |  17,648.5 μs |   349.84 μs |   429.64 μs |  17,458.5 μs |
|       BitConverterTryWriteBytesByteArrayArrayCopy |  100000 |  17,795.9 μs |   355.16 μs |   364.72 μs |  17,759.1 μs |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy |  100000 |  17,559.2 μs |   332.00 μs |   310.55 μs |  17,467.8 μs |
|                              UnsafeAsPointerWrite |  100000 |  17,993.0 μs |   357.48 μs |   397.34 μs |  18,038.1 μs |
|                                 **DirectBufferWrite** | **1000000** | **166,733.3 μs** | **3,243.93 μs** | **3,034.37 μs** | **164,873.1 μs** |
|                     BitConverterGetBytesArrayCopy | 1000000 | 167,020.1 μs | 3,282.32 μs | 3,070.28 μs | 168,336.7 μs |
|               BitConverterGetBytesBufferBlockCopy | 1000000 | 167,874.5 μs | 3,060.98 μs | 2,863.24 μs | 168,950.5 μs |
|       BitConverterTryWriteBytesByteArrayArrayCopy | 1000000 | 168,571.1 μs | 2,059.21 μs | 1,926.19 μs | 168,847.6 μs |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy | 1000000 | 168,572.6 μs | 2,053.39 μs | 1,920.74 μs | 168,898.6 μs |
|                              UnsafeAsPointerWrite | 1000000 | 168,767.4 μs | 2,823.46 μs | 2,641.07 μs | 169,817.8 μs |
