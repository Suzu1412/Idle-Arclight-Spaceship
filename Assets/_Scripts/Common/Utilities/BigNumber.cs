using System;
using System.Collections.Generic;
using Cysharp.Text;
using UnityEngine; // ZString requires this namespace

[Serializable]
public struct BigNumber : IComparable<BigNumber>
{
    private const double Log10Base = 10.0;
    private const double Log1000Base = 1000.0;
    private const double MinMantissa = 0.0001; // Prevents unnecessary floating precision errors
    private static readonly List<string> Units = new()
    { "", "K", "M", "B", "T" }; // Extend as needed
                                // "Qa", "Qi", "Sx", "Sp", "Oc", "No", "Dc"

    public static readonly BigNumber Zero = new BigNumber(0, 0);
    public static readonly BigNumber One = new BigNumber(1, 0);

    public double mantissa;
    public int exponent;

    public BigNumber(double value)
    {
        if (value == 0)
        {
            mantissa = 0;
            exponent = 0;
            return;
        }

        exponent = (int)Math.Floor(Math.Log10(Math.Abs(value)));
        mantissa = value / Math.Pow(Log10Base, exponent);

        Normalize();
    }

    public BigNumber(double mantissa, int exponent)
    {
        this.mantissa = mantissa;
        this.exponent = exponent;
        Normalize();
    }
    // Example of Normalize method for context
    private void Normalize()
    {
        if (mantissa == 0)
        {
            exponent = 0;
            return;
        }

        const double Log10Base = 10.0;
        const double epsilon = 1e-14;

        while (Math.Abs(mantissa) >= Log10Base)
        {
            mantissa /= Log10Base;
            exponent++;
        }

        while (Math.Abs(mantissa) < 1 && Math.Abs(mantissa) > epsilon)
        {
            mantissa *= Log10Base;
            exponent--;
        }

        if (Math.Abs(mantissa) < epsilon)
        {
            mantissa = 0;
            exponent = 0;
        }
    }

    // Basic Operations
    public static BigNumber operator +(BigNumber a, BigNumber b)
    {
        if (a.exponent > b.exponent) return new BigNumber(a.mantissa + (b.mantissa / Math.Pow(10, a.exponent - b.exponent)), a.exponent);
        if (b.exponent > a.exponent) return new BigNumber(b.mantissa + (a.mantissa / Math.Pow(10, b.exponent - a.exponent)), b.exponent);
        return new BigNumber(a.mantissa + b.mantissa, a.exponent);
    }

    public static BigNumber operator -(BigNumber a, BigNumber b)
    {
        if (a.exponent > b.exponent) return new BigNumber(a.mantissa - (b.mantissa / Math.Pow(10, a.exponent - b.exponent)), a.exponent);
        if (b.exponent > a.exponent) return new BigNumber(-b.mantissa + (a.mantissa / Math.Pow(10, b.exponent - a.exponent)), b.exponent);
        return new BigNumber(a.mantissa - b.mantissa, a.exponent);
    }

    public static BigNumber operator *(BigNumber a, BigNumber b)
    {
        return new BigNumber(a.mantissa * b.mantissa, a.exponent + b.exponent);
    }

    public static BigNumber operator /(BigNumber a, BigNumber b)
    {
        return new BigNumber(a.mantissa / b.mantissa, a.exponent - b.exponent);
    }

    public static BigNumber Max(BigNumber a, BigNumber b)
    {
        return (a > b) ? a : b;
    }

    public static BigNumber Pow(double baseValue, int exponent)
    {
        if (exponent == 0) return BigNumber.One;
        if (baseValue == 1) return BigNumber.One;
        if (baseValue == 0) return BigNumber.Zero;

        double newMantissa = Math.Pow(baseValue, exponent);
        int newExponent = 0;

        if (newMantissa >= 10)
        {
            while (newMantissa >= 10)
            {
                newMantissa /= 10;
                newExponent++;
            }
        }
        else if (newMantissa < 1 && newMantissa > 0)
        {
            while (newMantissa < 1)
            {
                newMantissa *= 10;
                newExponent--;
            }
        }

        return new BigNumber(newMantissa, newExponent * exponent);

    }

