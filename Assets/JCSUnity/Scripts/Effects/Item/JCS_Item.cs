/**
 * $File: JCS_ItemPickable.cs $
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
    public delegate void PickCallback(Collider other);

    /// <summary>
    /// Base class for all the item subclasses.
    /// </summary>
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

        [Header("** Check Variables (JCS_Item) **")]

        [Tooltip(@"Collider detect can be picked.
u can set this directly in order to get the pick effect too.
Once u set this, the object will do tween 
effect to this transform.")]
        [SerializeField]
        protected Collider mPickCollider = null;


        [Header("** Player Specific Settings (JCS_Item) **")]

        [Tooltip("Is the auto pick collider must be player?")]
        [SerializeField]
        protected bool mMustBeActivePlayer = true;

        [Tooltip("Key to active pick event.")]
        [SerializeField]
        protected KeyCode mPickKey = KeyCode.Z;


        [Header("** System Settings (JCS_Item) **")]

        [Tooltip("Pick item by click/mouse?")]
        [SerializeField]
        protected bool mPickByMouseDown = false;

        [Tooltip("When player hit this object pick it up automatically.")]
        [SerializeField]
        protected bool mAutoPickColliderTouched = false;

        [Tooltip(@"When the item are on the ground, pick it up when there 
is object that we target.")]
        [SerializeField]
        protected bool mAutoPickWhileCan = false;


        [Header("** Sound Settings (JCS_Item) **")]

        [Tooltip("Audio sound when u pick up this item")]
        [SerializeField]
        protected AudioClip mPickSound = null;

        [Tooltip("Audio sound when u pick up this item (Global) ")]
        [SerializeField]
        protected AudioClip mEffectSound = null;

        protected PickCallback mPickCallback = DefaultPickCallback;


        [Header("** Optional Variables (JCS_UnityObject) **")]
        [SerializeField]
        private JCS_TransfromTweener mTweener = null;
        [SerializeField]
        private JCS_DestinationDestroy mDestinationDestroy = null;

        //========================================
        //      setter / getter
        //------------------------------
        public bool AutoPickColliderTouched { get { return this.mAutoPickColliderTouched; } set { this.mAutoPickColliderTouched = value; } }
        public bool PickByMouseDown { get { return this.mPickByMouseDown; } set { this.mPickByMouseDown = value; } }
        public bool AutoPickWhileCan { get { return this.mAutoPickWhileCan; } set { this.mAutoPickWhileCan = value; } }
        public bool CanPick { get { return this.mCanPick; } set { this.mCanPick = value; } }
        public BoxCollider GetBoxCollider() { return this.mBoxCollider; }
        public Collider PickCollider { get { return this.mPickCollider; } set { this.mPickCollider = value; } }

        public void SetPickCallback(PickCallback func) { this.mPickCallback = func; }
        public PickCallback GetPickCallback() { return this.mPickCallback; }


        //========================================
        //      Unity's function
        //------------------------------
        protected virtual void Awake()
        {
            mBoxCollider = this.GetComponent<BoxCollider>();

            // update the data once 
            // depends on what game object is.
            UpdateUnityData();
        }

        protected virtual void Start()
        {
            if (mTweener == null)
                mTweener = this.GetComponent<JCS_TransfromTweener>();
            if (mDestinationDestroy == null)
                mDestinationDestroy = this.GetComponent<JCS_DestinationDestroy>();
        }

        protected virtual void Update()
        {
            DoAutoPickWhileCan();
        }

        protected virtual void OnTriggerStay(Collider other)
        {
            if (mAutoPickColliderTouched)
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

        protected virtual void OnMouseDown()
        {
            if (!mPickByMouseDown)
                return;

            if (mPickCollider == null)
            {
                JCS_Debug.JcsErrors(
                    this, "Cannot pick the item cuz there is no collider set.");

                return;
            }

            Pick(mPickCollider);
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
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

            JCS_Player p = other.GetComponent<JCS_Player>();

            if (mAutoPickColliderTouched && p != null)
            {
                DoPick(other);
                return;
            }

            if (mMustBeActivePlayer)
            {
                // Check the colliding object are is active player.
                if (JCS_PlayerManager.instance.IsActivePlayerTransform(other.transform))
                {
                    DoPick(other);
                }
            }
            else
            {
                DoPick(other);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public static void DefaultPickCallback(Collider other)
        {
            // do anything after the character pick this item up.
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        private void DoPick(Collider other)
        {
            DropEffect(other);

            if (mPickSound != null)
                JCS_SoundManager.instance.GetGlobalSoundPlayer().PlayOneShotWhileNotPlaying(mPickSound);

            // call item effect.
            mPickCallback.Invoke(other);

            // play the sound
            JCS_SoundManager.instance.GetGlobalSoundPlayer().PlayOneShot(mEffectSound);

            mCanPick = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
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
                if (mTweener == null)
                {
                    // default settings
                    mTweener = this.gameObject.AddComponent<JCS_TransfromTweener>();

                    mTweener.EasingY = JCS_TweenType.EASE_OUT_BACK;
                    mTweener.DurationX = 2.0f;
                    mTweener.DurationY = 5.0f;
                    mTweener.DurationZ = 0;
                }
                mTweener.StopTweenDistance = 0.2f;
                mTweener.DoTweenContinue(other.transform);
            }


            if (mDestinationDestroy == null)
            {
                // default settings
                mDestinationDestroy = this.gameObject.AddComponent<JCS_DestinationDestroy>();
                mDestinationDestroy.DestroyDistance = 0.5f;
            }
            mDestinationDestroy.SetTargetTransform(other.transform);
        }

        /// <summary>
        /// If the Pick Collider is not null, 
        /// then pick it up immediatly while we can pick.
        /// </summary>
        private void DoAutoPickWhileCan()
        {
            if (!mAutoPickWhileCan)
                return;

            if (mPickCollider == null)
                return;

            Pick(mPickCollider);
        }

    }
}
