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

        private JCS_ClimbableManager mClimbableManager = null;

        //----------------------
        // Protected Variables

        [Header("** Initilaize Variable (JCS_OrderLayerObject) **")]

        [Tooltip("Ground/Platform the ladder lean on.")]
        [SerializeField]
        protected JCS_2DPositionPlatform mPositionPlatform = null;

        //========================================
        //      setter / getter
        //------------------------------
        public JCS_OrderLayerObject OrderLayerObject { get { return this.mOrderLayerObject; } }
        public JCS_ClimbableManager ClimbableManager { get { return this.mClimbableManager; } }
        public JCS_2DPositionPlatform PositionPlatform { get { return this.mPositionPlatform; } }

        //========================================
        //      Unity's function
        //------------------------------
        protected virtual void Awake()
        {
            this.mOrderLayerObject = this.GetComponent<JCS_OrderLayerObject>();
        }

        protected virtual void Start()
        {
            if (ClimbableManager == null)
            {
                /**
                 *  if climbable object found in the scene, we must have 
                 *  the manager to manage.
                 */
                if (JCS_ClimbableManager.instance == null)
                    JCS_ClimbableManager.instance = JCS_IndieManager.instance.gameObject.AddComponent<JCS_ClimbableManager>();

                this.mClimbableManager = JCS_ClimbableManager.instance;
            }
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

        /// <summary>
        /// Check if is on top of the lean platform.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>
        /// true: is on top of the box.
        /// false: vice versa.
        /// </returns>
        public virtual bool IsOpTopOfLeanPlatform(JCS_Player player)
        {
            if (mPositionPlatform == null || player == null)
                return false;

            bool isTopOfBox = JCS_Physics.TopOfBox(
                       player.GetCharacterController(),
                       mPositionPlatform.GetPlatformCollider());

            return isTopOfBox;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
