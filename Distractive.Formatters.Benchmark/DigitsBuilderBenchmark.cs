using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Distractive.Formatters.Benchmark
{
    public class DigitsBuilderBenchmark
    {
        private long GetRandom() => Random.Shared.NextInt64(); //1_250_231_315;
        

        [Benchmark]
        public int UseSpan100()
        {
            static ReadOnlySpan<int> BuildDigits(Span<int> buffer, long value)
            {
                Debug.Assert(value >= 0);

                int i = buffer.Length;
                do
                {
                    value = Math.DivRem(value, 100, out var r);
                    buffer[--i] = (int)r;
                    buffer[--i] = (int)r;
                    //buffer[--i] = (int)r;
                } while (value > 0);

                return buffer[i..];
            }
            Span<int> span = stackalloc int[36];
            BuildDigits(span, GetRandom());
            return span[0];
        }

        [Benchmark]
        public int UseSpan()
        {            
            static ReadOnlySpan<int> BuildDigits(Span<int> buffer, long value)
            {                
                Debug.Assert(value >= 0);

                int i = buffer.Length;
                do
                {
                    value = Math.DivRem(value, 10, out var r);
                    buffer[--i] = (int)r;
                } while (value > 0);

                return buffer[i..];
            }
            Span<int> span = stackalloc int[36];
            BuildDigits(span, GetRandom());
            return span[0];
        }

        [Benchmark]
        public string UseToString()
        {
            return GetRandom().ToString();
        }
    }
}
