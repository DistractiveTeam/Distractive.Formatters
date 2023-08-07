using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Distractive.Formatters.Benchmark
{
    public class BigDecimalDivisionBenchmark
    {
        [Benchmark]
        [SkipLocalsInit]
        public long DivisionWithCheck()
        {
            decimal d = 1_234_567M;
            var dd = 1_000_000_000_000_000;
            long big = (long)(d >= dd ? d / dd : 0);
            long small = (long)(d % dd);
            return big + small;
        }

        [Benchmark]
        [SkipLocalsInit]
        public long DivisionWithoutCheck()
        {
            decimal d = 1_234_567M;
            var dd = 1_000_000_000_000_000;
            long big = (long)(d / dd);
            long small = (long)(d % dd);
            return big + small;
        }
    }
}
