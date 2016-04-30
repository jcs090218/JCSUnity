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
    public class JCS_2DSideScrollerPlayer 
        : JCS_Player
    {
        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        //-- Jumping
        [SerializeField] private JCS_JumpeType mJumpType = JCS_JumpeType.BASIC_JUMP;
        private bool mDoubleJump = false;
        private bool mTripleJump = false;
        [SerializeField] private float[] mJumpYForces = null;
        [SerializeField] private float[] mJumpXForces = null;
        private bool mSlitMoveInAir = false;
        [SerializeField] private float mTerminalSpeed = 10;
        [SerializeField] private bool[] mForceXAfterJump = null;

        //-- Animator Control
        private JCS_Animator mJCSAnimator = null;

        //-- Audio Control
        private JCS_2DSideScrollerPlayerAudioController mAudioController = null;


        //----------------------
        // Protected Variables
        //-- Facing
        private JCS_2DFaceType mFace = JCS_2DFaceType.FACE_LEFT;

        //-- GENERAL

        [SerializeField] private float mInAirPullBackForce = 0.8f;
        private JCS_2DFaceType mDirection = JCS_2DFaceType.FACE_LEFT;
        private bool mInAirIdle = false;
        private bool mInAirLeftPress = false;
        private bool mInAirRightPress = false;
        private bool mPullEffect = false;
        private bool mAttackedInAir = false;


        //========================================
        //      setter / getter
        //------------------------------

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
        }

        protected override void Update()
        {
            // apply gravity
            if (!mCharacterController.isGrounded)
                mVelocity.y -= (JCS_GameConstant.GRAVITY * Time.deltaTime * mPlayerGravity);

            // apply force
            base.Update();

            // Check player's direction
            if (mVelocity.x == 0)
                this.mDirection = JCS_2DFaceType.NONE;
            else if (mVelocity.x < 0)
                this.mDirection = JCS_2DFaceType.FACE_LEFT;
            else if (mVelocity.x > 0)
                this.mDirection = JCS_2DFaceType.FACE_RIGHT;

            if (!isGrounded())
            {
                DoAnimation(JCS_PlayerState.JUMP);

                // apply terminal speed
                if (mVelocity.y < -mTerminalSpeed)
                {
                    mSlitMoveInAir = true;
                    mVelocity.y = -mTerminalSpeed;
                }
                else
                    mSlitMoveInAir = false;
            }
            else
            {
                mInAirIdle = false;
                mInAirRightPress = false;
                mInAirLeftPress = false;
                mPullEffect = false;
                mAttackedInAir = false;
                DoAnimation(JCS_PlayerState.STAND);

                if (isInAttackStage())
                    mVelocity.x = 0;
            }
        }

        //========================================
        //      Self-Define
        //------------------------------
        //----------------------
        // Public Functions
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
            if (!mIsControllable || !isGrounded())
                return false;

            if (!DoAnimation(JCS_PlayerState.JUMP))
                return false;

            this.VelY = force;
            if (mForceXAfterJump[0])
                this.VelX = (mJumpXForces[0] * -(int)mFace);
            mDoubleJump = false;
            mTripleJump = false;

            mAudioController.BasicJumpSound();

            return true;
        }
        public bool DoubleJump(float firstForce = 10, float secodForce = 10)
        {
            if (!mIsControllable)
                return false;

            if (mAttackedInAir)
                return false;

            // Jump the first jump
            bool firstJump = BasicJump(firstForce);

            // check if do the second jump
            if (!firstJump && !mDoubleJump)
            {
                this.VelY = secodForce;
                mDoubleJump = true;
                mAudioController.DoubleJumpSound();

                if (mForceXAfterJump[1])
                    this.VelX = (mJumpXForces[1] * -(int)mFace);

                return true;
            }

            return false;
        }
        public bool TripleJump(float firstForce = 10, float secodForce = 10, float thirdForce = 10)
        {
            if (!mIsControllable)
                return false;

            if (mAttackedInAir)
                return false;

            // get current double jump succeed or not
            bool currentDoubleJump = DoubleJump(firstForce, secodForce);

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
            if (!mIsControllable || isInAttackStage())
                return;

            TurnFace(JCS_2DFaceType.FACE_RIGHT);

            if (!mSlitMoveInAir)
            {
                if (!isGrounded())
                    return;
            }

            DoAnimation(JCS_PlayerState.WALK);

            if (mPullEffect)
                return;

            if (!isGrounded())
                mInAirRightPress = true;

            // On Ground
            if (!mInAirIdle)
            {
                if (mInAirLeftPress)
                {
                    this.mVelocity.x = (-vel) + mInAirPullBackForce;
                    mPullEffect = true;
                }
                else
                    this.mVelocity.x = vel;
            }
            // In Air
            else
            {
                switch (mDirection)
                {
                    case JCS_2DFaceType.NONE:
                    case JCS_2DFaceType.FACE_RIGHT:
                        this.mVelocity.x = vel - mInAirPullBackForce;
                        break;
                    case JCS_2DFaceType.FACE_LEFT:
                        this.mVelocity.x = (-vel) + mInAirPullBackForce;
                        break;
                }
            }

        }
        public void MoveLeft(float vel)
        {
            if (!mIsControllable || isInAttackStage())
                return;

            TurnFace(JCS_2DFaceType.FACE_LEFT);

            if (!mSlitMoveInAir)
            {
                if (!isGrounded())
                    return;
            }

            DoAnimation(JCS_PlayerState.WALK);

            if (mPullEffect)
                return;

            if (!isGrounded())
                mInAirLeftPress = true;

            // In Air but without idle chance
            if (!mInAirIdle)
            {
                if (mInAirRightPress)
                {
                    this.mVelocity.x = vel - mInAirPullBackForce;
                    mPullEffect = true;
                }
                else
                    this.mVelocity.x = -vel;
            }
            // In Air with idle active
            else
            {
                switch (mDirection)
                {
                    case JCS_2DFaceType.NONE:
                    case JCS_2DFaceType.FACE_LEFT:
                        this.mVelocity.x = (-vel) + mInAirPullBackForce;
                        break;
                    case JCS_2DFaceType.FACE_RIGHT:
                        this.mVelocity.x = vel - mInAirPullBackForce;
                        break;
                }
            }
        }
        public void Stand()
        {
            if (isInAttackStage())
                return;

            // In Air
            if (!isGrounded())
            {
                mInAirIdle = true;
                return;
            }

            DoAnimation(JCS_PlayerState.STAND);
            this.mVelocity.x = 0;
        }
        public virtual void Attack()
        {
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
            DoAnimation(JCS_PlayerState.PRONE);
        }

        public virtual void Ladder()
        {
            DoAnimation(JCS_PlayerState.LADDER);
        }

        public virtual void Rope()
        {
            DoAnimation(JCS_PlayerState.ROPE);
        }

        //----------------------
        // Protected Functions

        //----------------------
        // Private Functions
        private void TurnFace(int facing)
        {
            TurnFace((JCS_2DFaceType)facing);
        }
        private void TurnFace(JCS_2DFaceType type)
        {
            Vector3 originalScale = this.gameObject.transform.localScale;
            float absoluteOriginalScale = JCS_Mathf.AbsoluteValue(originalScale.x);
            this.gameObject.transform.localScale = new Vector3((int)type * absoluteOriginalScale, originalScale.y, originalScale.z);

            mFace = type;
        }
        private bool DoAnimation(JCS_PlayerState state)
        {
            JCS_PlayerState lastState = GetJCSAnimator().GetCurrentAnimationState();

            if (!GetJCSAnimator().GetEndAttackStage())
                return false;

            if (state != JCS_PlayerState.ATTACK)
            {
                if (lastState == JCS_PlayerState.JUMP && !isGrounded())
                    return false;
            }

            mJCSAnimator.DoAnimation(state);

            return true;
        }
        private bool isInAttackStage()
        {
            JCS_PlayerState lastState = GetJCSAnimator().GetCurrentAnimationState();
            if (lastState == JCS_PlayerState.ATTACK)
                return true;

            return false;
        }

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

        
    }
}
