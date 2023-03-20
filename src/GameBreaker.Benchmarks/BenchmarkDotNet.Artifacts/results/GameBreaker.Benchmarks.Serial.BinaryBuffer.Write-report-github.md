``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-DUZNZJ : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                                            Method |     Length |          Mean |       Error |        StdDev |        Median |
|-------------------------------------------------- |----------- |--------------:|------------:|--------------:|--------------:|
|                                 **DirectBufferWrite** |        **100** |     **0.0000 ns** |   **0.0000 ns** |     **0.0000 ns** |     **0.0000 ns** |
|                     BitConverterGetBytesArrayCopy |        100 | 1,102.0833 ns | 106.2014 ns |   306.4153 ns | 1,000.0000 ns |
|               BitConverterGetBytesBufferBlockCopy |        100 | 1,171.4286 ns |  92.8865 ns |   260.4636 ns | 1,100.0000 ns |
|       BitConverterTryWriteBytesByteArrayArrayCopy |        100 | 1,488.0435 ns | 149.3925 ns |   421.3642 ns | 1,400.0000 ns |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy |        100 | 1,126.5957 ns |  93.5389 ns |   266.8718 ns | 1,100.0000 ns |
|                              UnsafeAsPointerWrite |        100 |    37.8947 ns |  23.3961 ns |    67.1279 ns |     0.0000 ns |
|                                 **DirectBufferWrite** |     **100000** |    **73.1959 ns** |  **25.6185 ns** |    **74.3240 ns** |   **100.0000 ns** |
|                     BitConverterGetBytesArrayCopy |     100000 | 1,246.8750 ns | 108.2035 ns |   312.1920 ns | 1,200.0000 ns |
|               BitConverterGetBytesBufferBlockCopy |     100000 | 1,131.9149 ns | 102.1830 ns |   291.5339 ns | 1,100.0000 ns |
|       BitConverterTryWriteBytesByteArrayArrayCopy |     100000 | 1,372.0430 ns | 131.2144 ns |   372.2334 ns | 1,400.0000 ns |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy |     100000 | 1,030.5263 ns |  73.6848 ns |   211.4155 ns | 1,000.0000 ns |
|                              UnsafeAsPointerWrite |     100000 |    46.8750 ns |  22.4636 ns |    64.8125 ns |     0.0000 ns |
|                                 **DirectBufferWrite** |  **100000000** |   **720.2128 ns** | **145.5247 ns** |   **415.1900 ns** |   **700.0000 ns** |
|                     BitConverterGetBytesArrayCopy |  100000000 | 3,955.2083 ns | 419.0576 ns | 1,209.0771 ns | 3,950.0000 ns |
|               BitConverterGetBytesBufferBlockCopy |  100000000 | 4,253.6842 ns | 410.1817 ns | 1,176.8883 ns | 4,000.0000 ns |
|       BitConverterTryWriteBytesByteArrayArrayCopy |  100000000 | 4,670.5263 ns | 438.9766 ns | 1,259.5063 ns | 4,700.0000 ns |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy |  100000000 | 4,587.7778 ns | 491.2156 ns | 1,369.3128 ns | 4,300.0000 ns |
|                              UnsafeAsPointerWrite |  100000000 |   809.7826 ns | 121.9216 ns |   343.8821 ns |   800.0000 ns |
|                                 **DirectBufferWrite** | **1000000000** |   **432.9897 ns** | **153.8854 ns** |   **446.4493 ns** |   **300.0000 ns** |
|                     BitConverterGetBytesArrayCopy | 1000000000 | 5,094.0860 ns | 733.4149 ns | 2,080.5759 ns | 4,850.0000 ns |
|               BitConverterGetBytesBufferBlockCopy | 1000000000 | 5,105.0562 ns | 592.0394 ns | 1,640.5384 ns | 4,750.0000 ns |
|       BitConverterTryWriteBytesByteArrayArrayCopy | 1000000000 | 5,175.8065 ns | 489.8794 ns | 1,389.7062 ns | 5,050.0000 ns |
| BitConverterTryWriteBytesByteArrayBufferBlockCopy | 1000000000 | 4,666.4835 ns | 624.4923 ns | 1,751.1434 ns | 4,550.0000 ns |
|                              UnsafeAsPointerWrite | 1000000000 |   227.8947 ns | 116.9482 ns |   335.5462 ns |     0.0000 ns |