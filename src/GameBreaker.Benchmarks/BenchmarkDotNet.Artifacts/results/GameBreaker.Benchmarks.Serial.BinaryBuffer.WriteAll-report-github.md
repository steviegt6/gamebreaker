``` ini

BenchmarkDotNet=v0.13.5, OS=Windows 10 (10.0.19045.2728/22H2/2022Update)
AMD Ryzen 7 5700G with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2
  Job-FBKXCO : .NET 7.0.4 (7.0.423.11508), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
|                                               Method |       N |          Mean |        Error |       StdDev |        Median |       Gen0 |    Allocated |
|----------------------------------------------------- |-------- |--------------:|-------------:|-------------:|--------------:|-----------:|-------------:|
|                                 **DirectBufferWriteAll** |    **1000** |     **269.74 μs** |     **4.114 μs** |     **3.212 μs** |     **268.95 μs** |          **-** |    **173.13 KB** |
|                     BitConverterGetBytesArrayCopyAll |    1000 |     315.79 μs |     8.132 μs |    23.070 μs |     304.90 μs |          - |    516.88 KB |
|               BitConverterGetBytesBufferBlockCopyAll |    1000 |     326.10 μs |    11.236 μs |    32.238 μs |     310.10 μs |          - |    516.88 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopyAll |    1000 |     472.43 μs |     5.741 μs |     4.794 μs |     472.10 μs |          - |    516.88 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopyAll |    1000 |     481.06 μs |     9.504 μs |    21.453 μs |     475.20 μs |          - |    516.88 KB |
|                              UnsafeAsPointerWriteAll |    1000 |      90.44 μs |     0.413 μs |     0.442 μs |      90.45 μs |          - |    110.63 KB |
|                                 **DirectBufferWriteAll** |  **100000** |  **12,139.53 μs** |   **275.281 μs** |   **807.352 μs** |  **12,039.90 μs** |          **-** |  **17188.76 KB** |
|                     BitConverterGetBytesArrayCopyAll |  100000 |  17,295.20 μs |   344.229 μs |   953.856 μs |  17,207.10 μs |  4000.0000 |  51563.76 KB |
|               BitConverterGetBytesBufferBlockCopyAll |  100000 |  15,904.32 μs |   266.528 μs |   422.742 μs |  15,944.60 μs |  4000.0000 |  51563.76 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopyAll |  100000 |  16,649.95 μs |   331.455 μs |   940.283 μs |  16,845.10 μs |  4000.0000 |  51563.76 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopyAll |  100000 |  17,461.68 μs |   349.100 μs |   620.524 μs |  17,405.75 μs |  4000.0000 |  51563.76 KB |
|                              UnsafeAsPointerWriteAll |  100000 |   7,202.03 μs |   172.211 μs |   482.899 μs |   7,263.10 μs |          - |  10938.76 KB |
|                                 **DirectBufferWriteAll** | **1000000** | **114,250.51 μs** | **2,220.766 μs** | **2,181.089 μs** | **114,357.55 μs** |  **7000.0000** | **171876.26 KB** |
|                     BitConverterGetBytesArrayCopyAll | 1000000 | 160,195.62 μs | 3,140.866 μs | 4,981.747 μs | 159,702.00 μs | 49000.0000 | 515626.26 KB |
|               BitConverterGetBytesBufferBlockCopyAll | 1000000 | 154,599.02 μs | 3,027.573 μs | 4,041.723 μs | 155,129.00 μs | 49000.0000 | 515626.26 KB |
|       BitConverterTryWriteBytesByteArrayArrayCopyAll | 1000000 | 148,039.01 μs | 2,944.700 μs | 4,407.489 μs | 147,903.55 μs | 49000.0000 | 515626.26 KB |
| BitConverterTryWriteBytesByteArrayBufferBlockCopyAll | 1000000 | 149,199.09 μs | 2,901.516 μs | 3,563.322 μs | 149,111.90 μs | 49000.0000 | 515626.26 KB |
|                              UnsafeAsPointerWriteAll | 1000000 |  56,372.04 μs | 1,120.098 μs | 2,158.048 μs |  56,311.70 μs |          - | 109376.26 KB |
