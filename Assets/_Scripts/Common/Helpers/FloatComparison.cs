using UnityEngine;

/**
 * Class for comparing floating point values 
 */
[HelpURL("https://coffeebraingames.wordpress.com/2013/12/18/a-generic-floating-point-comparison-class/")]
public static class FloatComparison
{

    /**
     * Returns whether or not a == b
     */
    public static bool TolerantEquals(float a, float b)
    {
        return Mathf.Approximately(a, b);
    }

    /**
     * Returns whether or not a >= b
     */
    public static bool TolerantGreaterThanOrEquals(float a, float b)
    {
        return a > b || TolerantEquals(a, b);
    }

    /**
     * Returns whether or not a > b
     */
    public static bool TolerantGreaterThan(float a, float b)
    {
        return a > b;
    }

    /**
     * Returns whether or not a <= b
     */
    public static bool TolerantLesserThanOrEquals(float a, float b)
    {
        return a < b || TolerantEquals(a, b);
    }

    /**
     * Returns whether or not a < b
     */
    public static bool TolerantLesserThan(float a, float b)
    {
        return a < b;
    }

}