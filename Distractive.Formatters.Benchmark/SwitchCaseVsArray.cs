using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Distractive.Formatters.Benchmark
{
    public class SwitchCaseVsArray
    {
        private static readonly string[] _numbers = new[] { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" };

        private int[] indices = new int[] { 0 };
        private int idx;
        [GlobalSetup]
        public void Setup()
        {
            indices = GetRandomInts(50000).ToArray();

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetNext()
        {
            if (idx == 0) idx = indices.Length;
            return indices[--idx];
        }

        private IEnumerable<int> GetRandomInts(int count)
        {
            Random random1 = new Random(123456);
            for (int i = 0; i < count; i++)
            {
                yield return random1.Next(10);
            }
        }

        [Benchmark]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public string SwitchCaseNew()
        {
            int n = GetNext();
            return n switch
            {
                0 => "ศูนย์",
                1 => "หนึ่ง",
                2 => "สอง",
                3 => "สาม",
                4 => "สี่",
                5 => "ห้า",
                6 => "หก",
                7 => "เจ็ด",
                8 => "แปด",
                9 => "เก้า",
                _ => "",
            };
        }

        [Benchmark]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public string SwitchCase()
        {
            int n = GetNext();
            switch (n)
            {
                case 0: return "ศูนย์";
                case 1: return "หนึ่ง";
                case 2: return "สอง";
                case 3: return "สาม";
                case 4: return "สี่";
                case 5: return "ห้า";
                case 6: return "หก";
                case 7: return "เจ็ด";
                case 8: return "แปด";
                case 9: return "เก้า";
                default: return "";
            }
        }

        [Benchmark]
        public string Array()
        {
            //int n = Random.Shared.Next(10);
            int n = GetNext();
            return _numbers[n];
        }


    }
}
