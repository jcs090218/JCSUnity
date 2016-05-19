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
        [SerializeField] private BoxCollider mPlatformCollider = null;
        [SerializeField] private BoxCollider mPlatformTrigger = null;


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
            mPlatformTrigger.name = JCS_GameSettings.instance.PLATFORM_TRIGGER_NAME;
        }

        private void OnTriggerStay(Collider other)
        {
            JCS_GameSettings gs = JCS_GameSettings.instance;

            if (other.name != gs.PLAYER_NAME)
                return;

            if (mPlatformTrigger.name != gs.PLATFORM_TRIGGER_NAME)
                return;

            Vector3 targetPos = other.transform.position;
            Vector3 thisPos = this.transform.position;

            if ((targetPos.y - (gs.PLATFORM_AND_PLAYER_GAP + gs.GAP_ACCEPT_RANGE)) > thisPos.y)
            {
                mPlatformCollider.isTrigger = false;
            }
            else
            {
                mPlatformCollider.isTrigger = true;
            }
        }

        private void OnTriggerExit()
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
