/**
 * $File: JCS_HitCountEvent.cs $
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

    [RequireComponent(typeof(JCS_HitListEvent))]
    public class JCS_HitCountEvent
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private JCS_HitListEvent mHitList = null;

        [Tooltip("how many hit to destroy this game object.")]
        [SerializeField] private uint mHitCount = 0;

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

        private void Update()
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
