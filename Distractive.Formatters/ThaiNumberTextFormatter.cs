using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Distractive.Formatters;


public sealed class ThaiNumberTextFormatter
{
    private static readonly string[][] _numberGrid = new[] {
        new[] { "", "หนึ่งแสน", "สองแสน", "สามแสน", "สี่แสน", "ห้าแสน", "หกแสน", "เจ็ดแสน", "แปดแสน", "เก้าแสน" },
        new[] { "", "หนึ่งหมื่น", "สองหมื่น", "สามหมื่น", "สี่หมื่น", "ห้าหมื่น", "หกหมื่น", "เจ็ดหมื่น", "แปดหมื่น", "เก้าหมื่น" },
        new[] { "", "หนึ่งพัน", "สองพัน", "สามพัน", "สี่พัน", "ห้าพัน", "หกพัน", "เจ็ดพัน", "แปดพัน", "เก้าพัน" },
        new[] { "", "หนึ่งร้อย", "สองร้อย", "สามร้อย", "สี่ร้อย", "ห้าร้อย", "หกร้อย", "เจ็ดร้อย", "แปดร้อย", "เก้าร้อย" },
        new[] { "", "สิบ", "ยี่สิบ", "สามสิบ", "สี่สิบ", "ห้าสิบ", "หกสิบ", "เจ็ดสิบ", "แปดสิบ", "เก้าสิบ" },
        new[] { "ศูนย์", "เอ็ด", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" },   
    };
    private static readonly string[] _numbers = new[] {
        "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า",
        "สิบ", "สิบเอ็ด", "สิบสอง", "สิบสาม", "สิบสี่", "สิบห้า", "สิบหก", "สิบเจ็ด", "สิบแปด", "สิบเก้า",
        "ยี่สิบ", "ยี่สิบเอ็ด", "ยี่สิบสอง", "ยี่สิบสาม", "ยี่สิบสี่", "ยี่สิบห้า", "ยี่สิบหก", "ยี่สิบเจ็ด", "ยี่สิบแปด", "ยี่สิบเก้า",
        "สามสิบ", "สามสิบเอ็ด", "สามสิบสอง", "สามสิบสาม", "สามสิบสี่", "สามสิบห้า", "สามสิบหก", "สามสิบเจ็ด", "สามสิบแปด", "สามสิบเก้า",
        "สี่สิบ", "สี่สิบเอ็ด", "สี่สิบสอง", "สี่สิบสาม", "สี่สิบสี่", "สี่สิบห้า", "สี่สิบหก", "สี่สิบเจ็ด", "สี่สิบแปด", "สี่สิบเก้า",
        "ห้าสิบ", "ห้าสิบเอ็ด", "ห้าสิบสอง", "ห้าสิบสาม", "ห้าสิบสี่", "ห้าสิบห้า", "ห้าสิบหก", "ห้าสิบเจ็ด", "ห้าสิบแปด", "ห้าสิบเก้า",
        "หกสิบ", "หกสิบเอ็ด", "หกสิบสอง", "หกสิบสาม", "หกสิบสี่", "หกสิบห้า", "หกสิบหก", "หกสิบเจ็ด", "หกสิบแปด", "หกสิบเก้า",
        "เจ็ดสิบ", "เจ็ดสิบเอ็ด", "เจ็ดสิบสอง", "เจ็ดสิบสาม", "เจ็ดสิบสี่", "เจ็ดสิบห้า", "เจ็ดสิบหก", "เจ็ดสิบเจ็ด", "เจ็ดสิบแปด", "เจ็ดสิบเก้า",
        "แปดสิบ", "แปดสิบเอ็ด", "แปดสิบสอง", "แปดสิบสาม", "แปดสิบสี่", "แปดสิบห้า", "แปดสิบหก", "แปดสิบเจ็ด", "แปดสิบแปด", "แปดสิบเก้า",
        "เก้าสิบ", "เก้าสิบเอ็ด", "เก้าสิบสอง", "เก้าสิบสาม", "เก้าสิบสี่", "เก้าสิบห้า", "เก้าสิบหก", "เก้าสิบเจ็ด", "เก้าสิบแปด", "เก้าสิบเก้า",
    };
    private const string s_Ed = "เอ็ด";
    private const string s_Stang = "สตางค์";
    private const string s_Baht = "บาท";
    private const string s_Tuan = "ถ้วน";
    private const string s_BahtTuan = s_Baht + s_Tuan;
    private const string s_Negative = "ลบ";

    internal ref struct CharBuffer
    {
        public CharBuffer(Span<char> span)
        {
            _span = span;
        }

        private int _position = 0;
        private readonly Span<char> _span;

        public void Append(string s)
        {
#if NET6_0_OR_GREATER
            s.CopyTo(_span[_position..]);
#else
            s.AsSpan().CopyTo(_span[_position..]);
#endif
            _position += s.Length;
        }
        
        public ReadOnlySpan<char> GetTrimmedSpan() => _span[.._position];
        public string AsString() =>
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
        new string(GetTrimmedSpan());
#else
        new string (GetTrimmedSpan().ToArray());
#endif
    }
    private static ReadOnlySpan<int> BuildDigits(Span<int> buffer, long value)
    {
        Debug.Assert(value >= 0);

        int i = buffer.Length;
        do
        {
            buffer[--i] = ((int)(value % 10));
            value /= 10;
        } while (value > 0);

        return buffer[i..];
    }

    private static ReadOnlySpan<int> BuildDigits(Span<int> buffer, decimal value)
    {
        if (long.MinValue < value && value < long.MaxValue)
        {
            return BuildDigits(buffer, (long)value);
        }
        else
        {
            const decimal divisor = 1_000_000_000_000_000_000;
            Debug.Assert(divisor <= value);
            long big = (long)(value / divisor);
            Debug.Assert(big > 0);
            long small = (long)(value % divisor);
            var smallDigits = BuildDigits(buffer, small);
            var bigDigits = BuildDigits(buffer[..^18], big);
            return buffer[^(bigDigits.Length + 18)..];
        }
    }
    
    private static void FormatInternal(scoped ref CharBuffer buffer, long value)
    {
        Debug.Assert(value < 1_000_000);
        if (value == 1)
        {
            buffer.Append("หนึ่ง");
            return;    
        }
        Span<int> digits = stackalloc int[6];
        digits.Clear();
        BuildDigits(digits, value);
        //var digits = BuildDigits(stackalloc int[6], value);
        Debug.Assert(digits.Length > 0);
        Debug.Assert(digits.Length <= 6);
        var grid = _numberGrid;

        for (int i = 0; i < 6; i++)
        {
            var n = digits[i];
            if (n != 0)
            {
                // digit
                buffer.Append(grid[i][n]);
            }
        }
    }


    private static void FormatInternal(scoped ref CharBuffer buffer, scoped ReadOnlySpan<int> digits)
    {
        Debug.Assert(digits.Length > 0);
        Debug.Assert(digits.Length <= 6);

        int loopDigitLen = digits.Length - 1;
        var grid = _numberGrid;

        for (int i = 0, scale = 6 - digits.Length; i < loopDigitLen; i++, scale++)
        {
            var n = digits[i];

            if (n != 0)
            {
                // digit
                buffer.Append(grid[scale][n]);
            }
        }

        // หลักหน่วย
        {
            var n = digits[^1];
            if (n != 0)
            {
                buffer.Append(n == 1 && digits.Length > 1 ? s_Ed : _numbers[n]);
            }
        }
    }

    private static void Format(scoped ref CharBuffer buffer, scoped ReadOnlySpan<int> digits, bool isNegative)
    {
        // เติมเครื่องหมายลบถ้าเป็นลบ
        if (isNegative)
        {
            buffer.Append(s_Negative);
        }

        // ถ้าเป็นเลขหลักเดียว
        if (digits.Length == 1)
        {
            var word = _numbers[digits[0]];
            buffer.Append(word);
        }
        else
        {
            var remains = digits.Length % 6;
            if (remains == 0) remains = 6;
            var start = 0;
            while (start < digits.Length)
            {
                FormatInternal(ref buffer, digits.Slice(start, remains));
                start += remains;
                remains = 6;
                if (start < digits.Length)
                {
                    buffer.Append("ล้าน");
                }
            }
        }
    }

    public string Format(long value)
    {
        bool isNegative = value < 0;
        if (isNegative) value = -value;
        if (value < 100) return isNegative ? "ลบ" + _numbers[value] : _numbers[value];

        var buffer = new CharBuffer(stackalloc char[180]);
        long b1, b2, b3;
        const long mil = 1_000_000;
        b3 = value % mil;
        b2 = (value / mil) % mil;
        b1 = value / (mil * mil);

        if (b1 > 0)
        {
            FormatInternal(ref buffer, b1);
            buffer.Append("ล้าน");
        }
        
        if (b2 > 0) {
            FormatInternal(ref buffer, b2);
            buffer.Append("ล้าน");
        }
        
        if (b3 == 1) buffer.Append(s_Ed);
        else FormatInternal(ref buffer, b3);
        
        return buffer.AsString();
    }

    public string GetBahtText(decimal value)
    {
        bool isNegative = value < 0;
        if (isNegative) value = -value;

        decimal longValue = decimal.Truncate(value);

        // ถ้าไม่มีทศนิยม และน้อยกว่า 100
        if (longValue == value && value < 100)
        {
            return (isNegative ? s_Negative : "") + _numbers[(int)value] + s_BahtTuan;
        }

        decimal dec = value - longValue;
        int satang = (int)(dec * 100);

        // integer part        
        var digits = BuildDigits(stackalloc int[36], longValue);
        // maximum digits is 29 and longest word per digit is 10 (หนึ่งหมื่น)
        // plus satang part then it should be less than 320
        // so we skip the calculation and use 320 instead because stackalloc is O(1)        
        var buffer = new CharBuffer(stackalloc char[320]);
        //var buffer = new CharBuffer(stackalloc char[digits.Length * 10 + 2 + (satang > 0 ? (5 + 3 + 5 + 6) : 3)]);
        if (longValue > 0)
        {
            Format(ref buffer, digits, isNegative);
        }

        // เพิ่มบาทถ้วน หรือบาท xx สตางค์
        if (satang == 0)
        {
            buffer.Append(s_BahtTuan);
        }
        else
        {
            if (longValue > 0)
            {
                buffer.Append(s_Baht);
            }

            buffer.Append(_numbers[satang]);
            buffer.Append(s_Stang);
        }
        return buffer.AsString();
    }
}

