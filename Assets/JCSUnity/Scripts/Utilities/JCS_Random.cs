/**
 * $File: JCS_Random.cs $
 * $Date: 2017-04-10 21:48:23 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JCSUnity
{
    /// <summary>
    /// Store of random algorithm.
    /// </summary>
    public static class JCS_Random
    {
        /// <summary>
        /// Return normal random range (Integer)
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

        /// <summary>
        /// Return normal random range and cover the max value. (Integer)
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

        /// <summary>
        /// Return normal random range (Float)
        /// </summary>
        /// <param name="min"> mininum value </param>
        /// <param name="max"> maxinum value </param>
        /// <returns> random number </returns>
        public static float Range(float min, float max)
        {
            return Random.Range(min, max);
        }

        /// <summary>
        /// Return a random color.
        /// </summary>
        /// <returns> random color object. </returns>
        public static Color RandomColor(float a = 1.0f)
        {
            return new Color(
                Range(0.0f, 1.0f),
                Range(0.0f, 1.0f),
                Range(0.0f, 1.0f),
                a);
        }

    }
}
