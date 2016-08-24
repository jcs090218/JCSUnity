/**
 * $File: JCS_Player.cs $
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

    [RequireComponent(typeof(JCS_CharacterControllerInfo))]
    public abstract class JCS_Player 
        : MonoBehaviour
    {
        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        //----------------------
        // Protected Variables

        protected CharacterController mCharacterController = null;
        protected JCS_CharacterControllerInfo mCharacterControllerInfo = null;


        [Header("** Initilaize Variables (JCS_Player) **")]

        [SerializeField] protected Vector3 mVelocity = Vector3.zero;
        [SerializeField] protected float mMoveSpeed = 0.0f;
        
        [SerializeField] protected bool mIsControllable = true;


        //========================================
        //      setter / getter
        //------------------------------
        public bool GetIsControllable() { return this.mIsControllable; }
        public void SetIsControllable(bool act) { this.mIsControllable = act; }
        public Vector3 Velocity { get { return this.mVelocity; } set { this.mVelocity = value; } }
        public float VelX { get { return this.mVelocity.x; } set { this.mVelocity.x = value; } }
        public float VelY { get { return this.mVelocity.y; } set { this.mVelocity.y = value; } }
        public float VelZ { get { return this.mVelocity.z; } set { this.mVelocity.z = value; } }
        public CharacterController GetCharacterController() { return this.mCharacterController; }
        public JCS_CharacterControllerInfo CharacterControllerInfo { get { return this.mCharacterControllerInfo; } }
        public float MoveSpeed { get { return this.mMoveSpeed; } set { this.mMoveSpeed = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        protected virtual void Awake()
        {
            mCharacterController = this.GetComponent<CharacterController>();
            mCharacterControllerInfo = this.GetComponent<JCS_CharacterControllerInfo>();
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
            // empty
        }

        protected virtual void LateUpdate()
        {
            // empty
        }

        protected virtual void FixedUpdate()
        {
            if (mCharacterController.enabled)
            {
                // apply force
                mCharacterController.Move(mVelocity * Time.deltaTime);
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        public abstract void ControlEnable(bool act);

        /// <summary>
        /// 
        /// </summary>
        public abstract void Stand();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Attack();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Jump();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Prone();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Alert();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Fly();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Ladder();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Rope();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Dance();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Swim();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Sit();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Hit();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Die();
        /// <summary>
        /// 
        /// </summary>
        public abstract void Ghost();

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions

    }
}
