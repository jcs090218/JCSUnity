/**
 * $File: JCS_2DSideScrollerPlayer.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using System;

namespace JCSUnity
{

    /// <summary>
    /// Character controll of player
    /// </summary>
    [RequireComponent(typeof(JCS_2DCharacterAnimator))]
    [RequireComponent(typeof(JCS_2DSideScrollerPlayerAudioController))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class JCS_2DSideScrollerPlayer
        : JCS_Player
    {
        //----------------------
        // Public Variables


        //----------------------
        // Private Variables

        protected SpriteRenderer mSpriteRenderer = null;

        //-- Action (Ladder, Rope)
        [Header("** Check Variables (JCS_2DSideScrollerPlayer) **")]
        //-- Facing
        [SerializeField] private JCS_2DFaceType mFace = JCS_2DFaceType.FACE_LEFT;

        //-- Climbing
        [Tooltip("See if there are ladder object so we can climb.")]
        [SerializeField]
        private bool mCanLadder = false;

        [Tooltip("See if there are rope object so we can climb.")]
        [SerializeField]
        private bool mCanRope = false;

        [Tooltip("Object that we can climb on.")]
        [SerializeField]
        private Transform mClimbingTransform = null;

        private bool mExitingClimbing = false;
        private bool mStartClimbing = false;

        [Tooltip("Count of the current jump.")]
        [SerializeField]
        private int mJumpCount = 0;


        [Header("** Runtime Variables (JCS_2DSideScrollerPlayer) **")]

        [Tooltip("Character's status.")]
        [SerializeField]
        private JCS_2DCharacterState mCharacterState
            = JCS_2DCharacterState.NORMAL;

        //-- Jumping
        [Header("** Jump Settings (JCS_2DSideScrollerPlayer) **")]

        [Tooltip("Type that this character can do.")]
        [SerializeField]
        private JCS_JumpeType mJumpType = JCS_JumpeType.BASIC_JUMP;

        private bool mDoubleJump = false;       // trigger to check
        private bool mTripleJump = false;       // trigger to check

        [Tooltip("Force apply when it jump. (Horizontal)")]
        [SerializeField] protected float[] mJumpYForces = null;

        [Tooltip("Force apply when it jump. (Vertical)")]
        [SerializeField] protected float[] mJumpXForces = null;

        [Tooltip("Maxinum speed of free fall object.")]
        [SerializeField] private float mTerminalSpeed = 20;

        [Tooltip("")]
        [SerializeField] private bool[] mForceXAfterJump = null;


        //-- Animator Control
        [Header("** Animation Settings (JCS_2DSideScrollerPlayer) **")]

        [Tooltip("Animation display when it jump event occurs.")]
        [SerializeField]
        private RuntimeAnimatorController[] mJumpAnim = null;

        [Tooltip("Animation sprite's offset for jump effect.")]
        [SerializeField]
        private Vector3[] mJumpAnimOffset = null;

        private JCS_2DCharacterAnimator mCharacterAnimator = null;
        private int mOrderLayer = 15;

        //-- Audio Control
        protected JCS_2DSideScrollerPlayerAudioController mAudioController = null;

        // once we call Physics.IgnoreCollision will call
        // OnTriggerEnter and OnTriggerExit once, 
        // to prevent this happen trigger this for checking.
        private bool mResetingCollision = false;


        //-- GENERAL
        [Header("** Other Settings (JCS_2DSideScrollerPlayer) **")]
        [SerializeField] private float mAirFriction = 0.5f;
        private bool mAttackedInAir = false;

        [Header("** Control Key Settings (JCS_2DSlideScrollerPlayer) **")]
        [SerializeField] private KeyCode mUpKey = KeyCode.UpArrow;
        [SerializeField] private KeyCode mDownKey = KeyCode.DownArrow;
        [SerializeField] private KeyCode mRightKey = KeyCode.RightArrow;
        [SerializeField] private KeyCode mLeftKey = KeyCode.LeftArrow;
        [SerializeField] private KeyCode mJumpKey = KeyCode.LeftAlt;

        [SerializeField] private KeyCode mClimbUpKey = KeyCode.UpArrow;
        [SerializeField] private KeyCode mClimbDownKey = KeyCode.DownArrow;


        [Header("** Climb Settings (JCS_2DSideScrollerPlayer) **")]

        [SerializeField] private bool mAutoClimb = false;

        [SerializeField] private JCS_ClimbMoveType mAutoClimbDirection = JCS_ClimbMoveType.MOVE_UP;

        [Tooltip("Force when u exit the climbing state.")]
        [SerializeField] private float mExitClimbForceY = 10;


        [Header("** Hit Settings (JCS_2DSideScrollerPlayer) **")]

        [Tooltip("Trigger to enable hit effect.")]
        [SerializeField] private bool mHitEffect = true;

        [Tooltip("Velocity when get hit. (Horizontal) ")]
        [SerializeField] private float mHitVelX = 5;

        [Tooltip("Velocity when get hit. (Vertical) ")]
        [SerializeField] private float mHitVelY = 5;

        //----------------------
        // Protected Variables


        //========================================
        //      setter / getter
        //------------------------------
        public bool AutoClimb { get { return this.mAutoClimb; } set { this.mAutoClimb = value; } }
        public JCS_ClimbMoveType AutoClimbDirection { get { return this.mAutoClimbDirection; } set { this.mAutoClimbDirection = value; } }
        public SpriteRenderer GetSpriteRenderer() { return this.mSpriteRenderer; }
        public bool ResetingCollision { get { return this.mResetingCollision; } set { this.mResetingCollision = value; } }
        public bool ExitingClimbing { get { return this.mExitingClimbing; } set { this.mExitingClimbing = value; } }
        public void SetClimbingTransform(Transform trans) { this.mClimbingTransform = trans; }
        public Transform GetClimbingTransform() { return this.mClimbingTransform; }
        public JCS_2DCharacterState CharacterState { get { return this.mCharacterState; } set { this.mCharacterState = value; } }
        public bool CanLadder { get { return this.mCanLadder; } set { this.mCanLadder = value; } }
        public bool CanRope { get { return this.mCanRope; } set { this.mCanRope = value; } }
        public void SetJumpType(JCS_JumpeType type) { this.mJumpType = type; }
        public JCS_JumpeType GetJumpType() { return this.mJumpType; }
        public JCS_2DCharacterAnimator GetCharacterAnimator() { return this.mCharacterAnimator; }
        public Animator GetAnimator() { return this.mCharacterAnimator.GetAnimator(); }
        public string GetAnimationState() { return this.mCharacterAnimator.GetAnimationState(); }
        public bool isGrounded() { return this.mCharacterController.isGrounded; }
        public JCS_2DSideScrollerPlayerAudioController GetAudioController() { return this.mAudioController; }
        public int JumpCount { get { return this.mJumpCount; } }
        public JCS_2DFaceType Face { get { return this.mFace; } }

        public KeyCode UpKey { get { return this.mUpKey; } set { this.mUpKey = value; } }
        public KeyCode DownKey { get { return this.mDownKey; } set { this.mDownKey = value; } }
        public KeyCode RightKey { get { return this.mRightKey; } set { this.mRightKey = value; } }
        public KeyCode LeftKey { get { return this.mLeftKey; } set { this.mLeftKey = value; } }
        public KeyCode JumpKey { get { return this.mJumpKey; } set { this.mJumpKey = value; } }
        public KeyCode ClimbUpKey { get { return this.mClimbUpKey; } set { this.mClimbUpKey = value; } }
        public KeyCode ClimbDownKey { get { return this.mClimbDownKey; } set { this.mClimbDownKey = value; } }

        //========================================
        //      Unity's function
        //------------------------------
        protected override void Awake()
        {
            base.Awake();

            this.mCharacterAnimator = this.GetComponent<JCS_2DCharacterAnimator>();
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
            base.Update();

            ProcessState();
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
        public void TurnFace(int facing)
        {
            TurnFace((JCS_2DFaceType)facing);
        }
        public void TurnFace(JCS_2DFaceType type)
        {
            Vector3 originalScale = this.gameObject.transform.localScale;
            float absoluteOriginalScale = JCS_Mathf.AbsoluteValue(originalScale.x);
            this.gameObject.transform.localScale = new Vector3((int)type * absoluteOriginalScale, originalScale.y, originalScale.z);

            mFace = type;
        }
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
            if (CharacterState == JCS_2DCharacterState.CLIMBING)
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
            if (CharacterState == JCS_2DCharacterState.CLIMBING)
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
            if (CharacterState == JCS_2DCharacterState.CLIMBING)
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

        /// <summary>
        /// Move right with defualt speed.
        /// </summary>
        public void MoveRight()
        {
            MoveRight(MoveSpeed);
        }
        /// <summary>
        /// move right with pass in speed.
        /// </summary>
        /// <param name="vel"> speed </param>
        public void MoveRight(float vel)
        {
            // cannot move during climbing
            if (CharacterState == JCS_2DCharacterState.CLIMBING)
                return;

            if (!mIsControllable)
                return;

            bool isAttacking = isInAttackStage();

            if (isGrounded() || mExitingClimbing)
            {
                if (!isAttacking)
                    this.mVelocity.x = vel;
                mExitingClimbing = false;
            }
            // in air
            else
            {
                this.mVelocity.x += (0 - this.mVelocity.x) * mAirFriction * Time.deltaTime;
            }

            if (isAttacking)
                return;

            TurnFace(JCS_2DFaceType.FACE_RIGHT);
            DoAnimation(JCS_LiveObjectState.WALK);
        }

        /// <summary>
        /// Move left with defualt speed.
        /// </summary>
        public void MoveLeft()
        {
            MoveLeft(MoveSpeed);
        }
        /// <summary>
        /// Move left with pass in speed.
        /// </summary>
        /// <param name="vel"> speed </param>
        public void MoveLeft(float vel)
        {
            // cannot move during climbing
            if (CharacterState == JCS_2DCharacterState.CLIMBING)
                return;

            if (!mIsControllable)
                return;

            bool isAttacking = isInAttackStage();

            if (isGrounded() || mExitingClimbing)
            {
                if (!isAttacking)
                    this.mVelocity.x = -vel;
                mExitingClimbing = false;
            }
            // in air
            else
            {
                this.mVelocity.x += (0 - this.mVelocity.x) * mAirFriction * Time.deltaTime;
            }

            if (isAttacking)
                return;

            TurnFace(JCS_2DFaceType.FACE_LEFT);
            DoAnimation(JCS_LiveObjectState.WALK);
        }

        public override void Stand()
        {
            if (mStartClimbing)
                return;

            if (isInAttackStage())
                return;

            // In Air
            if (!isGrounded())
                return;

            DoAnimation(JCS_LiveObjectState.STAND);
            this.mVelocity.x = 0;
        }
        public override void Attack()
        {
            // cannot attack during climbing
            if (CharacterState == JCS_2DCharacterState.CLIMBING)
                return;

            if (!mIsControllable || isInAttackStage())
                return;

            if (isGrounded())
                this.mVelocity.x = 0;
            else
                this.mAttackedInAir = true;

            DoAnimation(JCS_LiveObjectState.RAND_ATTACK);
            mAudioController.AttackSound();
        }
        public override void Jump() { Jump(GetJumpType()); }
        public override void Prone()
        {
            // cannot prone in the air.
            if (!isGrounded())
                return;

            if (mStartClimbing)
                return;

            // cannot prone while climbing.
            if (CharacterState == JCS_2DCharacterState.CLIMBING)
                return;

            if (CanRope || CanLadder)
            {
                if (CheckClimbDirection())
                {
                    CharacterState = JCS_2DCharacterState.CLIMBING;
                    return;
                }
            }

            DoAnimation(JCS_LiveObjectState.PRONE);

            this.mVelocity.x = 0;
        }
        public override void Alert()
        {

        }
        public override void Ladder()
        {
            DoAnimation(JCS_LiveObjectState.LADDER);

            this.mVelocity.x = 0;
        }
        public override void Rope()
        {
            DoAnimation(JCS_LiveObjectState.ROPE);

            this.mVelocity.x = 0;
        }
        public override void Die()
        {
            this.mVelocity = Vector3.zero;

            DoAnimation(JCS_LiveObjectState.DIE);
        }
        public override void Dance()
        {
            throw new NotImplementedException();
        }
        public override void Swim()
        {
            DoAnimation(JCS_LiveObjectState.SWIM);
        }
        public override void Fly()
        {
            DoAnimation(JCS_LiveObjectState.FLY);
        }
        public override void Sit()
        {

        }
        /// <summary>
        /// When player get hit.
        /// </summary>
        public override void Hit()
        {
            if (!mHitEffect)
            {
                JCS_GameErrors.JcsErrors(
                    this, 
                    "You call the function without checking the hit effect?");

                return;
            }

            ExitClimbing(0);

            int randDirection = JCS_Utility.JCS_IntRange(0, 2);        // 0 ~ 1

            // if 0 push right, else if 1 push left
            float pushVel = (randDirection == 0) ? mHitVelX : -mHitVelX;
            
            // apply force as velocity
            this.mVelocity.x += pushVel;

            // hop a bit. (velcotiy y axis)
            this.mVelocity.y += mHitVelY;


            DoAnimation(JCS_LiveObjectState.STAND);
        }
        public override void Ghost()
        {

        }

        public virtual void ClimbOrTeleport()
        {
            if (isInAttackStage())
                return;

            // check the direction correctly or not.
            // 這個讓原本就在梯子上面的玩家不再往上爬!
            if (!CheckClimbDirection())
                return;

            // set mode to climbing.
            CharacterState = JCS_2DCharacterState.CLIMBING;
        }

        //----------------------
        // Protected Functions

        /// <summary>
        /// Check if we are still in the attack stage.
        /// </summary>
        /// <returns>
        /// true: in attack stage.
        /// false: not in attack stage.
        /// </returns>
        protected bool isInAttackStage()
        {
            JCS_LiveObjectState lastState = GetCharacterAnimator().GetCurrentAnimationState();
            if (lastState == JCS_LiveObjectState.RAND_ATTACK && 
                !mCharacterAnimator.GetEndAttackStage())
                return true;

            return false;
        }

        /// <summary>
        /// Do the animation by passin the state.
        /// </summary>
        /// <param name="state"> state </param>
        /// <returns>
        /// true: play animation success.
        /// false: failed to play animation.
        /// </returns>
        protected bool DoAnimation(JCS_LiveObjectState state)
        {
            if (!GetCharacterAnimator().GetEndAttackStage())
                return false;

            if (state != JCS_LiveObjectState.RAND_ATTACK)
            {
                if (CharacterState != JCS_2DCharacterState.CLIMBING)
                {
                    JCS_LiveObjectState lastState = GetCharacterAnimator().GetCurrentAnimationState();

                    if (lastState == JCS_LiveObjectState.JUMP && !isGrounded())
                        return false;
                }
            }

            mCharacterAnimator.DoAnimation(state);
            DoSound(state);

            return true;
        }

        /// <summary>
        /// Do the sound base on the state we are in.
        /// </summary>
        /// <param name="state"> state we are in. </param>
        protected void DoSound(JCS_LiveObjectState state)
        {
            GetAudioController().PlaySoundByPlayerState(state);
        }


        //----------------------
        // Private Functions

        /// <summary>
        /// Process State Pattern.
        /// </summary>
        private void ProcessState()
        {
            switch (mCharacterState)
            {
                case JCS_2DCharacterState.NORMAL:
                    {
                        CharacterNormal();
                    }
                    break;
                case JCS_2DCharacterState.CLIMBING:
                    {
                        CharacterClimbing();
                    }
                    break;
                case JCS_2DCharacterState.ATTACK:
                    {

                    }
                    break;
            }
        }

        /// <summary>
        /// While Character is standing.
        /// </summary>
        private void CharacterNormal()
        {
            // if is in the AIR
            if (!isGrounded())
            {
                // apply gravity
                mVelocity.y -= (JCS_GameConstant.GRAVITY * 
                    Time.deltaTime * 
                    JCS_GameSettings.instance.GRAVITY_PRODUCT);

                DoAnimation(JCS_LiveObjectState.JUMP);

                // apply terminal speed
                if (mVelocity.y < -mTerminalSpeed)
                {
                    mVelocity.y = -mTerminalSpeed;
                }
            }
            // if is on the ground
            else
            {

                this.mVelocity.y = -1;

                Stand();
                mAttackedInAir = false;

                mJumpCount = 0;

                // if on the ground and is in attack stage, 
                // dont move the character in x-axis.
                if (isInAttackStage())
                    mVelocity.x = 0;
            }

            mStartClimbing = false;
        }

        /// <summary>
        /// While character are climbing.
        /// </summary>
        private void CharacterClimbing()
        {
            if (!mStartClimbing)
            {
                if (!mAutoClimb)
                {
                    if (!mCharacterAnimator.IsInState(JCS_LiveObjectState.STAND) &&
                        !mCharacterAnimator.IsInState(JCS_LiveObjectState.JUMP) &&
                        !mCharacterAnimator.IsInState(JCS_LiveObjectState.WALK))
                    {
                        ExitClimbing(0);
                        return;
                    }
                }

                if (mClimbingTransform == null)
                {
                    ExitClimbing(0);
                    return;
                }

                mStartClimbing = true;
            }

            if (CanLadder)
                Ladder();
            else if (CanRope)
                Rope();
            else
            {
                JCS_GameErrors.JcsErrors(
                    this,
                    "No Climbing action applied...");
                ExitClimbing(0);

                return;
            }

            // while climbing zero jump count.
            mJumpCount = 0;

            JCS_ClimbMoveType status = JCS_ClimbMoveType.IDLE;

            if (mAutoClimb)
            {
                status = mAutoClimbDirection;
            }
            else
            {
                // process input
                if (JCS_Input.GetKey(this.ClimbUpKey))
                    status = JCS_ClimbMoveType.MOVE_UP;
                else if (JCS_Input.GetKey(this.mClimbDownKey))
                    status = JCS_ClimbMoveType.MOVE_DOWN;
                else
                {
                    status = JCS_ClimbMoveType.IDLE;
                    mCharacterAnimator.StopAnimationInFrame();
                }
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
                if (mClimbingTransform == null)
                {
                    ExitClimbing(0);
                    return;
                }

                // let x axis the same as ladder x axis.
                Vector3 newPos = this.transform.position;
                newPos.x = mClimbingTransform.transform.position.x;
                this.transform.position = newPos;

                // start the animation agian
                mCharacterAnimator.PlayAnimationInFrame();
            }
            else
            {
                if (JCS_Input.GetKey(this.JumpKey) ||
                    JCS_Input.GetKey(this.JumpKey))
                {

                    if (JCS_Input.GetKey(this.LeftKey) || 
                        JCS_Input.GetKey(this.RightKey))
                    {
                        ExitClimbing();
                        mExitingClimbing = true;
                    }
                }
            }

            if (isGrounded())
                ExitClimbing(0);
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

            if (mJumpAnim.Length == 0)
                return;

            if (mJumpAnim[index] == null)
                return;

            GameObject gameObject = JCS_Utility.SpawnAnimateObjectDeathEvent(mJumpAnim[index], mOrderLayer - 1);

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

        /// <summary>
        /// Call this when exit climbing status.
        /// </summary>
        private void ExitClimbing()
        {
            ExitClimbing(mExitClimbForceY);
        }
        /// <summary>
        /// Call this when exit climbing status.
        /// </summary>
        /// <param name="jumpForce"> force to exit the climbing object. </param>
        private void ExitClimbing(float jumpForce)
        {
            mCharacterState = JCS_2DCharacterState.NORMAL;
            // when exit give a litte jump
            mVelocity.y = jumpForce;

            // start the animation agian
            mCharacterAnimator.PlayAnimationInFrame();

            if (jumpForce != 0)
            {
                CanLadder = false;
                CanRope = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckClimbDirection()
        {
            // no climbing object, no climbing!
            if (mClimbingTransform == null)
                return false;

            // when is in air, character can blim what ever
            if (!isGrounded())
                return true;

            JCS_ClimbMoveType keyInput = JCS_ClimbMoveType.IDLE;

            // process input
            if (JCS_Input.GetKey(this.ClimbUpKey))
                keyInput = JCS_ClimbMoveType.MOVE_UP;
            else if (JCS_Input.GetKey(this.mClimbDownKey))
                keyInput = JCS_ClimbMoveType.MOVE_DOWN;


            switch (keyInput)
            {
                case JCS_ClimbMoveType.IDLE: return true;

                case JCS_ClimbMoveType.MOVE_UP:
                    {
                        if (this.transform.position.y < this.mClimbingTransform.position.y)
                            return true;
                    }
                    break;
                case JCS_ClimbMoveType.MOVE_DOWN:
                    {
                        if (this.transform.position.y > this.mClimbingTransform.position.y)
                            return true;
                    }
                    break;
            }

            return false;

        }
       
    }
}
