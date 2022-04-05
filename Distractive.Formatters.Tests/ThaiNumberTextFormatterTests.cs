using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Distractive.Formatters.Tests
{
    public class ThaiNumberTextFormatterTests
    {
        [Theory]
        [InlineData(0, "ศูนย์")]
        [InlineData(10, "สิบ")]
        [InlineData(11, "สิบเอ็ด")]
        [InlineData(21, "ยี่สิบเอ็ด")]
        [InlineData(40, "สี่สิบ")]
        [InlineData(42, "สี่สิบสอง")]
        [InlineData(100, "หนึ่งร้อย")]
        [InlineData(101, "หนึ่งร้อยเอ็ด")]
        [InlineData(300, "สามร้อย")]
        [InlineData(9_223372_036854_775807, "เก้าล้านสองแสนสองหมื่นสามพันสามร้อยเจ็ดสิบสองล้านสามหมื่นหกพันแปดร้อยห้าสิบสี่ล้านเจ็ดแสนเจ็ดหมื่นห้าพันแปดร้อยเจ็ด")]


        [InlineData(-300, "ลบสามร้อย")]
        [InlineData(-1, "ลบหนึ่ง")]
        public void FormatTest(long num, string formattedText)
        {
            var formatter = new ThaiNumberTextFormatter();
            var text = formatter.Format(num);
            Assert.Equal(formattedText, text);
        }

        private ref struct CustomRef
        {
            public CustomRef(Span<char> outputs)
            {
                Outputs = outputs;
                Position = 0;
            }
            //public bool IsValid;
            //public Span<int> Inputs;
            public readonly Span<char> Outputs { get; }
            public int Position;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Write(string s)
            {
                s.CopyTo(Outputs[Position..]);
                Position += s.Length;
            }
        }

        //private static void SetFirst(CustomRef custom, int output)
        //{
        //    custom.Outputs[0] = output;
        //}

        //[Fact]
        //public void TestRefStruct()
        //{
        //    //var c = new CustomRef
        //    //{
        //    //    Outputs = stackalloc int[10]
        //    //};

        //    // make sure the first one is zero
        //    c.Outputs[0] = 0;
        //    // set first output to 1
        //    SetFirst(c, 1);
        //    Assert.Equal(1, c.Outputs[0]);
        //}

        private static void TestBuffer(CustomRef c)
        {
            c.Write("a");
            Assert.Equal('a', c.Outputs[0]);
        }

        [Fact]
        public void TestRefStructConstructor()
        {
            var c = new CustomRef(stackalloc char[10]);

            // make sure the first one is zero
            c.Outputs[0] = '\0';
            c.Write("a");
            // set first output to 1
            //SetFirst(c, 1);
            Assert.Equal('a', c.Outputs[0]);
        }

        [Fact]
        public void TestRefStructParameter()
        {
            var c = new CustomRef(stackalloc char[10]);

            // make sure the first one is zero
            c.Outputs[0] = '\0';
            TestBuffer(c);
            // set first output to 1
            //SetFirst(c, 1);
            Assert.Equal('a', c.Outputs[0]);
        }

        //private static void Write(in ThaiNumberTextFormatter.CharBuffer b)
        //{
        //    b.Write("a");
        //}

        //[Fact]
        //public void BufferFunction()
        //{
        //    var buffer = new ThaiNumberTextFormatter.CharBuffer(stackalloc char[100]);
        //    Write(buffer);
        //    Assert.Equal('a', buffer.GetTrimmedSpan()[0]);
        //}
    }
}
