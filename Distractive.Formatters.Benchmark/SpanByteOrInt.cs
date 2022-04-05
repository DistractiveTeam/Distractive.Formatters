using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Distractive.Formatters.Benchmark
{
    public class SpanByteOrInt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ReadOnlySpan<int> BuildDigits(in Span<int> buffer, long value)
        {
            Debug.Assert(value >= 0);

            int i = buffer.Length;
            do
            {
                buffer[--i] = ((int)(value % 10));
                value /= 10;
            } while (value > 0);

            return buffer[i..];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ReadOnlySpan<byte> BuildDigitsBytes(in Span<byte> buffer, long value)
        {
            Debug.Assert(value >= 0);

            int i = buffer.Length;
            do
            {
                buffer[--i] = (byte)(value % 10);
                value /= 10;
            } while (value > 0);

            return buffer[i..];
        }

        [Benchmark]
        public void Int()
        {
            BuildDigits(stackalloc int[20], 123456789123456);
        }

        [Benchmark]
        public void Bytes()
        {
            BuildDigitsBytes(stackalloc byte[20], 123456789123456);
        }
    }
}
