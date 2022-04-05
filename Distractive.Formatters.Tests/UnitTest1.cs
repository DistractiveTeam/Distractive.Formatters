using Distractive.Formatters;
using Xunit;

namespace Distractive.ThaiBahtFormatter.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(0, "�ٹ��")]
        [InlineData(10, "�Ժ")]
        [InlineData(11, "�Ժ���")]
        [InlineData(21, "����Ժ���")]
        [InlineData(40, "����Ժ")]
        [InlineData(42, "����Ժ�ͧ")]
        [InlineData(100, "˹������")]
        [InlineData(101, "˹���������")]
        [InlineData(300, "�������")]
        [InlineData(10_000_000, "�Ժ��ҹ")]
        public void FormatNumber(long num, string formattedText)
        {
            var formatter = new ThaiNumberTextFormatter();
            var text = formatter.Format(num);
            Assert.Equal(formattedText, text);
        }

        [Theory]
        [InlineData(-1, "ź˹�觺ҷ��ǹ")]
        [InlineData(0, "�ٹ��ҷ��ǹ")]
        [InlineData(10, "�Ժ�ҷ��ǹ")]
        [InlineData(11, "�Ժ��紺ҷ��ǹ")]
        [InlineData(21, "����Ժ��紺ҷ��ǹ")]
        [InlineData(40, "����Ժ�ҷ��ǹ")]
        [InlineData(42, "����Ժ�ͧ�ҷ��ǹ")]
        [InlineData(100, "˹�����ºҷ��ǹ")]
        [InlineData(101, "˹��������紺ҷ��ǹ")]
        [InlineData(300, "������ºҷ��ǹ")]
        [InlineData(10_000_000, "�Ժ��ҹ�ҷ��ǹ")]
        [InlineData(10_000_002_000_000_000_000, "�Ժ��ҹ�ͧ��ҹ��ҹ�ҷ��ǹ")]
        [InlineData(10_000_000_000_000_000_000, "�Ժ��ҹ��ҹ��ҹ�ҷ��ǹ")]
        public void FormatBahtTuan(decimal num, string formattedText)
        {
            var formatter = new ThaiNumberTextFormatter();
            var text = formatter.GetBahtText(num);
            Assert.Equal(formattedText, text);
        }

        [Fact]
        public void FormatBahtTuanMinMax()
        {
            var formatter = new ThaiNumberTextFormatter();
            var max = decimal.MaxValue; //79228_162514_264337_593543_950335m
            var text = formatter.GetBahtText(max);            
            var expectedText = 
                "��������Ҿѹ�ͧ��������ԺỴ��ҹ" +
                "˹���ʹˡ�����ͧ�ѹ��������Ժ�����ҹ" + 
                "�ͧ�ʹˡ�������ѹ�����������Ժ����ҹ" + 
                "����ʹ�����������ѹ�����������Ժ�����ҹ" + 
                "����ʹ������������������Ժ��Һҷ��ǹ";
            Assert.Equal(expectedText, text);
        }

        [Theory]
        [InlineData(0.10, "�Ժʵҧ��")]
        [InlineData(0.99, "����Ժ���ʵҧ��")]
        [InlineData(9.99, "��Һҷ����Ժ���ʵҧ��")]
        [InlineData(10.10, "�Ժ�ҷ�Ժʵҧ��")]
        [InlineData(10.12, "�Ժ�ҷ�Ժ�ͧʵҧ��")]
        public void FormatBahtStang(decimal num, string formattedText)
        {
            var formatter = new ThaiNumberTextFormatter();
            var text = formatter.GetBahtText(num);
            Assert.Equal(formattedText, text);
        }
    }
}