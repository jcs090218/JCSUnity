/**
 * $File: JCS_2DClimbableObject.cs $
 * $Date: 2017-05-02 14:57:23 $
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
    /// Base class for all the 2d climable object.
    /// 
    /// 2DRope, 2DLadder, etc.
    /// </summary>
    [RequireComponent(typeof(JCS_OrderLayerObject))]
    public abstract class JCS_2DClimbableObject
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        /*
         * To have this component in order to let the 
         * character's sorting layer infont or behind the 
         * climbing object. 
         */
        private JCS_OrderLayerObject mOrderLayerObject = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_OrderLayerObject OrderLayerObject { get { return this.mOrderLayerObject; } }

        //========================================
        //      Unity's function
        //------------------------------
        protected virtual void Awake()
        {
            this.mOrderLayerObject = this.GetComponent<JCS_OrderLayerObject>();
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Something that needed to check in update can be design here.
        /// This function should get call by when the player is done climbing.
        /// </summary>
        public abstract void ClimbableUpdate();

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
