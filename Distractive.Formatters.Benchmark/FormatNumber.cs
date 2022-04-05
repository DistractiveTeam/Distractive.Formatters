using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Distractive.Formatters;

namespace Distractive.Formatters.Benchmark
{
    [MemoryDiagnoser]
    public class FormatNumber
    {
        private readonly ThaiNumberTextFormatter Formatter = new();        

        private decimal[] decimals = new decimal[] { 0 };
        private int idx;
        [GlobalSetup]
        public void Setup()
        {
            decimals = GetDecimals(50000).ToArray();
            idx = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private decimal GetNextDecimal()
        {
            if (++idx >= decimals.Length) idx = 0;
            return decimals[idx];
        }

        private IEnumerable<decimal> GetDecimals(int count)
        {
            Random random1 = new Random(123456);
            for(int i = 0; i < count; i++)
            {
                var exp = random1.NextDouble() * 10;
                var num = (long)Math.Pow(10, exp);
                yield return num + (random1.Next(100) * 0.01M);
            }
        }
   
        [Benchmark]        
        public string New() => Formatter.GetBahtText(GetNextDecimal());

        //[Benchmark]
        //public string New2() => Formatter.Format2(1234567);
    }
}