    // Pow method to compute BigNumber^power
    public static BigNumber Pow(BigNumber baseValue, double power)
    {
        if (baseValue.mantissa == 0)
            return new BigNumber(0);

        if (power == 0)
            return new BigNumber(1);  // Any number to the power of 0 is 1

        // Use logarithmic identity: a^b = 10^(log10(a) * b)
        double logBase = Math.Log10(baseValue.mantissa) + baseValue.exponent;  // log10(mantissa * 10^exponent)
        double resultLog = logBase * power;  // log10(a) * b

        // Convert log result back to BigNumber form
        double resultExponent = Math.Floor(resultLog);
        double resultMantissa = Math.Pow(10, resultLog - resultExponent);

        return new BigNumber(resultMantissa, (int)resultExponent);
    }


    // Define Infinity for BigNumber
    public static BigNumber Infinity => new BigNumber(double.PositiveInfinity);

    public static double Log(BigNumber value, double baseValue = Math.E)
    {
        if (value.mantissa <= 0) return double.NegativeInfinity; // Log of zero or negative is undefined

        return Math.Log(value.mantissa, baseValue) + (value.exponent * Math.Log(10, baseValue));
    }

    // Log method to compute log base 10 of a BigNumber
    public static BigNumber Log(BigNumber value)
    {
        if (value.mantissa <= 0)
            throw new ArgumentException("Logarithm is only defined for positive numbers.");

        // Log10(value) = log10(mantissa * 10^exponent) = log10(mantissa) + exponent
        double logMantissa = Math.Log10(value.mantissa);
        double logResult = logMantissa + value.exponent;

        // Return as BigNumber (since it's a plain double, exponent is 0)
        return new BigNumber(logResult);
    }


    public static BigNumber Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new BigNumber(0);

        // Handle scientific notation (e.g., "1.23e6")
        if (input.Contains("e") || input.Contains("E"))
        {
            if (double.TryParse(input, System.Globalization.NumberStyles.Float,
                                System.Globalization.CultureInfo.InvariantCulture, out double result))
            {
                return new BigNumber(result);
            }
        }

        // Handle plain numbers (e.g., "1234567")
        if (double.TryParse(input, out double numericValue))
        {
            return new BigNumber(numericValue);
        }

