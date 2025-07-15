using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Distractive.Formatters.Tests
{
    public class FormatterExtensionsTests
    {
        [Fact]
        public void ExtensionTests()
        {
            Assert.Equal("หนึ่งบาทถ้วน", 1M.ToBahtText());
            Assert.Equal("หนึ่ง", 1L.ToThaiWords());
            Assert.Equal("หนึ่ง", 1.ToThaiWords());
        }

        [Theory]
        [InlineData(42, "สี่สิบสอง")]
        [InlineData(101, "หนึ่งร้อยหนึ่ง")]
        [InlineData(121, "หนึ่งร้อยยี่สิบเอ็ด")]
        [InlineData(1001, "หนึ่งพันหนึ่ง")]
        public void ToThaiWords_IntWithBoolParameter_ShouldNotCauseStackOverflow(int value, string expected)
        {
            // This test ensures that the ToThaiWords(int, bool) method 
            // doesn't cause stack overflow by calling itself recursively
            string result = value.ToThaiWords(true);
            Assert.Equal(expected, result);
            
            // Also verify it matches the long version behavior
            string longResult = ((long)value).ToThaiWords(true);
            Assert.Equal(longResult, result);
        }
    }
}
