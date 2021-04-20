using System;

/// <summary>
/// Serializable Custom Vector2 struct.
/// </summary>
[Serializable]
public struct CustomVector2
{
    public int x;
    public int y;

    /// <summary>
    /// Constructor for CustomVector2.
    /// </summary>
    /// <param name="x">Parameter X</param>
    /// <param name="y">Parameter Y</param>
    /// <param name="z">Parameter Z></param>
    public CustomVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Result operation from '+' operator.
    /// </summary>
    /// <param name="c1">Vector num1.</param>
    /// <param name="c2">Vector num2.</param>
    /// <returns>Returns a vector after the operation.</returns>
    public static CustomVector2 operator +(CustomVector2 c1,
                                            CustomVector2 c2)
    {
        CustomVector2 c3 = new CustomVector2(c1.x + c2.x, c1.y + c2.y);
        return c3;
    }

    /// <summary>
    /// Result operation from '-' operator.
    /// </summary>
    /// <param name="c1">Vector num1.</param>
    /// <param name="c2">Vector num2.</param>
    /// <returns>Returns a vector after the operation.</returns>
    public static CustomVector2 operator -(CustomVector2 c1,
                                            CustomVector2 c2)
    {
        CustomVector2 c3 = new CustomVector2(c1.x - c2.x, c1.y - c2.y);
        return c3;
    }

    /// <summary>
    /// Result operation from '++' operator.
    /// </summary>
    /// <param name="c1">Vector num1.</param>
    /// <returns>Returns a vector after the operation.</returns>
    public static CustomVector2 operator ++(CustomVector2 c1)
    {
        return new CustomVector2(c1.x + 1, c1.y + 1);
    }

    /// <summary>
    /// Result operation from '--' operator.
    /// </summary>
    /// <param name="c1">Vector num1.</param>
    /// <returns>Returns a vector after the operation.</returns>
    public static CustomVector2 operator --(CustomVector2 c1)
    {
        return new CustomVector2(c1.x - 1, c1.y - 1);
    }

    /// <summary>
    /// Result operation from '==' operator.
    /// </summary>
    /// <param name="c1">Vector num1.</param>
    /// <param name="c2">Vector num2.</param>
    /// <returns>Returns true if both vectors are equal.</returns>
    public static bool operator ==(CustomVector2 c1,
                                    CustomVector2 c2) => c1.Equals(c2);

    /// <summary>
    /// Result operation from '!=' operator.
    /// </summary>
    /// <param name="c1">Vector num1.</param>
    /// <param name="c2">Vector num2.</param>
    /// <returns>Returns true if both vectors are different.</returns>
    public static bool operator !=(CustomVector2 c1,
                                    CustomVector2 c2) => !c1.Equals(c2);

    /// <summary>
    /// Overrides GetHashCode.
    /// </summary>
    /// <returns>Returns hashcode for the current object.</returns>
    public override int GetHashCode()
    {
        return x.GetHashCode() ^ y.GetHashCode();
    }

    /// <summary>
    /// Overrides equals to compare CustomVector2 variables.
    /// </summary>
    /// <param name="obj">Object co compare.</param>
    /// <returns>Returns true if both objects are equal.</returns>
    public override bool Equals(object obj)
    {
        CustomVector2 other = (CustomVector2)obj;
        return x == other.x && y == other.y;
    }

    /// <summary>
    /// Overrides ToString.
    /// </summary>
    /// <returns>Returns a string with CustomVector2 x, y and z.</returns>
    public override string ToString() => $"[{x}, {y}]";
}
