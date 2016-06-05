/**
 * $File: JCS_2DSideScrollerPlayer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{

    // Character controll of player
    [RequireComponent(typeof(JCS_Animator))]
    [RequireComponent(typeof(JCS_2DSideScrollerPlayerAudioController))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class JCS_2DSideScrollerPlayer
        : JCS_Player
    {
        //----------------------
        // Public Variables


        //----------------------
        // Private Variables

        //-- Action (Ladder, Rope)
        [Header("** Check Variables **")]
        [SerializeField] private bool mCanLadder = false;
        [SerializeField] private bool mCanRope = false;
        private Transform mClimbingTransform = null;
        private bool mExitingClimbing = false;

        protected SpriteRenderer mSpriteRenderer = null;

        [Header("** Runtime Variables **")]
        [SerializeField]
        private JCS_2DCharacterMode mCharacterMode 
            = JCS_2DCharacterMode.NORMAL;

        //-- Jumping
        [Header("Jump Settings")]
        [SerializeField]
        private JCS_JumpeType mJumpType = JCS_JumpeType.BASIC_JUMP;
        private bool mDoubleJump = false;
        private bool mTripleJump = false;
        private int mJumpCount = 0;

        [Tooltip("Force apply when it jump. (Horizontal)")]
        [SerializeField] private float[] mJumpYForces = null;
        [Tooltip("Force apply when it jump. (Vertical)")]
        [SerializeField] private float[] mJumpXForces = null;

        [SerializeField] private float mTerminalSpeed = 10;
        [SerializeField] private bool[] mForceXAfterJump = null;

        //-- Animator Control
        [Header("** Animation Settings **")]
        private JCS_Animator mJCSAnimator = null;
        [Tooltip("Animation display when it jump event occurs.")]
        [SerializeField]
        private RuntimeAnimatorController[] mJumpAnim = null;
        [SerializeField]
        private Vector3[] mJumpAnimOffset = null;
        private int mOrderLayer = 15;

        //-- Audio Control
        private JCS_2DSideScrollerPlayerAudioController mAudioController = null;

        // once we call Physics.IgnoreCollision will call
        // OnTriggerEnter and OnTriggerExit once, 
        // to prevent this happen trigger this for checking.
        private bool mResetingCollision = false;

        //----------------------
        // Protected Variables
        //-- Facing
        private JCS_2DFaceType mFace = JCS_2DFaceType.FACE_LEFT;

        //-- GENERAL
        [Header("** Other Settings **")]
        private bool mAttackedInAir = false;
        [SerializeField] private float mAirFriction = 0.5f;


        //========================================
        //      setter / getter
        //------------------------------
        public SpriteRenderer GetSpriteRenderer() { return this.mSpriteRenderer; }
        public bool ResetingCollision { get { return this.mResetingCollision; } set { this.mResetingCollision = value; } }
        public bool ExitingClimbing { get { return this.mExitingClimbing; } set { this.mExitingClimbing = value; } }
        public void SetClimbingTransform(Transform trans) { this.mClimbingTransform = trans; }
        public Transform GetClimbingTransform() { return this.mClimbingTransform; }
        public JCS_2DCharacterMode CharacterMode { get { return this.mCharacterMode; } set { this.mCharacterMode = value; } }
        public bool CanLadder { get { return this.mCanLadder; } set { this.mCanLadder = value; } }
        public bool CanRope { get { return this.mCanRope; } set { this.mCanRope = value; } }
        public void SetJumpType(JCS_JumpeType type) { this.mJumpType = type; }
        public JCS_JumpeType GetJumpType() { return this.mJumpType; }
        private JCS_Animator GetJCSAnimator() { return this.mJCSAnimator; }
        public Animator GetAnimator() { return this.mJCSAnimator.GetAnimator(); }
        public string GetAnimationState() { return this.mJCSAnimator.GetAnimationState(); }
        public bool isGrounded() { return this.mCharacterController.isGrounded; }

        //========================================
        //      Unity's function
        //------------------------------
        protected override void Awake()
        {
            base.Awake();
            this.mJCSAnimator = this.GetComponent<JCS_Animator>();
            this.mAudioController = this.GetComponent<JCS_2DSideScrollerPlayerAudioController>();
            this.mSpriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        protected override void Start()
        {
            base.Start();

            SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
            mOrderLayer = sr.sortingOrder;
        }

        protected override void Update()
        {
            // apply force
            base.Update();

            switch (mCharacterMode)
            {
                case JCS_2DCharacterMode.NORMAL:
                    {
                        CharacterNormal();
                    }
                    break;
                case JCS_2DCharacterMode.CLIMBING:
                    {
                        CharacterClimbing();
                    }
                    break;
                case JCS_2DCharacterMode.ATTACK:
                    {

                    }
                    break;

            }

        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
        public override void ControlEnable(bool act)
        {
            // enable all the component here
            if (act)
            {
                if (JCS_GameSettings.instance.CAMERA_TYPE != JCS_CameraType.MULTI_TARGET)
                    this.mAudioController.GetAudioListener().enabled = true;
                mIsControllable = true;
            }
            // diable all the component here
            else
            {
                this.mAudioController.GetAudioListener().enabled = false;
                mIsControllable = false;
            }
        }
        public void Jump() { Jump(GetJumpType()); }
        public void Jump(JCS_JumpeType type)
        {
            switch (type)
            {
                case JCS_JumpeType.BASIC_JUMP:
                    BasicJump(mJumpYForces[0]);
                    break;
                case JCS_JumpeType.DOUBLE_JUMP:
                    DoubleJump(mJumpYForces[0], mJumpYForces[1]);
                    break;
                case JCS_JumpeType.TRIPLE_JUMP:
                    TripleJump(mJumpYForces[0], mJumpYForces[1], mJumpYForces[2]);
                    break;
                case JCS_JumpeType.DOUBLE_JUMP_FORCE:

                    break;
                case JCS_JumpeType.TRIPLE_JUMP_FORCE:

                    break;
            }
        }
        public bool BasicJump(float force = 10)
        {
            if (CharacterMode == JCS_2DCharacterMode.CLIMBING)
                return false;

            if (mExitingClimbing)
                return false;

            if (!isGrounded())
                return false;

            if (!mIsControllable)
                return false;

            if (isInAttackStage())
                return false;

            this.VelY = force;
            if (mForceXAfterJump[0])
                this.VelX = (mJumpXForces[0] * -(int)mFace);
            mDoubleJump = false;
            mTripleJump = false;

            mAudioController.BasicJumpSound();

            DoJumpAnimEffect(0);

            ++mJumpCount;

            return true;
        }
        public bool DoubleJump(float firstForce = 10, float secodForce = 10)
        {
            if (CharacterMode == JCS_2DCharacterMode.CLIMBING)
                return false;

            if (mExitingClimbing)
                return false;

            if (!mIsControllable)
                return false;

            if (mAttackedInAir)
                return false;

            if (isInAttackStage())
                return false;

            // Jump the first jump
            bool firstJump = BasicJump(firstForce);

            if (mJumpCount == 0 && !isGrounded())
                mDoubleJump = false;

            // check if do the second jump
            if (!firstJump && !mDoubleJump)
            {
                this.VelY = secodForce;
                mDoubleJump = true;
                mAudioController.DoubleJumpSound();

                if (mForceXAfterJump[1])
                    this.VelX = (mJumpXForces[1] * -(int)mFace);

                DoJumpAnimEffect(1);

                ++mJumpCount;

                return true;
            }

            return false;
        }
        public bool TripleJump(float firstForce = 10, float secodForce = 10, float thirdForce = 10)
        {
            if (CharacterMode == JCS_2DCharacterMode.CLIMBING)
                return false;

            if (mExitingClimbing)
                return false;

            if (!mIsControllable)
                return false;

            if (mAttackedInAir)
                return false;

            if (isInAttackStage())
                return false;

            // get current double jump succeed or not
            bool currentDoubleJump = DoubleJump(firstForce, secodForce);

            if (mJumpCount == 1 && !isGrounded())
                mTripleJump = false;

            // if current double jump not succeed
            // if double jump succeed 
            if (!currentDoubleJump &&
                !mTripleJump &&
                mDoubleJump)
            {
                this.VelY = thirdForce;
                mTripleJump = true;
                mAudioController.TripleJumpSound();

                if (mForceXAfterJump[2])
                    this.VelX = (mJumpXForces[2] * -(int)mFace);

                DoJumpAnimEffect(2);

                ++mJumpCount;

                return true;
            }

            return false;
        }
        public void ToggleFace()
        {
            if (mFace == JCS_2DFaceType.FACE_LEFT)
                mFace = JCS_2DFaceType.FACE_RIGHT;
            else
                mFace = JCS_2DFaceType.FACE_LEFT;

            TurnFace((int)mFace);
        }

        public void MoveRight()
        {
            MoveRight(MoveSpeed);
        }
        public void MoveLeft()
        {
            MoveLeft(MoveSpeed);
        }
        public void MoveRight(float vel)
        {
            // cannot move during climbing
            if (CharacterMode == JCS_2DCharacterMode.CLIMBING)
                return;

            if (!mIsControllable)
                return;

            if (isGrounded() || mExitingClimbing)
            {
                if (!isInAttackStage())
                    this.mVelocity.x = vel;
                mExitingClimbing = false;
            }
            // in air
            else
            {
                this.mVelocity.x += (0 - this.mVelocity.x) * mAirFriction * Time.deltaTime;
            }


            if (isInAttackStage())
                return;

            TurnFace(JCS_2DFaceType.FACE_RIGHT);
            DoAnimation(JCS_PlayerState.WALK);
        }
        public void MoveLeft(float vel)
        {
            // cannot move during climbing
            if (CharacterMode == JCS_2DCharacterMode.CLIMBING)
                return;

            if (!mIsControllable)
                return;


            if (isGrounded() || mExitingClimbing)
            {
                if (!isInAttackStage())
                    this.mVelocity.x = -vel;
                mExitingClimbing = false;
            }
            // in air
            else
            {
                this.mVelocity.x += (0 - this.mVelocity.x) * mAirFriction * Time.deltaTime;
            }


            if (isInAttackStage())
                return;

            TurnFace(JCS_2DFaceType.FACE_LEFT);
            DoAnimation(JCS_PlayerState.WALK);
        }
        public void Stand()
        {
            if (isInAttackStage())
                return;

            // In Air
            if (!isGrounded())
            {
                return;
            }

            DoAnimation(JCS_PlayerState.STAND);
            this.mVelocity.x = 0;
        }
        public virtual void Attack()
        {
            // cannot attack during climbing
            if (CharacterMode == JCS_2DCharacterMode.CLIMBING)
                return;

            if (!mIsControllable || isInAttackStage())
                return;

            if (isGrounded())
                this.mVelocity.x = 0;
            else
                this.mAttackedInAir = true;

            DoAnimation(JCS_PlayerState.ATTACK);
            mAudioController.AttackSound();
        }
        public virtual void Prone()
        {
            // cannot prone in the air.
            if (!isGrounded())
                return;

            // cannot prone while climbing.
            if (CharacterMode == JCS_2DCharacterMode.CLIMBING)
                return;

            if (CanRope || CanLadder)
            {
                CharacterMode = JCS_2DCharacterMode.CLIMBING;
                return;
            }

            DoAnimation(JCS_PlayerState.PRONE);

            this.mVelocity.x = 0;
        }

        public virtual void Ladder()
        {
            DoAnimation(JCS_PlayerState.LADDER);

            this.mVelocity.x = 0;
        }

        public virtual void Rope()
        {
            DoAnimation(JCS_PlayerState.ROPE);

            this.mVelocity.x = 0;
        }

        public virtual void ClimbOrTeleport()
        {
            if (CanLadder)
                Ladder();
            else if (CanRope)
                Rope();
            else
            {
                // no climbing
                return;
            }

            CharacterMode = JCS_2DCharacterMode.CLIMBING;
        }
        

        //----------------------
        // Protected Functions
        protected void TurnFace(int facing)
        {
            TurnFace((JCS_2DFaceType)facing);
        }
        protected void TurnFace(JCS_2DFaceType type)
        {
            Vector3 originalScale = this.gameObject.transform.localScale;
            float absoluteOriginalScale = JCS_Mathf.AbsoluteValue(originalScale.x);
            this.gameObject.transform.localScale = new Vector3((int)type * absoluteOriginalScale, originalScale.y, originalScale.z);

            mFace = type;
        }
        protected bool isInAttackStage()
        {
            JCS_PlayerState lastState = GetJCSAnimator().GetCurrentAnimationState();
            if (lastState == JCS_PlayerState.ATTACK)
                return true;

            return false;
        }
        protected bool DoAnimation(JCS_PlayerState state)
        {
            JCS_PlayerState lastState = GetJCSAnimator().GetCurrentAnimationState();

            if (!GetJCSAnimator().GetEndAttackStage())
                return false;

            if (state != JCS_PlayerState.ATTACK)
            {
                if (CharacterMode != JCS_2DCharacterMode.CLIMBING)
                    if (lastState == JCS_PlayerState.JUMP && !isGrounded())
                        return false;
            }

            mJCSAnimator.DoAnimation(state);

            return true;
        }

        //----------------------
        // Private Functions
        private void CharacterNormal()
        {
            // apply gravity
            if (!mCharacterController.isGrounded)
                mVelocity.y -= (JCS_GameConstant.GRAVITY * Time.deltaTime * mPlayerGravity);

            if (!isGrounded())
            {
                DoAnimation(JCS_PlayerState.JUMP);

                // apply terminal speed
                if (mVelocity.y < -mTerminalSpeed)
                {
                    mVelocity.y = -mTerminalSpeed;
                }
            }
            else
            {
                mAttackedInAir = false;
                DoAnimation(JCS_PlayerState.STAND);

                mJumpCount = 0;

                // not exiting climbing any more
                mExitingClimbing = false;

                if (isInAttackStage())
                    mVelocity.x = 0;
            }
        }
        private void CharacterClimbing()
        {

            if (CanLadder)
                Ladder();
            else if (CanRope)
                Rope();
            else
            {
                JCS_GameErrors.JcsErrors(
                    "JCS_2DSideScrollerPlayer",
                    -1,
                    "No Climbing action applied...");

                return;
            }


            JCS_ClimbMoveType status = JCS_ClimbMoveType.IDLE;

            // process input
            if (JCS_Input.GetKey(KeyCode.UpArrow))
                status = JCS_ClimbMoveType.MOVE_UP;
            else if (JCS_Input.GetKey(KeyCode.DownArrow))
                status = JCS_ClimbMoveType.MOVE_DOWN;
            else
            {
                status = JCS_ClimbMoveType.IDLE;
                mJCSAnimator.StopAnimationInFrame();
            }

            bool climbing = false;

            // process velocity
            switch (status)
            {
                case JCS_ClimbMoveType.IDLE:
                    this.mVelocity.y = 0;
                    break;
                case JCS_ClimbMoveType.MOVE_UP:
                    this.mVelocity.y = MoveSpeed;
                    climbing = true;
                    break;
                case JCS_ClimbMoveType.MOVE_DOWN:
                    this.mVelocity.y = -MoveSpeed;
                    climbing = true;
                    break;
            }

            if (climbing)
            {
                // let x axis the same as ladder x axis.
                Vector3 newPos = this.transform.position;
                newPos.x = mClimbingTransform.transform.position.x;
                this.transform.position = newPos;

                // start the animation agian
                mJCSAnimator.PlayAnimationInFrame();
            }
            else
            {
                if (JCS_Input.GetKey(KeyCode.LeftAlt) ||
                    JCS_Input.GetKey(KeyCode.RightAlt))
                {

                    if (JCS_Input.GetKey(KeyCode.LeftArrow))
                    {
                        mCharacterMode = JCS_2DCharacterMode.NORMAL;
                        CanLadder = false;
                        CanRope = false;
                        mVelocity.y = 10;
                        mExitingClimbing = true;
                    }

                    if (JCS_Input.GetKey(KeyCode.RightArrow))
                    {
                        mCharacterMode = JCS_2DCharacterMode.NORMAL;
                        CanLadder = false;
                        CanRope = false;
                        mVelocity.y = 10;
                        mExitingClimbing = true;
                    }
                }
            }

            // start the animation when is climbing / exit
            if (mExitingClimbing)
            {
                // start the animation agian
                mJCSAnimator.PlayAnimationInFrame();
            }
        }

        /// <summary>
        /// Spawn a animation for jumping effect.
        /// </summary>
        /// <param name="index"></param>
        private void DoJumpAnimEffect(int index)
        {
            if (mJumpAnim.Length < index)
                return;

            if (mJumpAnim.Length != mJumpAnimOffset.Length)
                return;

            if (mJumpAnim[index] == null)
                return;

            GameObject gameObject = JCS_UsefualFunctions.SpawnAnimateObjectDeathEvent(mJumpAnim[index], mOrderLayer - 1);

            Vector3 newPos = Vector3.zero;
            Vector3 tempOffset = mJumpAnimOffset[index];

            // change the position depends on the scale.
            if (this.transform.localScale.x < 0)
                tempOffset.x = -tempOffset.x;

            // assign new position for the effect.
            newPos = this.transform.localPosition + tempOffset;

            // assign to effect's transform.
            gameObject.transform.localPosition = newPos;
            gameObject.transform.localScale = this.transform.localScale;
        }

    }
}
