// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using Distractive.Formatters.Benchmark;

//var summary = BenchmarkRunner.Run<ThaiBahtTextCompare>();
//var summary = BenchmarkRunner.Run<FormatNumber>();
//var summary = BenchmarkRunner.Run<SpanByteOrInt>();
//BenchmarkRunner.Run<SwitchCaseVsArray>();
var summary = BenchmarkRunner.Run<CalculationBenchmark>();

Console.WriteLine("Hello, World!");
