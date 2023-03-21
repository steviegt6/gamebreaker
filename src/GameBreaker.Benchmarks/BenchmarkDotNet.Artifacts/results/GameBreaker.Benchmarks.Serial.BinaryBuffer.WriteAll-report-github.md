``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-ZHYCAJ : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                                               Method |       N |          Mean |        Error |       StdDev |        Median |       Gen0 |    Allocated |
|----------------------------------------------------- |-------- |--------------:|-------------:|-------------:|--------------:|-----------:|-------------:|
|                                 **DirectBufferWriteAll** |    **1000** |     **271.32 μs** |     **5.041 μs** |     **4.209 μs** |     **270.70 μs** |          **-** |    **173.13 KB** |
|                     BitConverterGetBytesArrayCopyAll |    1000 |     303.83 μs |     5.964 μs |     8.164 μs |     300.50 μs |          - |    516.88 KB |
|               BitConverterGetBytesBufferBlockCopyAll |    1000 |     315.61 μs |     6.288 μs |    17.213 μs |     307.10 μs |          - |    516.88 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopyAll |    1000 |     484.72 μs |     3.233 μs |     2.700 μs |     484.10 μs |          - |    516.88 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopyAll |    1000 |     486.23 μs |     9.630 μs |     9.008 μs |     487.20 μs |          - |    516.88 KB |
|                              UnsafeAsPointerWriteAll |    1000 |      90.39 μs |     0.729 μs |     0.646 μs |      90.45 μs |          - |    110.63 KB |
|                                 **DirectBufferWriteAll** |  **100000** |  **11,882.55 μs** |   **249.568 μs** |   **735.856 μs** |  **11,468.10 μs** |          **-** |  **17188.76 KB** |
|                     BitConverterGetBytesArrayCopyAll |  100000 |  17,723.15 μs |   411.506 μs | 1,180.688 μs |  17,748.80 μs |  4000.0000 |  51563.76 KB |
|               BitConverterGetBytesBufferBlockCopyAll |  100000 |  16,439.46 μs |   281.681 μs |   562.548 μs |  16,324.30 μs |  4000.0000 |  51563.76 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopyAll |  100000 |  16,812.61 μs |   343.529 μs | 1,002.091 μs |  17,056.90 μs |  4000.0000 |  51563.76 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopyAll |  100000 |  18,264.04 μs |   358.233 μs |   707.117 μs |  18,292.10 μs |  4000.0000 |  51563.76 KB |
|                              UnsafeAsPointerWriteAll |  100000 |   7,069.17 μs |   180.497 μs |   509.095 μs |   7,169.10 μs |          - |  10938.76 KB |
|                                 **DirectBufferWriteAll** | **1000000** | **115,998.29 μs** | **1,857.840 μs** | **1,737.824 μs** | **115,996.10 μs** |  **7000.0000** | **171876.26 KB** |
|                     BitConverterGetBytesArrayCopyAll | 1000000 | 154,441.11 μs | 2,091.157 μs | 1,853.756 μs | 154,828.85 μs | 49000.0000 | 515626.26 KB |
|               BitConverterGetBytesBufferBlockCopyAll | 1000000 | 160,404.77 μs | 3,121.838 μs | 4,672.621 μs | 162,112.15 μs | 49000.0000 | 515626.26 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopyAll | 1000000 | 144,748.48 μs | 2,646.058 μs | 4,634.357 μs | 143,050.80 μs | 49000.0000 | 515626.26 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopyAll | 1000000 | 156,664.50 μs | 3,104.793 μs | 4,144.810 μs | 157,619.60 μs | 49000.0000 | 515626.26 KB |
|                              UnsafeAsPointerWriteAll | 1000000 |  53,904.86 μs | 1,071.448 μs | 2,038.541 μs |  54,666.20 μs |          - | 109376.26 KB |
