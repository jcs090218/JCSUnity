/**
 * $File: JCS_AnimPool.cs $
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

    /// <summary>
    /// Pool of animation
    /// </summary>
    public class JCS_AnimPool
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables **")]
        [Tooltip("Pool to randomly choose.")]
        [SerializeField]
        private RuntimeAnimatorController[] mAnimPool = null;

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
        public RuntimeAnimatorController GetRandomAnim()
        {
            if (mAnimPool.Length == 0)
                return null;

            int randIndex = JCS_Utility.JCS_IntRange(0, mAnimPool.Length);

            return mAnimPool[randIndex];
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
