/**
 * $File: JCS_2DRope.cs $
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
    /// 
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class JCS_2DRope 
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private BoxCollider mBoxCollider = null;

        [Header("** Initilaize Variable (JCS_2DRope) **")]
        [Tooltip("Ground/Platform the ladder lean on.")]
        [SerializeField] private BoxCollider mPlatform = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        protected virtual void Awake()
        {
            mBoxCollider = this.GetComponent<BoxCollider>();

            if (mPlatform == null)
            {
                JCS_Debug.JcsErrors(
                    "JCS_2DRope",
                     
                    "U have a ladder without a platform/ground to lean on.");
            }
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            JCS_2DSideScrollerPlayer p = other.GetComponent<JCS_2DSideScrollerPlayer>();
            if (p == null)
                return;

            if (p.GetClimbingTransform() != this.transform)
            {
                p.CanRope = true;
                p.CanLadder = false;
                p.SetClimbingTransform(this.transform);
            }
        }
        protected virtual void OnTriggerExit(Collider other)
        {
            JCS_2DSideScrollerPlayer p = other.GetComponent<JCS_2DSideScrollerPlayer>();
            if (p == null)
                return;

            // check if player leave the ladder.
            if (!JCS_Physics.JcsOnTriggerCheck(p.GetCharacterController(), mBoxCollider))
            {
                p.CharacterState = JCS_2DCharacterState.NORMAL;

                if (p.JumpCount == 0 && JCS_Physics.TopOfBox(p.GetCharacterController(), mPlatform))
                {
                    // set on the platform
                    JCS_Physics.SetOnTopOfBox(p.GetCharacterController(), mPlatform, 0.1f);
                    p.VelY = -1;
                }
            }

            p.SetClimbingTransform(null);
            p.GetCharacterAnimator().PlayAnimationInFrame();
            p.CanRope = false;
        }

        protected virtual void OnTriggerStay(Collider other)
        {

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
