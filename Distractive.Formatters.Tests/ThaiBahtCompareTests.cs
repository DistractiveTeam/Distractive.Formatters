using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Distractive.Formatters.Tests
{
    public class ThaiBahtCompareTests
    {
        [Theory]
        [InlineData(1_000_000, 11_000_000)]
        public void Compare(int start, int end)
        {
            var formatter = new ThaiNumberTextFormatter();
            for(int i = start; i <= end; i++)
            {
                string s1 = GreatFriends.ThaiBahtText.ThaiBahtTextUtil.ThaiBahtText(i);
                string s2 = formatter.GetBahtText(i);
                Assert.Equal(s1, s2);
            }
        }
    }
}
