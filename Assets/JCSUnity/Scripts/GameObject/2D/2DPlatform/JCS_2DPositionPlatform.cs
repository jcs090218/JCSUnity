/**
 * $File: JCS_2DPositionPlatform.cs $
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
    /// Accurate platform calculate the movement 
    /// base on the position.
    /// </summary>
    public class JCS_2DPositionPlatform
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        [Header("** Runtime Variables **")]
        [Tooltip("Collider will detect to see if the player close to the actual one")]
        [SerializeField] private BoxCollider mPlatformTrigger = null;
        [Tooltip("Collider will actually stop the player")]
        [SerializeField] private BoxCollider mPlatformCollider = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public BoxCollider GetPlatformTrigger() { return this.mPlatformTrigger; }
        public BoxCollider GetPlatformCollider() { return this.mPlatformCollider; }

        //========================================
        //      Unity's function
        //------------------------------
        protected void Start()
        {
            // add to list
            JCS_2DGameManager.instance.AddPlatformList(this);
        }

        protected void OnTriggerStay(Collider other)
        {

            CharacterController cc = other.transform.GetComponent<CharacterController>();

            if (cc == null)
                return;

            if (JCS_Physics.TopOfBox(cc, mPlatformCollider))
            {
                Physics.IgnoreCollision(
                    mPlatformCollider,
                    cc, 
                    false);
            }
            else
            {
                Physics.IgnoreCollision(
                    mPlatformCollider,
                    cc,
                    true);
            }

            JCS_2DSideScrollerPlayer p = other.GetComponent<JCS_2DSideScrollerPlayer>();
            if (p == null)
                return;

            if (p.CharacterState == JCS_2DCharacterState.CLIMBING)
            {
                // IMPORTANT(JenChieh): Note that IgnoreCollision will reset 
                // the trigger state of affected colliders, 
                // so you might receive OnTriggerExit and 
                // OnTriggerEnter messages in response to 
                // calling this.
                Physics.IgnoreCollision(
                    mPlatformCollider,
                    p.GetCharacterController(),
                    true);

            }

            p.ResetingCollision = true;
        }

        private void OnTriggerExit(Collider other)
        {
            //CharacterController cc = other.transform.GetComponent<CharacterController>();

            //if (cc == null)
            //    return;

            //Physics.IgnoreCollision(
            //        mPlatformCollider,
            //        cc,
            //        true);

            //JCS_2DSideScrollerPlayer p = other.GetComponent<JCS_2DSideScrollerPlayer>();
            //if (p == null)
            //    return;

            //p.ResetingCollision = true;
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
