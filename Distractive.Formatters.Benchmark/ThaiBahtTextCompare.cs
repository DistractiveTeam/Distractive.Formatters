using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using GreatFriends.ThaiBahtText;
using NumberToThaiText;

namespace Distractive.Formatters.Benchmark
{
    [MemoryDiagnoser]
    //[EventPipeProfiler(EventPipeProfile.CpuSampling)]
    public class ThaiBahtTextCompare
    {
        private decimal[] decimals = new decimal[] { 0 };
        private string[] decimalStrings = new[] { "" };
        private long[] longs = new long[] { 0 };
        private int idx;
        [GlobalSetup]
        public void Setup()
        {
            decimals = GetDecimals(50000).ToArray();
            longs = decimals.Select(d => (long)d).ToArray();
            decimalStrings = decimals.Select(i => i.ToString()).ToArray();
        }
        private IEnumerable<decimal> GetDecimals(int count)
        {
            Random random1 = new Random(123456);
            for (int i = 0; i < count; i++)
            {
                var exp = random1.NextDouble() * 10;
                var num = (long)Math.Pow(10, exp);
                yield return num + (random1.Next(100) * 0.01M);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private decimal GetNextDecimal()
        {
            if (idx == 0) idx = decimals.Length;
            return decimals[--idx];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetNextDecimalString()
        {
            if (idx == 0) idx = decimals.Length;
            return decimalStrings[--idx];
        }

        private long GetNextLong()
        {
            if (idx == 0) idx = longs.Length;
            return longs[--idx];
        }


        private readonly ThaiNumberTextFormatter formatter = new();

        [Benchmark]
        [ArgumentsSource(nameof(GetNextDecimal))]
        public string DistractiveLong() => formatter.Format(GetNextLong());

        [Benchmark]
        [ArgumentsSource(nameof(GetNextDecimal))]
        public string Distractive() => formatter.GetBahtText(GetNextDecimal());

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(GetNextDecimal))]
        public string ThaiBahtText() => ThaiBahtTextUtil.ThaiBahtText(GetNextDecimal());        

        private NumToThaiTextConverter numToThaiText = new();
        [Benchmark]
        [ArgumentsSource(nameof(GetNextDecimal))]
        public object NumberToThaiText() => numToThaiText.Convert(GetNextDecimalString(), true, true);
    }
}
