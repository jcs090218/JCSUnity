/**
 * $File: JCS_2DPositionPlatform.cs $
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
            if (mPlatformTrigger != null)
                mPlatformTrigger.name = JCS_GameSettings.instance.PLATFORM_TRIGGER_NAME;

            // add to list
            JCS_2DGameManager.instance.AddPlatformList(this);
        }

        protected void OnTriggerStay(Collider other)
        {
            JCS_GameSettings gs = JCS_GameSettings.instance;

            if (mPlatformTrigger.name != gs.PLATFORM_TRIGGER_NAME)
                return;

            CharacterController cc = other.transform.GetComponent<CharacterController>();

            if (cc == null)
                return;

            Vector3 targetPos = other.transform.position;
            Vector3 platformPos = this.transform.position;

            float halfOfCharacterControllerHeight = ((cc.height / 2) + cc.radius) * cc.transform.localScale.y;

            // TODO(JenChieh): missing the offset
            float platformUpperBound = platformPos.y;

            if ((targetPos.y - halfOfCharacterControllerHeight) > 
                platformUpperBound)
            {
                mPlatformCollider.isTrigger = false;
            }
            else
            {
                mPlatformCollider.isTrigger = true;
            }
        }

        protected void OnTriggerExit()
        {
            mPlatformCollider.isTrigger = true;
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
