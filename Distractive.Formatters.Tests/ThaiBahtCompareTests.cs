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
        [InlineData(1, 1_200_000)]
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

        [Theory]
        [InlineData(1, 2_000_000)]
        [InlineData(-999999999999999999, -999999999999999999+1_000_000)]
        public void CompareLong(long start, long end)
        {
            var formatter = new ThaiNumberTextFormatter();
            for (long i = start; i <= end; i++)
            {
                string s2 = formatter.Format(i) + "บาทถ้วน";
                string s1 = GreatFriends.ThaiBahtText.ThaiBahtTextUtil.ThaiBahtText(i);                
                Assert.Equal(s1, s2);
            }
        }
    }
}