        throw new FormatException($"Invalid BigNumber format: {input}");
    }

    public static BigNumber SmoothDamp(BigNumber current, BigNumber target, ref float mantissaVelocity, ref float exponentVelocity, double smoothTime, double deltaTime = -1)
    {
        if (deltaTime < 0) deltaTime = Time.unscaledDeltaTime;

        // If exponent difference is too large, snap to target immediately
        if (Math.Abs(current.exponent - target.exponent) > 6)
            return target;

        // Smooth the mantissa separately
        double newMantissa = Mathf.SmoothDamp((float)current.mantissa, (float)target.mantissa, ref mantissaVelocity, (float)smoothTime * 0.5f, Mathf.Infinity, (float)deltaTime);

        // If the mantissa is too small or too large, adjust the exponent smoothly
        int newExponent = current.exponent;

        if (Math.Abs(newMantissa) >= 10)
        {
            newMantissa /= 10;
            newExponent++;
        }
        else if (Math.Abs(newMantissa) < 1 && newMantissa != 0)
        {
            newMantissa *= 10;
            newExponent--;
        }

        // Smooth the exponent change, but let it be quicker than the mantissa
        float smoothedExponent = Mathf.SmoothDamp(current.exponent, target.exponent, ref exponentVelocity, (float)smoothTime * 0.3f, Mathf.Infinity, (float)deltaTime);
        newExponent = Mathf.RoundToInt(smoothedExponent);

        return new BigNumber(newMantissa, newExponent);
    }

    // Helper method to reconstruct BigNumber from a double value
    public static BigNumber FromDouble(double value)
    {
        if (value == 0) return BigNumber.Zero;

        int exponent = (int)Math.Floor(Math.Log10(Math.Abs(value)));
        double mantissa = value / Math.Pow(10, exponent);

        return new BigNumber(mantissa, exponent);
    }

    // BigNumber with Double Operations
    public static BigNumber operator *(BigNumber a, double b)
    {
        return new BigNumber(a.mantissa * b, a.exponent);
    }

    public static BigNumber operator *(double b, BigNumber a) => a * b;

    public static BigNumber operator /(BigNumber a, double b)
    {
        return new BigNumber(a.mantissa / b, a.exponent);
    }

    public static BigNumber operator +(BigNumber a, double b)
    {
        return new BigNumber(a.mantissa + (b / Math.Pow(10, a.exponent)), a.exponent);
    }

    public static BigNumber operator -(BigNumber a, double b)
    {
        return new BigNumber(a.mantissa - (b / Math.Pow(10, a.exponent)), a.exponent);
    }

    // BigNumber with Float Operations
    public static BigNumber operator *(BigNumber a, float b)
    {
        return new BigNumber(a.mantissa * b, a.exponent);
    }

    public static BigNumber operator *(float b, BigNumber a) => a * b;

    public BigNumber Ceil()
    {
        return new BigNumber(Math.Ceiling(mantissa), exponent);
    }

    // Lerp method
    public static BigNumber Lerp(BigNumber start, BigNumber end, float t)
    {
        // Interpolate mantissa
        double lerpedMantissa = Mathf.Lerp((float)start.mantissa, (float)end.mantissa, t);

        // Interpolate exponent
        int lerpedExponent = Mathf.RoundToInt(Mathf.Lerp(start.exponent, end.exponent, t));

        // Return new BigNumber based on lerped values
        return new BigNumber(lerpedMantissa, lerpedExponent);
    }

    // Convert to double (for UI display)
    public double ToDouble()
    {
        return mantissa * Math.Pow(10, exponent);
    }

    public int ToInt()
    {
        return (int)ToDouble(); // Convert to double first, then cast to int
    }

    public float ToFloat()
    {
        return (float)ToDouble();
    }

    // Comparisons
    public int CompareTo(BigNumber other)
    {
        if (exponent > other.exponent) return 1;
        if (exponent < other.exponent) return -1;
        return mantissa.CompareTo(other.mantissa);
    }

    public static bool operator >(BigNumber a, BigNumber b) => a.CompareTo(b) > 0;
    public static bool operator <(BigNumber a, BigNumber b) => a.CompareTo(b) < 0;
    public static bool operator >=(BigNumber a, BigNumber b) => a.CompareTo(b) >= 0;
    public static bool operator <=(BigNumber a, BigNumber b) => a.CompareTo(b) <= 0;
    public static bool operator ==(BigNumber a, BigNumber b) => a.CompareTo(b) == 0;
    public static bool operator !=(BigNumber a, BigNumber b) => a.CompareTo(b) != 0;

    public override bool Equals(object obj) => obj is BigNumber bn && this == bn;
    public override int GetHashCode() => mantissa.GetHashCode() ^ exponent.GetHashCode();

    // **ZString Optimized AA Notation Formatter**
    public readonly string GetFormat()
    {
        int magnitude = exponent / 3;
        if (magnitude < 0) return ZString.Format("{0}", "0");
        if (magnitude < Units.Count)
        {
            return ZString.Format("{0:F2} {1}", mantissa * Math.Pow(10, exponent % 3), Units[magnitude]);
        }

        // Handle custom AA notation beyond the predefined units (e.g., AA, AB, AC)
        int unitInt = magnitude - Units.Count;
        int secondUnit = unitInt % 26;
        int firstUnit = unitInt / 26;
        char firstChar = (char)('A' + firstUnit);
        char secondChar = (char)('A' + secondUnit);
        return ZString.Format("{0:F2} {1}{2}", mantissa * Math.Pow(10, exponent % 3), firstChar, secondChar);
    }

    public override string ToString() => GetFormat();
}