``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-ATKFOU : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                                               Method |       N |          Mean |        Error |        StdDev |        Median |       Gen0 |    Allocated |
|----------------------------------------------------- |-------- |--------------:|-------------:|--------------:|--------------:|-----------:|-------------:|
|                                 **DirectBufferWriteAll** |    **1000** |     **519.66 μs** |     **4.316 μs** |      **3.826 μs** |     **520.25 μs** |          **-** |    **173.13 KB** |
|                     BitConverterGetBytesArrayCopyAll |    1000 |     303.35 μs |     5.811 μs |      5.152 μs |     301.55 μs |          - |    516.88 KB |
|               BitConverterGetBytesBufferBlockCopyAll |    1000 |     311.58 μs |     6.228 μs |     11.849 μs |     307.50 μs |          - |    516.88 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopyAll |    1000 |     491.75 μs |     9.598 μs |      8.015 μs |     488.80 μs |          - |    516.88 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopyAll |    1000 |     500.48 μs |     9.920 μs |     10.187 μs |     504.10 μs |          - |    516.88 KB |
|                              UnsafeAsPointerWriteAll |    1000 |      88.72 μs |     0.644 μs |      0.502 μs |      88.85 μs |          - |    110.63 KB |
|                                 **DirectBufferWriteAll** |  **100000** |  **15,348.15 μs** |   **305.582 μs** |    **714.288 μs** |  **15,128.80 μs** |          **-** |  **17188.76 KB** |
|                     BitConverterGetBytesArrayCopyAll |  100000 |  17,019.78 μs |   339.774 μs |    901.033 μs |  16,934.00 μs |  4000.0000 |  51563.76 KB |
|               BitConverterGetBytesBufferBlockCopyAll |  100000 |  15,556.66 μs |   206.598 μs |    161.298 μs |  15,571.20 μs |  4000.0000 |  51563.76 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopyAll |  100000 |  16,627.23 μs |   329.879 μs |    874.792 μs |  17,009.45 μs |  4000.0000 |  51563.76 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopyAll |  100000 |  17,440.83 μs |   338.147 μs |    516.386 μs |  17,429.80 μs |  4000.0000 |  51563.76 KB |
|                              UnsafeAsPointerWriteAll |  100000 |   7,165.48 μs |   191.273 μs |    542.610 μs |   7,222.70 μs |          - |  10938.76 KB |
|                                 **DirectBufferWriteAll** | **1000000** | **147,810.37 μs** | **2,151.154 μs** |  **2,012.191 μs** | **148,199.60 μs** |  **7000.0000** | **171876.26 KB** |
|                     BitConverterGetBytesArrayCopyAll | 1000000 | 162,924.05 μs | 3,111.458 μs |  3,583.159 μs | 163,052.30 μs | 49000.0000 | 515626.26 KB |
|               BitConverterGetBytesBufferBlockCopyAll | 1000000 | 159,136.54 μs |   882.536 μs |    736.957 μs | 159,100.20 μs | 49000.0000 | 515626.26 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopyAll | 1000000 | 154,259.04 μs | 3,709.686 μs | 10,583.942 μs | 153,282.70 μs | 49000.0000 | 515626.26 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopyAll | 1000000 | 163,586.28 μs | 4,526.250 μs | 13,274.707 μs | 159,961.90 μs | 49000.0000 | 515626.26 KB |
|                              UnsafeAsPointerWriteAll | 1000000 |  54,310.79 μs | 1,076.282 μs |  2,124.476 μs |  55,019.65 μs |          - | 109376.26 KB |
