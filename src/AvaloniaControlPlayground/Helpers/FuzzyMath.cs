using System;

namespace AvaloniaControlPlayground.Helpers;

public static class FuzzyMath
{
    private const double DoubleEpsilon = 2.2204460492503131e-016;

    public static bool IsPositive(double value) => GreaterThan(value, 0D);

    public static bool IsNegative(double value) => LessThan(value, double.NegativeZero);

    public static bool CloseToZero(double value) => !IsPositive(value) && !IsNegative(value);

    public static bool GreaterThan(double value1, double value2) =>
        value1 > value2 && !AreClose(value1, value2);

    public static bool LessThan(double value1, double value2) =>
        value1 < value2 && !AreClose(value1, value2);

    public static bool AreClose(double value1, double value2)
    {
        //in case they are Infinities (then epsilon check does not work)
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (value1 == value2) return true;
        double eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * DoubleEpsilon;
        double delta = value1 - value2;
        return -eps < delta && eps > delta;
    }
}
