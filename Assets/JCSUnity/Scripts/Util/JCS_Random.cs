/**
 * $File: JCS_Random.cs $
 * $Date: 2017-04-10 21:48:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Store of random algorithm.
    /// </summary>
    public static class JCS_Random
    {
        /* Variables */

        /* Setter & Getter */

        /* Functions */

        /// <summary>
        /// Return normal random range.
        /// </summary>
        /// <param name="min"> mininum value </param>
        /// <param name="max"> maxinum value </param>
        /// <returns> random number </returns>
        public static int Range(int min, int max)
        {
            return Random.Range(min, max);
        }
        public static uint Range(uint min, uint max)
        {
            return (uint)Random.Range(min, max);
        }
        public static float Range(float min, float max)
        {
            return Random.Range(min, max);
        }

        /// <summary>
        /// Return normal random range and cover the max value.
        /// </summary>
        /// <param name="min"> mininum value </param>
        /// <param name="max"> maxinum value </param>
        /// <returns> random number </returns>
        public static int RangeInclude(int min, int max)
        {
            return Range(min, max + 1);
        }
        public static uint RangeInclude(uint min, uint max)
        {
            return Range(min, max + 1);
        }
        public static float RangeInclude(float min, float max)
        {
            float include = FindDecimalInclude(max);
            return Range(min, max + include);
        }

        /// <summary>
        /// Return a random vector 2 int.
        /// </summary>
        public static Vector2Int Vector2Int(int min, int max)
        {
            int x = Range(min, max);
            int y = Range(min, max);
            return new Vector2Int(x, y);
        }

        /// <summary>
        /// Return a random vector 2.
        /// </summary>
        public static Vector2 Vector2(float min, float max)
        {
            float x = Range(min, max);
            float y = Range(min, max);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Return a random vector 3 int.
        /// </summary>
        public static Vector3Int Vector3Int(int min, int max)
        {
            int x = Range(min, max);
            int y = Range(min, max);
            int z = Range(min, max);
            return new Vector3Int(x, y, z);
        }

        /// <summary>
        /// Return a random vector 3.
        /// </summary>
        public static Vector3 Vector3(float min, float max)
        {
            float x = Range(min, max);
            float y = Range(min, max);
            float z = Range(min, max);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Return a random Quaternion.
        /// </summary>
        public static Quaternion Quaternion(float min, float max)
        {
            float x = Range(min, max);
            float y = Range(min, max);
            float z = Range(min, max);
            return UnityEngine.Quaternion.Euler(x, y, z);
        }

        /// <summary>
        /// Return a random color.
        /// </summary>
        /// <returns> random color object. </returns>
        public static Color Color()
        {
            float a = Range(0.0f, 1.0f);
            return Color(a);
        }
        public static Color Color(float a = 1.0f)
        {
            float x = Range(0.0f, 1.0f);
            float y = Range(0.0f, 1.0f);
            float z = Range(0.0f, 1.0f);

            return new Color(x, y, z, a);
        }

        /// <summary>
        /// Find the decimal include.
        /// </summary>
        /// <param name="num"> Number use to calculate. </param>
        /// <returns> Return the include decimal number. </returns>
        private static float FindDecimalInclude(float num)
        {
            string str = num.ToString();

            if (str.Contains("."))
                str = str.Split('.')[1];

            return 1.0f / Mathf.Pow(10.0f, str.Length);
        }

        /// <summary>
        /// Choose one object from the list.
        /// </summary>
        /// <typeparam name="T"> Type of the object. </typeparam>
        /// <param name="inArray"> The list or array to choose from. </param>
        /// <returns> The chosen object from the list or array. </returns>
        public static T ChooseOneE<T>(params T[] args)  // Ellipsis
        {
            return ChooseOne(args);
        }
        public static T ChooseOne<T>(ICollection<T> lst)
        {
            if (lst.Count == 0)
                return default(T);

            int index = Range(0, lst.Count);

            return lst.ElementAt(index);
        }
        public static KeyValuePair<T, K> ChooseOne<T, K>(ICollection<KeyValuePair<T, K>> dict)
        {
            if (dict == null || dict.Count == 0)
                return default;

            int index = Range(0, dict.Count);

            return dict.ElementAt(index);
        }

        /// <summary>
        /// Return a random enum value.
        /// </summary>
        /// <typeparam name="T"> The enum type. </typeparam>
        /// <returns> The enum value that has been randomly chosen. </returns>
        public static T EnumValue<T>()
        {
            var r = new System.Random();
            var v = System.Enum.GetValues(typeof(T));
            return (T)v.GetValue(r.Next(v.Length));
        }

        /// <summary>
        /// Return a random point in bounds.
        /// </summary>
        /// <param name="bounds"> The bounds object. </param>
        public static Vector3 PointInBounds(Bounds bounds)
        {
            return new Vector3(
                Range(bounds.min.x, bounds.max.x),
                Range(bounds.min.y, bounds.max.y),
                Range(bounds.min.z, bounds.max.z));
        }

        /// <summary>
        /// Return a random position in sphere.
        /// </summary>
        public static Vector3 PointInSphere(Vector3 centerPosition, float radius)
        {
            Vector3 randomPoint = Random.insideUnitSphere * radius;
            return centerPosition + randomPoint;
        }
    }
}
