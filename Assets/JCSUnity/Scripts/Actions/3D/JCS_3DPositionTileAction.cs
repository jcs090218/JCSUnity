/**
 * $File: JCS_3DPositionTileAction.cs $
 * $Date: 2017-04-10 02:04:46 $
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
    /// If object goes a certain range set back to certain 
    /// position.
    /// </summary>
    public class JCS_3DPositionTileAction
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        [Header("** Runtime Variables (JCS_3DDistanceTileAction) **")]

        [Tooltip("Is the component active?")]
        [SerializeField]
        private bool mActive = true;

        [Tooltip(@"If object goes out of this distance, will 
be set to this position in each axis.")]
        [SerializeField]
        private Vector3 mAbsolutePositionInAxis = Vector3.zero;


        [Header("** - Max Settings (JCS_3DDistanceTileAction) **")]

        [Tooltip("If axis x is over this will get reset.")]
        [SerializeField]
        private float mMaxX = float.PositiveInfinity;
        [Tooltip("If axis y is over this will get reset.")]
        [SerializeField]
        private float mMaxY = float.PositiveInfinity;
        [Tooltip("If axis z is over this will get reset.")]
        [SerializeField]
        private float mMaxZ = float.PositiveInfinity;


        [Header("** - Min Settings (JCS_3DDistanceTileAction) **")]

        [Tooltip("If axis x is over this will get reset.")]
        [SerializeField]
        private float mMinX = float.NegativeInfinity;
        [Tooltip("If axis y is over this will get reset.")]
        [SerializeField]
        private float mMinY = float.NegativeInfinity;
        [Tooltip("If axis z is over this will get reset.")]
        [SerializeField]
        private float mMinZ = float.NegativeInfinity;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool Active { get { return this.mActive; } set { this.mActive = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        private void Update()
        {
            if (!mActive)
                return;

            Vector3 newPos = this.transform.localPosition;

            if (newPos.x < mMinX || newPos.x > mMaxX)
                newPos.x = mAbsolutePositionInAxis.x;

            if (newPos.y < mMinY || newPos.y > mMaxY)
                newPos.y = mAbsolutePositionInAxis.y;

            if (newPos.z < mMinZ || newPos.z > mMaxZ)
                newPos.z = mAbsolutePositionInAxis.z;

            this.transform.localPosition = newPos;
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
