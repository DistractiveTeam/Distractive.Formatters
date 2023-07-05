# Distractive.Formatters

Various string formatters, mainly for Thai language

## Description

Convert number to Thai words. Thai words and Baht text format supported.

## Getting Started

### Dependencies

.NET 6 or .NET Standard 2.0 or 2.1

### Installing

* `nuget install Distractive.Formatters`

### Usage

```
var formatter = new ThaiNumberTextFormatter();
formatter.Format(1001); // หนึ่งพันเอ็ด
formatter.GetBahtText(9.99M); // เก้าบาทเก้าสิบเก้าสตางค์
```

All public methods of ThaiNumberTextFormatter are guaranteed to be thread-safe, so using it as a static instance is safe.

## Authors

[Tia](https://github.com/tiakun)

## Version History

* 1.0.0
    * Initial Release

## License

This project is licensed under the [MIT] License - see the LICENSE.md file for details

## Acknowledgments

Inspired by
* [ThaiBahtText](https://github.com/greatfriends/ThaiBahtText)
* [NumberToThaiText](https://github.com/natthakhon/NumberToThaiText)
