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
    [RequireComponent(typeof(Rigidbody))]
    public class JCS_Item
        : JCS_UnityObject
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        //----------------------
        // Protected Variables
        protected bool mCanPick = true;
        protected BoxCollider mBoxCollider = null;

        [Tooltip("Audio sound when u pick up this item")]
        [SerializeField] protected AudioClip mPickSound = null;

        [SerializeField] protected KeyCode mPickKey = KeyCode.Z;

        [Tooltip("When player hit this object pick it up automatically.")]
        [SerializeField] protected bool mAutoPick = false;

        //========================================
        //      setter / getter
        //------------------------------
        public bool AutoPick { get { return this.mAutoPick; } set { this.mAutoPick = value; } }
        public bool CanPick { get { return this.mCanPick; } set { this.mCanPick = value; } }
        public BoxCollider GetBoxCollider() { return this.mBoxCollider; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mBoxCollider = this.GetComponent<BoxCollider>();

            // update the data once 
            // depends on what game object is.
            UpdateUnityData();
        }

        private void OnTriggerStay(Collider other)
        {
            if (mAutoPick)
            {
                // picked
                Pick(other);
                return;
            }

            if (JCS_Input.GetKey(mPickKey))
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

            RC_Player p = other.GetComponent<RC_Player>();

            if (mAutoPick && p != null)
            {
                DoPick(other);
                return;
            }

            // Check the colliding object are is active player.
            if (JCS_PlayerManager.instance.IsActivePlayerTransform(other.transform))
            {
                DoPick(other);
            }
        }

        public virtual void SubclassCallBack(Collider other)
        {
            // do anything after the character pick this item up.
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void DoPick(Collider other)
        {
            DropEffect(other);

            if (mPickSound != null)
                JCS_SoundManager.instance.GetGlobalSoundPlayer().PlayOneShotWhileNotPlaying(mPickSound);

            // call item effect.
            SubclassCallBack(other);

            mCanPick = false;
        }
        private void DropEffect(Collider other)
        {
            // Throw Action Effect...
            {
                //JCS_ThrowAction ta = this.gameObject.AddComponent<JCS_ThrowAction>();
                //ta.SetTargetTransform(other.transform);
                //ta.ActiveEffect();
            }

            // Tweener Effect...
            {
                JCS_Tweener tweener = this.gameObject.AddComponent<JCS_Tweener>();
                tweener.EasingY = JCS_TweenType.EASE_OUT_BACK;
                tweener.DurationX = 2.0f;
                tweener.DurationY = 5.0f;
                tweener.DurationZ = 0;
                tweener.StopTweenDistance = 0.2f;
                tweener.DoTweenContinue(other.transform);
            }

            JCS_DestinationDestroy jcsdd = this.gameObject.AddComponent<JCS_DestinationDestroy>();
            jcsdd.DestroyDistance = 0.5f;
            jcsdd.SetTargetTransform(other.transform);
        }

    }
}
