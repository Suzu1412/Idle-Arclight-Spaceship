using System.Collections.Generic;
using System;
using System.Diagnostics;

// https://oguzkonya.com/formatting-big-numbers-aa-notation/
public static class FormatNumber
{
    private static readonly int charA = Convert.ToInt32('a');

    private static Dictionary<int, string> _units = new()
    {
        { 0, "" },
        { 1, "K" },
        { 2, "M" },
        { 3, "B" },
        { 4, "T" }
    };

    /// <summary>
    /// Format Double
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static FormattedNumber FormatDouble(double value)
    {
        if (value <= 0d)
        {
            return new FormattedNumber(value, "");
        }

        int magnitude = (int)Math.Log(value, 1000);
        double finalValue = value / Math.Pow(1000, magnitude);
        string unit;
        if (magnitude < _units.Count)
        {
            unit = _units[magnitude];
        }
        else
        {
            int unitInt = magnitude - _units.Count;
            int secondUnit = unitInt % 26;
            int firstUnit = unitInt / 26;
            unit = Convert.ToChar(firstUnit + charA).ToString() + Convert.ToChar(secondUnit + charA).ToString();
        }
        // Math.Floor(m * 100) / 100) fixes rounding errors

        return new FormattedNumber(finalValue, unit);
        //return (Math.Floor(finalValue * 100) / 100).ToString("0.##") + unit;
    }
}

public struct FormattedNumber
{
    public double Value;
    public string Unit;

    public FormattedNumber(double value, string unit)
    {
        Value = Math.Floor(value * 100) / 100;
        Unit = unit;
    }

    public readonly string GetFormat()
    {
        return Value.ToString("0.##") + Unit;
    }
}
