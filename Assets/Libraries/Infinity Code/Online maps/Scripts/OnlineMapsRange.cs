/*     INFINITY CODE 2013-2016      */
/*   http://www.infinity-code.com   */

using UnityEngine;

/// <summary>
/// Class of range.
/// </summary>
[System.Serializable]
public class OnlineMapsRange
{
    /// <summary>
    /// Maximum value.
    /// </summary>
    public int max = int.MaxValue;

    /// <summary>
    /// Minimum value.
    /// </summary>
    public int min = int.MinValue;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="min">Minimum value.</param>
    /// <param name="max">Maximum value.</param>
    public OnlineMapsRange(int min = int.MinValue, int max = int.MaxValue)
    {
        this.min = min;
        this.max = max;
    }

    /// <summary>
    /// Checks and limits value.
    /// </summary>
    /// <param name="value">Value</param>
    /// <returns>Value corresponding to the specified range.</returns>
    public int CheckAndFix(int value)
    {
        FixZeroRange();
        if (value < min) value = min;
        if (value > max) value = max;
        return value;
    }

    private void FixZeroRange()
    {
        if (min == 0 && max == 0)
        {
            min = int.MinValue;
            max = int.MaxValue;
        }
    }

    /// <summary>
    /// Checks whether the number in the range.
    /// </summary>
    /// <param name="value">Value</param>
    /// <returns>True - if the number is in the range, false - if not.</returns>
    public bool InRange(int value)
    {
        FixZeroRange();
        return value >= min && value <= max;
    }

    /// <summary>
    /// Converts a range to string.
    /// </summary>
    /// <returns>String</returns>
    public override string ToString()
    {
        return string.Format("Min: {0}, Max: {1}", min, max);
    }

    /// <summary>
    /// Updates the minimum and maximum values​​.
    /// </summary>
    /// <param name="newMin">Minimum value.</param>
    /// <param name="newMax">Maximum value.</param>
    /// <returns>True - if the range is changed, false - if not changed.</returns>
    public bool Update(int newMin, int newMax)
    {
        bool changed = false;
        if (newMin != min)
        {
            min = newMin;
            changed = true;
        }
        if (newMax != max)
        {
            max = newMax;
            changed = true;
        }
        return changed;
    }
}