using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Distractive.Formatters;


#if NET6_0_OR_GREATER
[SkipLocalsInit]
#endif
public sealed class ThaiNumberTextFormatter
{
    private static readonly string[][] _numberGrid = new[] {
        new[] { "", "หนึ่งแสน", "สองแสน", "สามแสน", "สี่แสน", "ห้าแสน", "หกแสน", "เจ็ดแสน", "แปดแสน", "เก้าแสน" },
        new[] { "", "หนึ่งหมื่น", "สองหมื่น", "สามหมื่น", "สี่หมื่น", "ห้าหมื่น", "หกหมื่น", "เจ็ดหมื่น", "แปดหมื่น", "เก้าหมื่น" },
        new[] { "", "หนึ่งพัน", "สองพัน", "สามพัน", "สี่พัน", "ห้าพัน", "หกพัน", "เจ็ดพัน", "แปดพัน", "เก้าพัน" },
        new[] { "", "หนึ่งร้อย", "สองร้อย", "สามร้อย", "สี่ร้อย", "ห้าร้อย", "หกร้อย", "เจ็ดร้อย", "แปดร้อย", "เก้าร้อย" },
    };
    private static readonly string[] _w100k = _numberGrid[0];
    private static readonly string[] _w10k = _numberGrid[1];
    private static readonly string[] _w1k = _numberGrid[2];
    private static readonly string[] _w100 = _numberGrid[3];
    private static readonly string[] _numbers = new[] {
        "", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า",
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
    private const decimal md = 1_000_000M;

    private readonly string _et;
    public ThaiNumberTextFormatter(ThaiNumberTextFormatterOptions options)
    {
        Options = options;
        _et = options.FormatFlags.HasFlag(ThaiNumberTextFormatFlags.EtWithTensOnly)
            ? "หนึ่ง" : s_Ed;
    }

    public ThaiNumberTextFormatter() : this(ThaiNumberTextFormatterOptions.Default) { }

    public ThaiNumberTextFormatterOptions Options { get; }

    internal ref struct CharBuffer
    {
        public CharBuffer(Span<char> span)
        {
            _span = span;
        }

        private int _position = 0;
        private readonly Span<char> _span;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        new(GetTrimmedSpan());
#else
        new(GetTrimmedSpan().ToArray());
#endif
    }




    private void FormatInternal(scoped ref CharBuffer buffer, int value)
    {
        Debug.Assert(value > 0);
        Debug.Assert(value < 1_000_000);

        if (value == 0) return;
        if (value < 100)
        {
            buffer.Append(_numbers[value]);
            return;
        }

        var a = _w100k[value / 100_000 % 10];
        var b = _w10k[value / 10_000 % 10];
        var c = _w1k[value / 1_000 % 10];
        var d = _w100[value / 100 % 10];
        var p = value % 100;
        var s = p == 1 ? _et : _numbers[p];
        buffer.Append(a);
        buffer.Append(b);
        buffer.Append(c);
        buffer.Append(d);        
        buffer.Append(s);
    }

    private void Format(scoped ref CharBuffer buffer, long value, bool fillMil = false)
    {
        bool isNegative = value < 0;
        if (isNegative) value = -value;
        if (isNegative) buffer.Append(s_Negative);

        if (value < 100 && !fillMil)
        {
            string s = value == 0 ? "ศูนย์" : _numbers[(int)value];
            buffer.Append(s);
            return;
        }

        const long mil = 1_000_000;
        long b3 = value;
        long b2 = value / mil;
        long b1 = value / (mil * mil);
        long b0 = value / (mil * mil * mil);
        Debug.Assert(b0 < 100);

        if (b0 > 0)
        {
            buffer.Append(_numbers[b0]);
            buffer.Append("ล้าน");
        }

        if (b1 > 0 || fillMil)
        {
            FormatInternal(ref buffer, (int)(b1 % mil));
            buffer.Append("ล้าน");
        }

        if (b2 > 0 || fillMil)
        {
            FormatInternal(ref buffer, (int)(b2 % mil));
            buffer.Append("ล้าน");
        }

        if (b3 > 0)
        {
            var val = (int)(b3 % mil);
            if (val == 1) buffer.Append(_et);
            else FormatInternal(ref buffer, val);
        }
    }

    public string Format(long value)
    {
        var buffer = new CharBuffer(stackalloc char[180]);
        Format(ref buffer, value);
        return buffer.AsString();
    }

    public string GetBahtText(decimal value)
    {
        if (value == 0) return "ศูนย์บาทถ้วน";
        Debug.Assert(value != 0);

        // maximum digits is 29 and longest word per digit is 10 (หนึ่งหมื่น)
        // plus satang part then it should be less than 320
        // so we skip the calculation and use 320 instead because stackalloc is O(1)        
        var buffer = new CharBuffer(stackalloc char[320]);

        if (value < 0)
        {
            value = -value;
            buffer.Append(s_Negative);
        }

        decimal longValue = decimal.Truncate(value);        

        if (longValue != 0)
        {            
            if (longValue < long.MaxValue)
            {
                Format(ref buffer, (long)longValue);
            }
            else
            {
                const long divisor = 1_000_000_000_000_000_000;
                Debug.Assert(divisor <= value);
                long big = (long)(value / divisor);
                Debug.Assert(big != 0);
                long small = (long)(value % divisor);                
                Format(ref buffer, big);
                buffer.Append("ล้าน");
                Format(ref buffer, small, true);
            }
        }

        decimal dec = value - longValue;
        int satang = (int)(dec * 100);

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

public sealed record ThaiNumberTextFormatterOptions(ThaiNumberTextFormatFlags FormatFlags = ThaiNumberTextFormatFlags.Default)
{
    public static ThaiNumberTextFormatterOptions Default { get; } = new();
    public static ThaiNumberTextFormatterOptions EtWithTensOnly { get; } = new(ThaiNumberTextFormatFlags.EtWithTensOnly);
}

[Flags]
public enum ThaiNumberTextFormatFlags
{
    /// <summary>
    /// ค่าตั้งต้น จะใช้เอ็ดเสมอ (ร้อยเอ็ด พันเอ็ด ล้านเอ็ด ร้อยเอ็ดล้าน ฯลฯ)
    /// ซึ่งเป็นรูปแบบที่ราชบัณฑิตยสภาแนะนำ
    /// </summary>
    Default = 0,

    /// <summary>
    /// ใช้เอ็ดกับหลักสิบเท่านั้น (ยี่สิบเอ็ด-เก้าสิบเอ็ด) ที่เหลือจะเป็นหนึ่ง เช่น หนึ่งร้อยหนึ่ง
    /// </summary>
    EtWithTensOnly = 1,

    [EditorBrowsable(EditorBrowsableState.Never)]
    TotalPerm = EtWithTensOnly * 2,
}

