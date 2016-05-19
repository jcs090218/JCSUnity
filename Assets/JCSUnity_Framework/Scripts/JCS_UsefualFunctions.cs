/**
 * $File: JCS_UsefualFunctions.cs $
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
    // Function pointer
    public delegate int JCS_Range(int min, int max);


    public class JCS_UsefualFunctions 
        : MonoBehaviour
    {
        public static GameObject SpawnGameObject(string objectPath, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            return Instantiate(Resources.Load<GameObject>(objectPath), position, rotation) as GameObject;
        }

        public static Object SpawnGameObject(Object trans, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            if (trans == null)
                return null;

            return Instantiate(trans, position, rotation);
        }

        public static string HeaderDecorator(string desc)
        {
            return "** " + desc + " **";
        }

        /// <summary>
        /// Return normal random range (Integer)
        /// </summary>
        /// <param name="min"> mininum value </param>
        /// <param name="max"> maxinum value </param>
        /// <returns> random number </returns>
        public static int JCS_IntRange(int min, int max)
        {
            return Random.Range(min, max);
        }
        /// <summary>
        /// Return normal random range (Float)
        /// </summary>
        /// <param name="min"> mininum value </param>
        /// <param name="max"> maxinum value </param>
        /// <returns> random number </returns>
        public static float JCS_FloatRange(float min, float max)
        {
            return Random.Range(min, max);
        }

    }
}
