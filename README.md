# String Math Calculator

String Math Calculator is able to solve math calculations within a string, returning a double as the product. Supports the basic operators, *, /, + and -. Also supports parentheses, roots, exponents and logarithms. The operations are calculated left to right by PEMDAS order.

## Getting Started

### Prerequisites

A .net framework that conforms to .net standard 2.0

https://dotnet.microsoft.com/

Windows -
.net framework or .net core

Windows, Linux, Mac -
.net core

### Installation

String Math Calculator is available to install as a Nuget package:

https://www.nuget.org/packages/StringMathCalculator/

Nuget installation instructions:

https://docs.microsoft.com/en-us/nuget/consume-packages/ways-to-install-a-package

## Examples

A basic forumla, operations are performed left to right, in PEMDAS order.
```
using StringMathCalculator;

Calculator calculator = new Calculator();

//Calculate returns a double
Console.WriteLine(calculator.Calculation("3 + 8 - 9 / 7 * 6").Calculate());
```

Square root of 4. Any root can be used on the right side of r.
```
Console.WriteLine(calculator.Calculation("4 r 2").Calculate());
```

Exponent, 4 by the power of two.
```
Console.WriteLine(calculator.Calculation("4 ^ 2").Calculate());
```

Log, 4 by the log of 2.
```
Console.WriteLine(calculator.Calculation("4 log 2").Calculate());
```

Parentheses can be used
```
Console.WriteLine(calculator.Calculation("(3 + 4) * 2").Calculate());
```

The times operator can be omitted if a number is next to a parentheses 
```
Console.WriteLine(calculator.Calculation("(3 + 4) 2").Calculate());
Console.WriteLine(calculator.Calculation("2 (3 + 4)").Calculate());
```
