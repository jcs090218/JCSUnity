/**
 * $File: JCS_HitListEvent.cs $
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

    public class JCS_HitListEvent
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Tooltip("List of object will lower 1 hit count.")]
        [SerializeField] private string[] mHitObjectList = null;
        [Tooltip("Base on Hit Object List will atuomatically add clone object.")]
        [SerializeField] private bool mIncludeClone = true;
        private bool mIsHit = false;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool IsHit { get { return this.mIsHit; } set { this.mIsHit = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void OnTriggerEnter(Collider other)
        {

            foreach (string obj in mHitObjectList)
            {
                if (other.gameObject.name == obj)
                {
                    mIsHit = true;
                    continue;
                }

                // Check clone
                if (mIncludeClone)
                {
                    if (other.gameObject.name == (obj + "(Clone)"))
                    {
                        mIsHit = true;
                        continue;
                    }
                }
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
