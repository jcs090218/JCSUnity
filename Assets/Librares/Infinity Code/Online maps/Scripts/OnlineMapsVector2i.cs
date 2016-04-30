/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

/// <summary>
/// Integer version of Vector2 class.
/// </summary>
[System.Serializable]
public class OnlineMapsVector2i
{
    /// <summary>
    /// The x value.
    /// </summary>
    public int x;

    /// <summary>
    /// The y value.
    /// </summary>
    public int y;

    /// <summary>
    /// Gets the OnlineMapsVector2i where x=0 and y=0.
    /// </summary>
    /// <value>
    /// The zero OnlineMapsVector2i.
    /// </value>
    public static OnlineMapsVector2i zero
    {
        get
        {
            return new OnlineMapsVector2i(0, 0);
        }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="X">
    /// The x coordinate.
    /// </param>
    /// <param name="Y">
    /// The y coordinate.
    /// </param>
    public OnlineMapsVector2i(int X = 0, int Y = 0)
    {
        x = X;
        y = Y;
    }

    /// <summary>
    /// Vector2 casting operator.
    /// </summary>
    /// <param name="val">
    /// The value.
    /// </param>
    public static implicit operator Vector2(OnlineMapsVector2i val)
    {
        return new Vector2(val.x, val.y);
    }

    /// <summary>
    /// Vector2 to OnlineMapsVector2i casting operator.
    /// </summary>
    /// <param name="vector">
    /// The vector.
    /// </param>
    public static implicit operator OnlineMapsVector2i(Vector2 vector)
    {
        return new OnlineMapsVector2i((int)vector.x, (int)vector.y);
    }

    /// <summary>
    /// Converts OnlineMapsVector2i to string.
    /// </summary>
    /// <returns>String</returns>
    public override string ToString()
    {
        return string.Format("X: {0}, Y: {1}", x, y);
    }

    /// <summary>
    /// Subtraction operator.
    /// </summary>
    /// <param name="v1">
    /// The first OnlineMapsVector2i.
    /// </param>
    /// <param name="v2">
    /// The second OnlineMapsVector2i.
    /// </param>
    /// <returns>
    /// The result of the operation.
    /// </returns>
    public static OnlineMapsVector2i operator -(OnlineMapsVector2i v1, OnlineMapsVector2i v2)
    {
        return new OnlineMapsVector2i(v1.x - v2.x, v1.y - v2.y);
    }

    /// <summary>
    /// Subtraction operator.
    /// </summary>
    /// <param name="v1">
    /// The first Vector2.
    /// </param>
    /// <param name="v2">
    /// The second OnlineMapsVector2i.
    /// </param>
    /// <returns>
    /// The result of the operation.
    /// </returns>
    public static Vector2 operator -(Vector2 v1, OnlineMapsVector2i v2)
    {
        return new Vector2(v1.x - v2.x, v1.y - v2.y);
    }

    /// <summary>
    /// Addition operator.
    /// </summary>
    /// <param name="v1">
    /// The first OnlineMapsVector2i.
    /// </param>
    /// <param name="v2">
    /// The second OnlineMapsVector2i.
    /// </param>
    /// <returns>
    /// The result of the operation.
    /// </returns>
    public static OnlineMapsVector2i operator +(OnlineMapsVector2i v1, OnlineMapsVector2i v2)
    {
        return new OnlineMapsVector2i(v1.x + v2.x, v1.y + v2.y);
    }

    /// <summary>
    /// Equality operator.
    /// </summary>
    /// <param name="v1">The first OnlineMapsVector2i.</param>
    /// <param name="v2">The second OnlineMapsVector2i.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator ==(OnlineMapsVector2i v1, OnlineMapsVector2i v2)
    {
        if ((object) v1 == (object) v2) return true;
        if ((object) v1 == null || (object) v2 == null) return false;
        return v1.x == v2.x && v1.y == v2.y;
    }

    /// <summary>
    /// Inequality operator.
    /// </summary>
    /// <param name="v1">The first OnlineMapsVector2i.</param>
    /// <param name="v2">The second OnlineMapsVector2i.</param>
    /// <returns>The result of the operation.</returns>
    public static bool operator !=(OnlineMapsVector2i v1, OnlineMapsVector2i v2)
    {
        return !(v1 == v2);
    }

    public override bool Equals(object obj)
    {
        OnlineMapsVector2i v = obj as OnlineMapsVector2i;
        if (v == null) return false;
        return this == v;
    }

    public override int GetHashCode()
    {
        return x ^ y;
    }
}