/**
 * $File: JCS_Mathf.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{
    public class JCS_Mathf : MonoBehaviour
    {
        public static float AbsoluteValue(float val)
        {
            if (val < 0)
                return -val;
            return val;
        }

        public static float ToPositive(float val)
        {
            return AbsoluteValue(val);
        }

        public static float ToNegative(float val)
        {
            return -(AbsoluteValue(val));
        }

        public static bool isPositive(float val)
        {
            return (val > 0 && val != 0) ? true : false;
        }

        public static bool isNegative(float val)
        {
            return (val < 0 && val != 0) ? true : false;
        }
    }
}
