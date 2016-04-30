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
    public class JCS_UsefualFunctions : MonoBehaviour
    {
        public static GameObject SpawnGameObject(string objectPath, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            return Instantiate(Resources.Load<GameObject>(objectPath), position, rotation) as GameObject;
        }

        public static Object SpawnGameObject(Object trans, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion())
        {
            return Instantiate(trans, position, rotation);
        }

        public static string HeaderDecorator(string desc)
        {
            return "** " + desc + " **";
        }

    }
}
