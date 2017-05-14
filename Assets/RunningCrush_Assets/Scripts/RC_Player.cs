/**
 * $File: RC_Player.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;


/// <summary>
/// Running Crush game's player.
/// </summary>
public class RC_Player 
    : JCS_2DSideScrollerPlayer
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables
    [Header("** RC_Player Settings (RC_Player) **")]
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

    // every player have a liquid bar.
    private JCS_3DLiquidBar mLiquidBar = null;

    // actions
    private bool mReviving = false;
    private bool mWait = false;

    // golds
    [Header("** Check Variables (RC_Player) **")]
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
    public void SetLiquidBar(JCS_3DLiquidBar rb) { this.mLiquidBar = rb; }

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

#if (UNITY_EDITOR)
        Test();
#endif

        if (mWait)
            MoveSpeed = 0;

        WinLose();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        UpdateBar();

        DoIsDead();
    }

#if (UNITY_EDITOR)
    private void Test()
    {
        if (JCS_Input.GetKey(KeyCode.V))
            Die();
    }
#endif


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
        if (CharacterState == JCS_2DCharacterState.CLIMBING)
            return;

        mWait = !mWait;

        // Waiting animation trigger!
        if (mWait)
            DoAnimation(JCS_LiveObjectState.SIT);
    }

    public override void Die()
    {
        base.Die();
        mMoveSpeed = 0;

        mIsDead = true;
        mReviving = true;
    }

    public void PushY(float val)
    {
        this.mVelocity.y += val;
    }
    public void DeltaPoint(float val)
    {
        mLiquidBar.DeltaCurrentValue(val);
    }

    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
    private void ProcessInput()
    {
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

                    if (JCS_Input.GetKeyDown(KeyCode.E))
                        ToggleWait();
                }
                break;
            // player 2
            case 1:
                {
                    if (JCS_Input.GetKey(KeyCode.UpArrow))
                        Jump();

                    if (JCS_Input.GetKeyDown(KeyCode.RightControl))
                        ToggleWait();
                }
                break;
            // player 3
            case 2:
                {
                    if (JCS_Input.GetKey(KeyCode.I))
                        Jump();

                    if (JCS_Input.GetKeyDown(KeyCode.O))
                        ToggleWait();
                }
                break;
            // player 4
            case 3:
                {
                    if (JCS_Input.GetKey(KeyCode.Keypad8))
                        Jump();

                    if (JCS_Input.GetKeyDown(KeyCode.Keypad9))
                        ToggleWait();
                }
                break;
        }

        MoveRight();

        if (JCS_Input.GetKeyDown(KeyCode.P))
            JCS_SceneManager.instance.LoadScene("RC_LogoScene");

    }
    private void MobileInput()
    {
        // TODO.
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

                Die();

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
    }
    private void WinLose()
    {
        if (!RC_GameSettings.instance.GAME_OVER)
            return;

        // the winning player does not need to prone.
        if (RC_Camera.instance.GetTrackingPlayer().transform == this.transform)
        {
            // Do Win animation
            DoAnimation(JCS_LiveObjectState.STAND);
        }
        else
        {
            // Do lose animation
            DoAnimation(JCS_LiveObjectState.PRONE);
        }
        

        // dont move.
        mVelocity.x = 0;
    }
    private void UpdateBar()
    {
        if (mLiquidBar == null)
            return;

        Vector3 newPos = this.transform.position;
        newPos += RC_GameSettings.instance.LIQUIDBAR_OFFSET;
        mLiquidBar.transform.position = newPos;
    }

}
