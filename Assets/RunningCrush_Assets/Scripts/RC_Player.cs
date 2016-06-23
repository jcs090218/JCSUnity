/**
 * $File: RC_Player.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information $
 *		                Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


public class RC_Player : JCS_2DSideScrollerPlayer
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables
    [Header("** RC_Player Settings **")]
    [SerializeField] private int mControlIndex = 0;

    // Original speed starting of the game.
    private float mRecordSpeed = 0;
    private float[] mRecordJumpForce = null;

    private RC_PlayerPointer mRCPlayerPointer = null;
    private RC_RevivePointer mRCRevivePointer = null;

    [Tooltip("How fast the speed goes back to original speed.")]
    [SerializeField] private float mSpeedFriction = 0.5f;
    [Tooltip("How fast the speed goes back to original jump force.")]
    [SerializeField] private float mJumpFriction = 0.5f;

    private bool mIsDead = false;
    [SerializeField] private int mLife = 3;

    [Tooltip("How high the player should revive.")]
    [SerializeField] private float mReviveHeight = 50;
    [Tooltip("How many time player need to join the next game.")]
    [SerializeField] private float mReviveTime = 3.0f;
    private float mReviveTimer = 0;

    // actions
    private bool mReviving = false;
    private bool mWait = false;

    // golds
    [Header("** Check Variables **")]
    // player hold his own gold. (for multi-player system.)
    // if is multi-player mode then we threat this as a point.
    [SerializeField] private int mCurrentGold = -1;

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------
    public void SetRCPlayerPointer(RC_PlayerPointer pp) { this.mRCPlayerPointer = pp; }
    public void SetRCRevivePointer(RC_RevivePointer rp) { this.mRCRevivePointer = rp; }
    public bool IsDead { get { return this.mIsDead; } set { this.mIsDead = value; } }
    public bool Reviving { get { return this.mReviving; } set { this.mReviving = value; } }
    public int ControlIndex { get { return this.mControlIndex; } set { this.mControlIndex = value; } }
    public void SetWait(bool act)
    {
        this.mWait = act;
    }
    public int CurrentGold { get { return this.mCurrentGold; } set { this.mCurrentGold = value; } }

    //========================================
    //      Unity's function
    //------------------------------
    protected override void Awake()
    {
        base.Awake();

        this.mRecordSpeed = MoveSpeed;

        // record down the jump force
        {
            mRecordJumpForce = new float[mJumpYForces.Length];
            this.mRecordJumpForce[0] = mJumpYForces[0];
            for (int index = 0;
                index < mRecordJumpForce.Length;
                ++index)
            {
                this.mRecordJumpForce[index] = mJumpYForces[index];
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        // load the gold information from game data.
        mCurrentGold = RC_GameSettings.RC_GAME_DATA.Gold;
    }

    protected override void Update() 
    {
        // this should always be on top of update function.
        base.Update();

        ProcessInput();

        // when get effect, trying to get back to orignal power
        {
            this.MoveSpeed += (this.mRecordSpeed - this.MoveSpeed) / mSpeedFriction * Time.deltaTime;

            for (int index = 0;
                index < mRecordJumpForce.Length;
                ++index)
            {
                mJumpYForces[index] += (this.mRecordJumpForce[index] - mJumpYForces[index]) / mJumpFriction * Time.deltaTime;
            }
        }


        if (mWait)
            MoveSpeed = 0;
    }

    private void LateUpdate()
    {
        DoIsDead();

        // this should be the last check.
        WinLose();
    }

   
    
    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions

    public void Block()
    {
        // when it get block minus the speed a bit.
        DeltaSpeed(-10);
    }

    public void DeltaSpeed(float deltaSpeed)
    {
        this.MoveSpeed += deltaSpeed;
    }

    public void DeltaJumpForce(float deltaVal, int index)
    {
        this.mJumpYForces[index] += deltaVal;
    }
    /// <summary>
    /// 挑釁用...
    /// </summary>
    public void ToggleWait()
    {
        // if is climbing return!
        if (CharacterMode == JCS_2DCharacterMode.CLIMBING)
            return;

        mWait = !mWait;

        // Waiting animation trigger!
        if (mWait)
            DoAnimation(JCS_PlayerState.SIT);
    }

    public void Dead()
    {

    }

    public void PushY(float val)
    {
        this.mVelocity.y += val;
    }
    
    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
    private void ProcessInput()
    {
        // if the game is over does not input
        if (RC_GameSettings.instance.GAME_OVER)
            return;

        // process cross platform input.
        switch (JCS_ApplicationManager.instance.PLATFORM_TYPE)
        {
            case JCS_PlatformType.PC:
                PCInput();
                break;
            case JCS_PlatformType.MOBILE:
                MobileInput();
                break;
        }
    }
    private void PCInput()
    {

        switch (mControlIndex)
        {
            // player 1
            case 0:
                {
                    if (JCS_Input.GetKey(KeyCode.W))
                        Jump();
                    else
                        MoveRight();

                    if (JCS_Input.GetKeyDown(KeyCode.E))
                        ToggleWait();
                }
                break;
            // player 2
            case 1:
                {
                    if (JCS_Input.GetKey(KeyCode.UpArrow))
                        Jump();
                    else
                        MoveRight();

                    if (JCS_Input.GetKeyDown(KeyCode.RightControl))
                        ToggleWait();
                }
                break;
            // player 3
            case 2:
                {
                    if (JCS_Input.GetKey(KeyCode.I))
                        Jump();
                    else
                        MoveRight();

                    if (JCS_Input.GetKeyDown(KeyCode.O))
                        ToggleWait();
                }
                break;
            // player 4
            case 3:
                {
                    if (JCS_Input.GetKey(KeyCode.Keypad8))
                        Jump();
                    else
                        MoveRight();

                    if (JCS_Input.GetKeyDown(KeyCode.Keypad9))
                        ToggleWait();
                }
                break;
        }

        if (JCS_Input.GetKeyDown(KeyCode.K))
            JCS_SceneManager.instance.LoadScene("RC_LogoScene");

    }
    private void MobileInput()
    {

    }
    private void DoIsDead()
    {
        // if is game over no need to check.
        if (RC_GameSettings.instance.GAME_OVER)
            return;


        if (!mIsDead)
        {
            // if out of screen space
            if (!JCS_Camera.main.CheckInScreenSpace(GetCharacterController()))
            {
                // minus a live
                --mLife;

                mSpriteRenderer.enabled = false;

                JCS_DynamicScene jcsds = JCS_SceneManager.instance.GetDynamicScene();
                JCS_2DShakeEffect jcsse = jcsds.GetComponent<JCS_2DShakeEffect>();

                // shake once! if player is dead
                if (jcsse != null)
                    jcsse.DoShake();

                // when player is dead must be behind, 
                // so auto set to run if the player were being stupid
                // before he dies.
                SetWait(false);

                mIsDead = true;
                mReviving = true;
            }
        }
        // do "revive" or "game over".
        else
        {
            if (mReviving)
            {
                mReviveTimer += Time.deltaTime;

                if (mReviveTime < mReviveTimer)
                {
                    // reset timer back to zero.
                    mReviveTimer = 0;

                    Revive();

                    // end revive
                    mReviving = false;
                }
            }
            // if is done revive and still trying to get back to scene.
            else
            {
                // do this only when is following
                Vector3 newPos = this.transform.position;

                // set to the same depth
                newPos.x = JCS_Camera.main.transform.position.x;

                this.transform.position = newPos;

                VelX = MoveSpeed;


                // check if the player in the scene or not.
                if (JCS_Camera.main.CheckInScreenSpace(GetCharacterController()))
                {
                    // back to game!
                    mIsDead = false;
                }

            }


        }

    }
    private void Revive()
    {
        Vector3 newPos = this.transform.position;
        newPos.y = mReviveHeight;
        newPos.x = JCS_Camera.main.transform.position.x;
        this.transform.position = newPos;

        mSpriteRenderer.enabled = true;
    }
    private void WinLose()
    {
        if (!RC_GameSettings.instance.GAME_OVER)
            return;

        // the winning player does not need to prone.
        if (RC_Camera.instance.GetTrackingPlayer().transform == this.transform)
        {
            // Do Win animation
            DoAnimation(JCS_PlayerState.ALERT);
        }
        else
        {
            // Do lose animation
            DoAnimation(JCS_PlayerState.PRONE);
        }
        

        // dont move.
        mVelocity.x = 0;
    }

}
