using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distractive.Formatters
{
    public static class FormatterExtensions
    {
        private static readonly ThaiNumberTextFormatter _formatter = new ThaiNumberTextFormatter();

        public static string ToThaiWords(this long value) => _formatter.Format(value);
        public static string ToThaiWords(this int value) => ToThaiWords((long)value);

        public static string ToBahtText(this decimal value) => _formatter.GetBahtText(value);
    }
}
