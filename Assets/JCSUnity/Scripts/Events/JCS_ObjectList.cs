/**
 * $File: JCS_ObjectList.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                    Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;


namespace JCSUnity
{

    /// <summary>
    /// 
    /// </summary>
    public class JCS_ObjectList
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Tooltip("List of objects.")]
        [SerializeField] private Transform[] mObjects = null;
        

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Return a random object from the list.
        /// </summary>
        /// <returns> object </returns>
        public Transform GetRandomObjectFromList()
        {
            if (mObjects.Length == 0)
                return null;

            int randIndex = JCS_Utility.JCS_IntRange(0, mObjects.Length);

            Transform randObj = mObjects[randIndex];

            return randObj;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
