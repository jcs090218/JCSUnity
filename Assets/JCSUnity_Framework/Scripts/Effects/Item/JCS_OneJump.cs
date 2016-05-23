/**
 * $File: JCS_OneJump.cs $
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
        [SerializeField] private float mJumpForce = 10;
        [SerializeField] private float mMoveForce = 10;
        [SerializeField] private float mItemGravity = 2;

        private Vector3 mVelocity = Vector3.zero;
        private BoxCollider mBoxCollider = null;
        

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------
        public bool Effect { get { return this.mEffect; } set { this.mEffect = value; } }
        public Vector3 GetVelocity() { return this.mVelocity; }

        //========================================
        //      Unity's function
        //------------------------------
        private void Awake()
        {
            mBoxCollider = this.GetComponent<BoxCollider>();
        }
        private void Start()
        {
            JCS_PlayerManager.instance.IgnorePhysicsToAllPlayer(mBoxCollider);
            JCS_2DGameManager.instance.IgnoreAllPlatformTrigger(mBoxCollider);
        }

        private void Update()
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

            mVelocity.y = 0;
            mEffect = false;

            // enable the physic once on the ground
            JCS_PlayerManager.instance.EnablePhysicsToAllPlayer(this.mBoxCollider);
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public void DoForce()
        {
            DoForce(mMoveForce, mJumpForce);
        }
        public void DoForce(float moveForce, float jumpForce)
        {
            mVelocity.y = jumpForce;
            mVelocity.x = moveForce;
            this.mMoveForce = moveForce;
            this.mJumpForce = jumpForce;
            mEffect = true;
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
