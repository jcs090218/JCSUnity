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

        [Header("** Runtime Variables (JCS_AnimPool) **")]

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

        /// <summary>
        /// Get a animation from the pool randomly.
        /// </summary>
        /// <returns> animation value. </returns>
        public RuntimeAnimatorController GetRandomAnim()
        {
            if (mAnimPool.Length == 0)
                return null;

            int randIndex = JCS_Random.Range(0, mAnimPool.Length);

            return mAnimPool[randIndex];
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
