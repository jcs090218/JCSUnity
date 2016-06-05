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

    public class JCS_Mathf 
        : MonoBehaviour
    {
        public static float AbsoluteValue(float val)
        {
            if (val < 0)
                return -val;
            return val;
        }

        public static Vector3 AbsoluteValue(Vector3 val)
        {
            Vector3 tempVal = val;

            tempVal.x = AbsoluteValue(tempVal.x);
            tempVal.y = AbsoluteValue(tempVal.y);
            tempVal.z = AbsoluteValue(tempVal.z);

            return tempVal;
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

        public static float ToPositive(int val)
        {
            return AbsoluteValue(val);
        }

        public static float ToNegative(int val)
        {
            return -(AbsoluteValue(val));
        }

        public static bool isPositive(int val)
        {
            return (val > 0 && val != 0) ? true : false;
        }

        public static bool isNegative(int val)
        {
            return (val < 0 && val != 0) ? true : false;
        }

        public static float Reciprocal(float val)
        {
            return (1 / val);
        }

        public static float Sqr(float val)
        {
            return (val * val);
        }

        public static float DistanceOfUnitVector(float x1, float x2)
        {
            float distance = x2 - x1;
            distance = AbsoluteValue(distance);
            return distance;
        }

    }
}
