/**
 * $File: JCS_ItemPickable.cs $
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

    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(JCS_SoundPlayer))]
    public class JCS_Item
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private bool mCanPick = true;
        private BoxCollider mBoxCollider = null;

        private JCS_SoundPlayer mSoundPlayer = null;
        [Tooltip("Audio sound when u pick up this item")]
        [SerializeField] private AudioClip mPickSound = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool CanPick { get { return this.mCanPick; } set { this.mCanPick = value; } }
        public BoxCollider GetBoxCollider() { return this.mBoxCollider; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mBoxCollider = this.GetComponent<BoxCollider>();
            mSoundPlayer = this.GetComponent<JCS_SoundPlayer>();

            // update the data once 
            // depends on what game object is.
            UpdateUnityData();
        }

        private void OnTriggerStay(Collider other)
        {
            if (JCS_Input.GetKeyDown(KeyCode.Z))
            {
                Pick(other);
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void Pick(Collider other)
        {
            if (!mCanPick)
                return;

            JCS_OneJump joj = this.GetComponent<JCS_OneJump>();
            if (joj != null)
            {

                // Only when item is on the ground!
                if (joj.GetVelocity().y != 0)
                    return;
            }

            // Check the colliding object are is active player.
            if (JCS_PlayerManager.instance.IsActivePlayerTransform(other.transform))
            {
                JCS_ThrowAction ta = this.gameObject.AddComponent<JCS_ThrowAction>();
                ta.SetTargetTransform(other.transform);
                ta.ActiveEffect();

                JCS_DestinationDestroy jcsdd = this.gameObject.AddComponent<JCS_DestinationDestroy>();
                jcsdd.SetTargetTransform(other.transform);

                if (mPickSound != null)
                    mSoundPlayer.PlayOneShot(mPickSound);

                mCanPick = false;
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        

    }
}
