# String Math Calculator

String Calculator is able to solve math calculations within a string, returning a double as the product. Supports the basic operators, *, /, + and -. Also supports parentheses, roots, exponents and logarithms. The operations are calculated left to right by PEMDAS order.

## Getting Started

WIP

## Examples

A basic forumla, operations are performed left to right, in PEMDAS order.
```
Calculator calculator = new Calculator();

//Calculate returns a double
Console.WriteLine(calculator.Calculation("3 + 8 - 9 / 7 * 6").Calculate());
```

Square root of 4. Any root can be used on the right side of r.
```
Calculator calculator = new Calculator();

//Calculate returns a double
Console.WriteLine(calculator.Calculation("4 r 2").Calculate());
```

Exponent, 4 by the power of two.
```
Calculator calculator = new Calculator();

//Calculate returns a double
Console.WriteLine(calculator.Calculation("4 ^ 2").Calculate());
```

Log, 4 by the log of 2.
```
Calculator calculator = new Calculator();

//Calculate returns a double
Console.WriteLine(calculator.Calculation("4 ^ 2").Calculate());
```

Parentheses can be used
```
Calculator calculator = new Calculator();

//Calculate returns a double
Console.WriteLine(calculator.Calculation("(3 + 4) * 2").Calculate());
```
