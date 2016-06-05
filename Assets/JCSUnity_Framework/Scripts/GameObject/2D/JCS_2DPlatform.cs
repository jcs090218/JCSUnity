/**
 * $File: JCS_2DPlatform.cs $
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

    public class JCS_2DPlatform 
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
        protected virtual void Start()
        {

            JCS_Player player = JCS_GameManager.instance.GetJCSPlayer();
            if (mPlatformCollider == null ||
                mPlatformTrigger == null)
            {
                JCS_GameErrors.JcsErrors("JCS_2DPlatform");
                return;
            }

            Physics.IgnoreCollision(mPlatformCollider,
                    mPlatformTrigger, true);
        }


        protected virtual void OnTriggerEnter(Collider other)
        {
            JCS_Player player = JCS_GameManager.instance.GetJCSPlayer();
            if (other.gameObject.name == player.name)
            {
                Physics.IgnoreCollision(mPlatformCollider,
                    player.GetCharacterController(), true);

            }

        }

        protected virtual void OnTriggerExit(Collider other)
        {
            JCS_Player player = JCS_GameManager.instance.GetJCSPlayer();
            if (other.gameObject.name == player.name)
            {
                Physics.IgnoreCollision(mPlatformCollider,
                    player.GetCharacterController(), false);

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
