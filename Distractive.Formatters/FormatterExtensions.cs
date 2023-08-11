using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distractive.Formatters
{
    public static class FormatterExtensions
    {        
        private static readonly ThaiNumberTextFormatter[] _formatters
            = new ThaiNumberTextFormatter[(int)ThaiNumberTextFormatFlags.TotalPerm] 
                { new(), new(ThaiNumberTextFormatterOptions.EtWithTensOnly) };
        private static ThaiNumberTextFormatter Get(ThaiNumberTextFormatterOptions options)
            => Get(options.FormatFlags);

        private static ThaiNumberTextFormatter Get(ThaiNumberTextFormatFlags flags) => _formatters[(int)flags];
        

        public static string ToThaiWords(this long value) => _formatters[0].Format(value);
        public static string ToThaiWords(this int value) => ToThaiWords((long)value);

        public static string ToBahtText(this decimal value) => _formatters[0].GetBahtText(value);

        public static string ToThaiWords(this long value, 
            ThaiNumberTextFormatterOptions options) => Get(options).Format(value);
        
        public static string ToThaiWords(this int value, 
            ThaiNumberTextFormatterOptions options) => Get(options).Format((long)value);

        public static string ToBahtText(this decimal value, 
            ThaiNumberTextFormatterOptions options) => Get(options).GetBahtText(value);

        public static string ToThaiWords(this long value, bool useEtWithTensOnly) 
            => _formatters[useEtWithTensOnly ? 1 : 0].Format(value);

        public static string ToThaiWords(this int value, bool useEtWithTensOnly) => ToThaiWords(value, useEtWithTensOnly);

        public static string ToBahtText(this decimal value,
            bool useEtWithTensOnly) => _formatters[useEtWithTensOnly ? 1 : 0].GetBahtText(value);
    }
}
