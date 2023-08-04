using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftAntimalwareEngine;
using digit = System.Int32;

namespace Distractive.Formatters.Benchmark
{
    public class CalculationBenchmark
    {
        private static decimal MaxInteger = decimal.Truncate(decimal.MaxValue);
        private static long _big = (long) (MaxInteger / 1_000_000_000_000_000);
        private static long _small = (long)(MaxInteger % 1_000_000_000_000_000);

        // Summary
        // 1. DecimalToString is as fast as split to big and small
        // 2. Split to millons is slower
        // 3. Mod 10 loop is even slower

        [Benchmark]
        [SkipLocalsInit]
        public int DecimalSplitMod100()
        {
            var v = MaxInteger;
            var big = (long)(v / 1_000_000_000_000_000);
            var small = (long)(v % 1_000_000_000_000_000);
            //var big = _big;
            //var small = _small;
            Span<digit> d = stackalloc digit[30];
            int i = d.Length - 1;
            do
            {                
                (small, long rem) = Math.DivRem(small, 100);
                d[i--] = (digit)(rem);
            } while (small > 0);
            do
            {
                (big, long rem) = Math.DivRem(big, 100);
                d[i--] = (digit)(rem);
            } while (big > 0);

            return d[0];
        }

        [Benchmark]
        [SkipLocalsInit]
        public string DecimalToString()
        {
            return MaxInteger.ToString();
        }

        [Benchmark]
        [SkipLocalsInit]
        public int SplitToMillions()
        {
            Span<int> buffer = stackalloc int[5];
            var value = MaxInteger;
            var md = 1_000_000M;
            Debug.Assert(value >= 0);

            int i = 0;
            do
            {
                buffer[i++] = (int)(value % md);
                value /= md;
            } while (value >= 1);

            Span<digit> d = stackalloc digit[30];
            i = d.Length - 1;
            for (int j = buffer.Length; j > 0; j--)
            {
                var val = buffer[^j];
                do
                {
                    (val, var rem) = Math.DivRem(val, 10);
                    d[i--] = rem;
                } while (val > 0);
            }

            return d[0];
        }        

        [Benchmark]
        [SkipLocalsInit]
        public digit DecimalSplitMod10()
        {
            var v = MaxInteger;
            var big = (long) (v / 1_000_000_000_000_000);
            var small = (long) (v % 1_000_000_000_000_000);
            //var big = _big;
            //var small = _small;
            Span<digit> d = stackalloc digit[30];
            int i = d.Length - 1;
            do
            {
                (small, long rem) = Math.DivRem(small, 10);
                d[i--] = (digit)(rem);
            } while (small > 0);
            do
            {
                (big, long rem) = Math.DivRem(big, 10);
                d[i--] = (digit)(rem);
            } while (big > 0);
            
            return d[0];
        }        

        [Benchmark]
        [SkipLocalsInit]
        public int DecimalMod10()
        {
            var v = MaxInteger;
            Span<int> d = stackalloc int[40];
            int i = 39;
            do
            {
                d[i--] = (int)(v % 10);
                v = decimal.Truncate(v / 10);
            } while (v > 0);
            return d[0];
        }

    }
}
