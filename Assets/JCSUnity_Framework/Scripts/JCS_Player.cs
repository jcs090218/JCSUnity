/**
 * $File: JCS_Player.cs $
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
    [RequireComponent(typeof(CharacterController))]
    public abstract class JCS_Player : MonoBehaviour
    {
        [SerializeField] protected float mPlayerGravity = 2.0f;
        [SerializeField] protected Vector3 mVelocity = Vector3.zero;
        [SerializeField] protected float mMoveSpeed = 0.0f;
        protected CharacterController mCharacterController = null;
        [SerializeField] protected bool mIsControllable = true;


        public bool GetIsControllable() { return this.mIsControllable; }
        public void SetIsControllable(bool act) { this.mIsControllable = act; }
        public Vector3 Veclotiy { get { return this.mVelocity; } set { this.mVelocity = value; } }
        public float VelX { get { return this.mVelocity.x; } set { this.mVelocity.x = value; } }
        public float VelY { get { return this.mVelocity.y; } set { this.mVelocity.y = value; } }
        public float VelZ { get { return this.mVelocity.z; } set { this.mVelocity.z = value; } }
        public CharacterController GetCharacterController() { return this.mCharacterController; }
        public float MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }


        protected virtual void Awake()
        {
            mCharacterController = this.GetComponent<CharacterController>();
        }

        protected virtual void Start()
        {
            // set Execute order lower than "JCS_GameManager"
            JCS_GameManager.instance.SetJCSPlayer(this);

            // Player Manager will take care of all the player
            JCS_PlayerManager.instance.AddPlayerToManage(this);
        }

        protected virtual void Update()
        {
            // apply force
            mCharacterController.Move(mVelocity * Time.deltaTime);
        }

        public abstract void ControlEnable(bool act);

    }
}
