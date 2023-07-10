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
    }
}
