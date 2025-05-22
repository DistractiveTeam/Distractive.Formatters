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
        public void ExtensionTests_DefaultOptions()
        {
            Assert.Equal("หนึ่งบาทถ้วน", 1M.ToBahtText());
            Assert.Equal("หนึ่ง", 1L.ToThaiWords());
            Assert.Equal("หนึ่ง", 1.ToThaiWords());
        }

        [Fact]
        public void IntToThaiWords_WithBooleanFlag_ShouldUseCorrectOption()
        {
            // Test the specific int overload that was fixed for StackOverflow,
            // and ensure it respects the boolean flag.
            Assert.Equal("หนึ่งร้อยหนึ่ง", 101.ToThaiWords(true)); // EtWithTensOnly
            Assert.Equal("หนึ่งร้อยเอ็ด", 101.ToThaiWords(false)); // Default
            Assert.Equal("ยี่สิบเอ็ด", 21.ToThaiWords(true)); // Should be "ยี่สิบเอ็ด" regardless of flag as 'เอ็ด' is part of "ยี่สิบเอ็ด"
            Assert.Equal("ยี่สิบเอ็ด", 21.ToThaiWords(false));
            Assert.Equal("หนึ่ง", 1.ToThaiWords(true)); // For numbers like 1, 2, ..., 10, 20, 100 the flag has no effect on the word "หนึ่ง" itself.
            Assert.Equal("หนึ่ง", 1.ToThaiWords(false));
        }

        [Fact]
        public void ExtensionMethods_WithBooleanFlag_Options()
        {
            // Long with boolean
            Assert.Equal("หนึ่งร้อยหนึ่ง", 101L.ToThaiWords(true));
            Assert.Equal("หนึ่งร้อยเอ็ด", 101L.ToThaiWords(false));
            Assert.Equal("สิบเอ็ดล้านหนึ่ง", 11_000_001L.ToThaiWords(true));
            Assert.Equal("สิบเอ็ดล้านเอ็ด", 11_000_001L.ToThaiWords(false));


            // Decimal with boolean
            Assert.Equal("หนึ่งร้อยหนึ่งบาทถ้วน", 101M.ToBahtText(true));
            Assert.Equal("หนึ่งร้อยเอ็ดบาทถ้วน", 101M.ToBahtText(false));
            Assert.Equal("หนึ่งร้อยหนึ่งบาทสิบสตางค์", 101.10M.ToBahtText(true));
            Assert.Equal("หนึ่งร้อยเอ็ดบาทสิบสตางค์", 101.10M.ToBahtText(false));
        }

        [Fact]
        public void ExtensionMethods_WithOptionsObject()
        {
            var etTensOnlyOpts = ThaiNumberTextFormatterOptions.EtWithTensOnly;
            var defaultOpts = ThaiNumberTextFormatterOptions.Default;

            // Int with options object
            Assert.Equal("หนึ่งร้อยหนึ่ง", 101.ToThaiWords(etTensOnlyOpts));
            Assert.Equal("หนึ่งร้อยเอ็ด", 101.ToThaiWords(defaultOpts));

            // Long with options object
            Assert.Equal("หนึ่งร้อยหนึ่ง", 101L.ToThaiWords(etTensOnlyOpts));
            Assert.Equal("หนึ่งร้อยเอ็ด", 101L.ToThaiWords(defaultOpts));
            Assert.Equal("สิบเอ็ดล้านหนึ่ง", 11_000_001L.ToThaiWords(etTensOnlyOpts));
            Assert.Equal("สิบเอ็ดล้านเอ็ด", 11_000_001L.ToThaiWords(defaultOpts));

            // Decimal with options object
            Assert.Equal("หนึ่งร้อยหนึ่งบาทถ้วน", 101M.ToBahtText(etTensOnlyOpts));
            Assert.Equal("หนึ่งร้อยเอ็ดบาทถ้วน", 101M.ToBahtText(defaultOpts));
            Assert.Equal("หนึ่งร้อยหนึ่งบาทสิบสตางค์", 101.10M.ToBahtText(etTensOnlyOpts));
            Assert.Equal("หนึ่งร้อยเอ็ดบาทสิบสตางค์", 101.10M.ToBahtText(defaultOpts));
        }
    }
}
