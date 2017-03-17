/**
 * $File: JCS_OneJump.cs $
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
    /// Currently for GUI use.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class JCS_OneJump
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private bool mEffect = false;

        [Header("** Runtime Variables **")]
        [Tooltip("How many force to apply on jump")]
        [SerializeField]
        private float mJumpForce = 10;
        [SerializeField]
        private float mMoveForce = 10;
        [SerializeField]
        private float mItemGravity = 2;

        private Vector3 mVelocity = Vector3.zero;
        private BoxCollider mBoxCollider = null;

        private Rigidbody mRigidbody = null;
        private RaycastHit mRaycastHit;
        private Collider mFixCollider = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }
        public Vector3 GetVelocity() { return this.mVelocity; }

        public Rigidbody GetRigidbody() { return this.mRigidbody; }
        public Collider GetFixCollider() { return this.mFixCollider; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mBoxCollider = this.GetComponent<BoxCollider>();
            mRigidbody = this.GetComponent<Rigidbody>();
        }
        private void Start()
        {
            if (JCS_PlayerManager.instance != null)
                JCS_PlayerManager.instance.IgnorePhysicsToAllPlayer(mBoxCollider);

            if (JCS_2DGameManager.instance != null)
                JCS_2DGameManager.instance.IgnoreAllPlatformTrigger(mBoxCollider);
        }

        private void FixedUpdate()
        {
            if (!mEffect)
                return;

            this.transform.position += mVelocity * Time.deltaTime;
            mVelocity.y += -JCS_GameConstant.GRAVITY * Time.deltaTime * mItemGravity;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (mVelocity.y > 0)
                return;

            // meet ignore object
            JCS_ItemIgnore jcsii = other.GetComponent<JCS_ItemIgnore>();
            if (jcsii != null)
                return;

            JCS_Item tempItem = this.GetComponent<JCS_Item>();
            // if itself it a item, we check other is a item or not.
            if (tempItem != null)
            {
                tempItem = other.GetComponent<JCS_Item>();
                // if both are item then we dont borther 
                // each other action.
                if (tempItem != null)
                    return;
            }

#if (UNITY_EDITOR)
            // if is debug mode print this out.
            // in order to know what does item touched and 
            // stop this movement.
            if (JCS_GameSettings.instance.DEBUG_MODE)
                JCS_Debug.PrintName(other.transform);
#endif

            mVelocity.y = 0;
            mEffect = false;


            mFixCollider = other;

            // TODO(JenChieh): not all the object we get set are 
            //                 box collider only.
            BoxCollider beSetBox = other.GetComponent<BoxCollider>();

            // set this ontop of the other box(ground)
            if (beSetBox != null)
                JCS_Physics.SetOnTopOfBox(mBoxCollider, beSetBox);


            // enable the physic once on the ground
            JCS_PlayerManager.instance.IgnorePhysicsToAllPlayer(this.mBoxCollider, false);
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// Apply force in order to do hop effect.
        /// </summary>
        /// <param name="depth"> include depth? </param>
        public void DoForce(bool depth = false)
        {
            DoForce(mMoveForce, mJumpForce, depth);
        }
        /// <summary>
        /// Apply force in order to do hop effect.
        /// </summary>
        /// <param name="moveForce"> force to move in x axis </param>
        /// <param name="jumpForce"> force to move in y axis </param>
        /// /// <param name="depth"> include depth? </param>
        public void DoForce(float moveForce, float jumpForce, bool depth = false)
        {
            mVelocity.y = jumpForce;
            mVelocity.x = moveForce;
            this.mMoveForce = moveForce;
            this.mJumpForce = jumpForce;
            mEffect = true;

            // including depth!
            if (depth)
            {
                float tempMoveForce = moveForce / 2;

                // override x
                mVelocity.x = JCS_Utility.JCS_FloatRange(
                    -tempMoveForce,
                    tempMoveForce);

                // apply depth
                mVelocity.z = JCS_Utility.JCS_FloatRange(
                    -tempMoveForce,
                    tempMoveForce);
            }
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
