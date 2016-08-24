/**
 * $File: JCS_Mathf.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    public class JCS_Mathf 
        : MonoBehaviour
    {

        /// <summary>
        /// Absolute the value. (Integer)
        /// </summary>
        /// <param name="val"> value to absolute </param>
        /// <returns> absolute value </returns>
        public static int AbsoluteValue(int val)
        {
            if (val < 0)
                return -val;
            return val;
        }
        /// <summary>
        /// Absolute the value. (Float)
        /// </summary>
        /// <param name="val"> value to absolute </param>
        /// <returns> absolute value </returns>
        public static float AbsoluteValue(float val)
        {
            if (val < 0)
                return -val;
            return val;
        }
        /// <summary>
        /// Absolute the value. (Vector3)
        /// </summary>
        /// <param name="val"> value to absolute </param>
        /// <returns> absolute value </returns>
        public static Vector3 AbsoluteValue(Vector3 val)
        {
            Vector3 tempVal = val;

            tempVal.x = AbsoluteValue(tempVal.x);
            tempVal.y = AbsoluteValue(tempVal.y);
            tempVal.z = AbsoluteValue(tempVal.z);

            return tempVal;
        }
        /// <summary>
        /// Set the value to positive (Integer)
        /// </summary>
        /// <param name="val"> value to positive </param>
        /// <returns> positive value </returns>
        public static float ToPositive(int val)
        {
            return AbsoluteValue(val);
        }
        /// <summary>
        /// Set the value to positive (Float)
        /// </summary>
        /// <param name="val"> value to positive </param>
        /// <returns> positive value </returns>
        public static float ToPositive(float val)
        {
            return AbsoluteValue(val);
        }
        /// <summary>
        /// Set the value to negative (Integer)
        /// </summary>
        /// <param name="val"> value to negative </param>
        /// <returns> negative value </returns>
        public static float ToNegative(int val)
        {
            return -(AbsoluteValue(val));
        }
        /// <summary>
        /// Set the value to negative (Float)
        /// </summary>
        /// <param name="val"> value to negative </param>
        /// <returns> negative value </returns>
        public static float ToNegative(float val)
        {
            return -(AbsoluteValue(val));
        }
        /// <summary>
        /// Check if the value is positive (Integer)
        /// </summary>
        /// <param name="val"> value to check </param>
        /// <returns> true: is positive, false: is negative </returns>
        public static bool isPositive(int val)
        {
            return (val > 0 && val != 0) ? true : false;
        }
        /// <summary>
        /// Check if the value is positive (Float)
        /// </summary>
        /// <param name="val"> value to check </param>
        /// <returns> true: is positive, false: is negative </returns>
        public static bool isPositive(float val)
        {
            return (val > 0 && val != 0) ? true : false;
        }
        /// <summary>
        /// Check if the value is negative (Integer)
        /// </summary>
        /// <param name="val"> value to check </param>
        /// <returns> true: is negative, false: is positive </returns>
        public static bool isNegative(int val)
        {
            return (val < 0 && val != 0) ? true : false;
        }
        /// <summary>
        /// Check if the value is negative (Float)
        /// </summary>
        /// <param name="val"> value to check </param>
        /// <returns> true: is negative, false: is positive </returns>
        public static bool isNegative(float val)
        {
            return (val < 0 && val != 0) ? true : false;
        }
        /// <summary>
        /// To Reverse the value.
        /// </summary>
        /// <param name="val"> value to reverse </param>
        /// <returns> reverse value </returns>
        public static float ToReverse(float val)
        {
            return -val;
        }
        /// <summary>
        /// Reciprocal of the value
        /// </summary>
        /// <param name="val"> value to reciprocal </param>
        /// <returns> reciprocal value </returns>
        public static float Reciprocal(float val)
        {
            return (1 / val);
        }
        /// <summary>
        /// Short hand of sqr of value
        /// </summary>
        /// <param name="val"> value u want to sqr </param>
        /// <returns> sqr value </returns>
        public static float Sqr(float val)
        {
            return (val * val);
        }
        /// <summary>
        /// Framework define vector1's distance.
        /// The same as Unity's API 
        /// "Vector2.distance" and "Vector3.distnace".
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static float DistanceOfUnitVector(float x1, float x2)
        {
            float distance = x2 - x1;
            distance = AbsoluteValue(distance);
            return distance;
        }
        /// <summary>
        /// Return bool to check the number is even or not.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool isEven(int index)
        {
            return ((index % 2) == 0);
        }
        /// <summary>
        /// Center of the array length
        /// </summary>
        /// <param name="length"> length of the array </param>
        /// <returns> center of the array length </returns>
        public static int FindMiddleIndex(int length)
        {
            return (length / 2);
        }
        /// <summary>
        /// Pass in an index that is out of range, 
        /// this function will return the in range index
        /// within the range 0 ~ length with over flow calculation.
        /// </summary>
        /// <param name="targetIndex"> index u pass in </param>
        /// <param name="length"> length of the array. </param>
        /// <returns> over flow calculation index. </returns>
        public static int OverFlowIndex(int targetIndex, int length)
        {
            if (targetIndex < length && targetIndex > 0)
                return targetIndex;

            if (targetIndex < 0)
                return length + (targetIndex % length);

            return (targetIndex % length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentVal"></param>
        /// <param name="targetVal"></param>
        /// <returns></returns>
        public static int ApproachTo(int currentVal, int targetVal)
        {
            if (currentVal == targetVal)
                return currentVal;

            if (currentVal < targetVal)
                return ++currentVal;

            if (targetVal < currentVal)
                return --currentVal;

            JCS_GameErrors.JcsErrors(
                "JCS_Mathf", 
                 
                "Errors with approaching to a value...");

            return 0;
        }
        /// <summary>
        /// 計算旋轉的點 (JCS_VECTOR2F)
        /// </summary>
        /// <param name="point"> 我們要計算的"點" </param>
        /// <param name="cos"> Cos 的角度 "Cos(angle)" </param>
        /// <param name="sin"> Sin 的角度 "Sin(angle)" </param>
        /// <param name="origin"> 以這個"點"為中心旋轉 </param>
        /// <returns></returns>
        public static Vector3 RotatePointZ(
            Vector3 point, float cos, float sin, Vector3 origin)
        {
            return new Vector3(
            origin.x + ((point.x - origin.x) * cos) - ((point.y - origin.y) * sin),
            origin.y + ((point.x - origin.x) * sin) + ((point.y - origin.y) * cos),
            point.z);
        }
        public static Vector3 RotatePointX(
            Vector3 point, float cos, float sin, Vector3 origin)
        {
            return new Vector3(
            point.x,
            origin.z + ((point.z - origin.z) * cos) - ((point.y - origin.y) * sin),
            origin.y + ((point.z - origin.z) * sin) + ((point.y - origin.y) * cos));
        }
        public static Vector3 RotatePointY(
            Vector3 point, float cos, float sin, Vector3 origin)
        {
            return new Vector3(
            origin.x + ((point.x - origin.x) * cos) - ((point.z - origin.z) * sin),
            point.y,
            origin.z + ((point.x - origin.x) * sin) + ((point.z - origin.z) * cos));
        }

        /// <summary>
        /// check if the vector value is infinity
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool IsInfinity(Vector2 check)
        {
            return (float.IsInfinity(check.x) ||
                float.IsInfinity(check.y));
        }
        /// <summary>
        /// check if the vector value is infinity
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool IsInfinity(Vector3 check)
        {
            return (float.IsInfinity(check.x) ||
                float.IsInfinity(check.y) ||
                float.IsInfinity(check.z));
        }

        /// <summary>
        /// Return min value compare the two pass
        /// in values.
        /// </summary>
        /// <param name="a"> value a </param>
        /// <param name="b"> value b </param>
        /// <returns> smaller value </returns>
        public static float Min(float a, float b)
        {
            return (a < b) ? a : b;
        }

        /// <summary>
        /// Return max value compare the two pass 
        /// in values.
        /// </summary>
        /// <param name="a"> value a </param>
        /// <param name="b"> value b </param>
        /// <returns> larger value </returns>
        public static float Max(float a, float b)
        {
            return (a > b) ? a : b;
        }

        /// <summary>
        /// Check if the number is valid.
        /// </summary>
        /// <param name="vec"> vector to check </param>
        /// <returns> 
        /// true: is nan
        /// false: valid value
        /// </returns>
        public static bool IsNaN(Vector3 vec)
        {
            return (float.IsNaN(vec.x) || float.IsNaN(vec.y) || float.IsNaN(vec.z));
        }

        /// <summary>
        /// Check if the number is valid.
        /// </summary>
        /// <param name="vec"> vector to check </param>
        /// <returns> 
        /// true: is nan
        /// false: valid value
        /// </returns>
        public static bool IsNaN(Vector2 vec)
        {
            return (float.IsNaN(vec.x) || float.IsNaN(vec.y));
        }

    }
}
