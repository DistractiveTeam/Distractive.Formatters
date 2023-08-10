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
        [InlineData(10_000_000, "สิบล้าน")]
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

        [Theory]
        [InlineData(-1, "ลบหนึ่งบาทถ้วน")]
        [InlineData(-1_000, "ลบหนึ่งพันบาทถ้วน")]
        [InlineData(0, "ศูนย์บาทถ้วน")]
        [InlineData(10, "สิบบาทถ้วน")]
        [InlineData(11, "สิบเอ็ดบาทถ้วน")]
        [InlineData(21, "ยี่สิบเอ็ดบาทถ้วน")]
        [InlineData(40, "สี่สิบบาทถ้วน")]
        [InlineData(42, "สี่สิบสองบาทถ้วน")]
        [InlineData(100, "หนึ่งร้อยบาทถ้วน")]
        [InlineData(101, "หนึ่งร้อยเอ็ดบาทถ้วน")]
        [InlineData(300, "สามร้อยบาทถ้วน")]
        [InlineData(10_000_000, "สิบล้านบาทถ้วน")]
        [InlineData(10_000_002_000_000_000_000, "สิบล้านสองล้านล้านบาทถ้วน")]
        [InlineData("-10_000_000_000_000_000_000", "ลบสิบล้านล้านล้านบาทถ้วน")]
        [InlineData("-10_000_000_000_000_000_000_000_000", "ลบสิบล้านล้านล้านล้านบาทถ้วน")]
        public void FormatBahtTuan(object num, string formattedText)
        {
            if (num is string s)
            {
                num = s.Replace("_", "");
            }
            var val = Convert.ToDecimal(num);
            var formatter = new ThaiNumberTextFormatter();
            var text = formatter.GetBahtText(val);
            Assert.Equal(formattedText, text);
        }

        [Fact]
        public void FormatBahtTuanMinMax()
        {
            var formatter = new ThaiNumberTextFormatter();
            var max = decimal.MaxValue; //79228_162514_264337_593543_950335m
            var text = formatter.GetBahtText(max);
            var expectedText =
                "เจ็ดหมื่นเก้าพันสองร้อยยี่สิบแปดล้าน" +
                "หนึ่งแสนหกหมื่นสองพันห้าร้อยสิบสี่ล้าน" +
                "สองแสนหกหมื่นสี่พันสามร้อยสามสิบเจ็ดล้าน" +
                "ห้าแสนเก้าหมื่นสามพันห้าร้อยสี่สิบสามล้าน" +
                "เก้าแสนห้าหมื่นสามร้อยสามสิบห้าบาทถ้วน";
            Assert.Equal(expectedText, text);
        }

        [Theory]
        [InlineData(0.10, "สิบสตางค์")]
        [InlineData(0.99, "เก้าสิบเก้าสตางค์")]
        [InlineData(9.99, "เก้าบาทเก้าสิบเก้าสตางค์")]
        [InlineData(10.10, "สิบบาทสิบสตางค์")]
        [InlineData(10.12, "สิบบาทสิบสองสตางค์")]
        public void FormatBahtStang(decimal num, string formattedText)
        {
            var formatter = new ThaiNumberTextFormatter();
            var text = formatter.GetBahtText(num);
            Assert.Equal(formattedText, text);
        }
    }
}
