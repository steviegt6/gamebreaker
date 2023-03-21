``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-RULYUL : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                                               Method |       N |          Mean |        Error |       StdDev |        Median |       Gen0 |    Allocated |
|----------------------------------------------------- |-------- |--------------:|-------------:|-------------:|--------------:|-----------:|-------------:|
|                                 **DirectBufferWriteAll** |    **1000** |     **273.23 μs** |     **4.068 μs** |     **3.606 μs** |     **271.95 μs** |          **-** |    **173.13 KB** |
|                     BitConverterGetBytesArrayCopyAll |    1000 |     312.16 μs |     6.211 μs |    14.760 μs |     306.80 μs |          - |    516.88 KB |
|               BitConverterGetBytesBufferBlockCopyAll |    1000 |     301.61 μs |     5.034 μs |     4.462 μs |     300.55 μs |          - |    516.88 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopyAll |    1000 |     534.87 μs |    10.660 μs |    14.943 μs |     530.50 μs |          - |    516.88 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopyAll |    1000 |     456.88 μs |     2.909 μs |     2.271 μs |     457.20 μs |          - |    516.88 KB |
|                              UnsafeAsPointerWriteAll |    1000 |      89.44 μs |     0.889 μs |     0.742 μs |      89.60 μs |          - |    110.63 KB |
|                                 **DirectBufferWriteAll** |  **100000** |  **11,751.08 μs** |   **234.235 μs** |   **556.685 μs** |  **11,562.80 μs** |          **-** |  **17188.76 KB** |
|                     BitConverterGetBytesArrayCopyAll |  100000 |  16,562.77 μs |   329.573 μs |   634.975 μs |  16,555.00 μs |  4000.0000 |  51563.76 KB |
|               BitConverterGetBytesBufferBlockCopyAll |  100000 |  15,945.68 μs |   310.761 μs |   425.373 μs |  15,823.95 μs |  4000.0000 |  51563.76 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopyAll |  100000 |  16,954.35 μs |   372.480 μs | 1,086.541 μs |  17,288.40 μs |  4000.0000 |  51563.76 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopyAll |  100000 |  17,466.07 μs |   347.712 μs |   372.048 μs |  17,405.60 μs |  4000.0000 |  51563.76 KB |
|                              UnsafeAsPointerWriteAll |  100000 |   6,975.41 μs |   180.433 μs |   505.952 μs |   7,020.30 μs |          - |  10938.76 KB |
|                                 **DirectBufferWriteAll** | **1000000** | **117,153.91 μs** | **2,244.571 μs** | **2,099.573 μs** | **117,683.80 μs** |  **7000.0000** | **171876.26 KB** |
|                     BitConverterGetBytesArrayCopyAll | 1000000 | 156,271.02 μs | 3,093.401 μs | 3,309.904 μs | 156,404.05 μs | 49000.0000 | 515626.26 KB |
|               BitConverterGetBytesBufferBlockCopyAll | 1000000 | 152,207.19 μs | 2,754.388 μs | 4,525.539 μs | 150,534.40 μs | 49000.0000 | 515626.26 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopyAll | 1000000 | 141,400.89 μs | 1,725.934 μs | 1,529.995 μs | 141,012.15 μs | 49000.0000 | 515626.26 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopyAll | 1000000 | 142,416.09 μs | 2,092.381 μs | 1,747.233 μs | 141,881.30 μs | 49000.0000 | 515626.26 KB |
|                              UnsafeAsPointerWriteAll | 1000000 |  53,193.24 μs | 1,055.001 μs | 1,642.510 μs |  52,874.15 μs |          - | 109376.26 KB |
