/**
 * $File: JCS_HitCountEvent.cs $
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
    /// Count the hit base on the hit list.
    /// </summary>
    [RequireComponent(typeof(JCS_HitListEvent))]
    public class JCS_HitCountEvent
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_HitListEvent mHitList = null;

        [Tooltip(@"How many hit to destroy this game object. 
(If this is 0 than wont active hit animation event.)")]
        [SerializeField] private uint mHitCount = 1;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mHitList = this.GetComponent<JCS_HitListEvent>();
        }

        private void FixedUpdate()
        {
            if (mHitList.IsHit)
            {
                --mHitCount;
                CheckDestroy();
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
        private void CheckDestroy()
        {
            if (mHitCount <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                mHitList.IsHit = false;
            }
        }
    }
}
