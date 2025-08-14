/**
 * $File: RC_Player.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;
using MyBox;

/// <summary>
/// Running Crush game's player.
/// </summary>
public class RC_Player : JCS_2DSideScrollerPlayer
{
    /* Variables */

        // golds
    [Separator("Check Variables (RC_Player)")]

    // player hold his own gold. (for multi-player system.)
    // if is multi-player mode then we threat this as a point.
    [SerializeField] 
    private int mCurrentGold = -1;

    [Separator("Runtime Variables (RC_Player)")]

    [SerializeField] 
    private int mControlIndex = 0;

    // Original speed starting of the game.
    private float mRecordSpeed = 0;
    private float[] mRecordJumpForce = null;

    private RC_PlayerPointer mRCPlayerPointer = null;
    private RC_RevivePointer mRCRevivePointer = null;

    [Tooltip("How fast the speed goes back to original speed.")]
    [SerializeField]
    [Range(JCS_Constants.FRICTION_MIN, 5.0f)]
    private float mSpeedFriction = 0.5f;

    [Tooltip("How fast the speed goes back to original jump force.")]
    [SerializeField]
    [Range(JCS_Constants.FRICTION_MIN, 5.0f)]
    private float mJumpFriction = 0.5f;

    private bool mIsDead = false;

    [SerializeField] 
    private int mLife = 3;

    [Tooltip("How high the player should revive.")]
    [SerializeField] 
    private float mReviveHeight = 50;

    [Tooltip("How many time player need to join the next game.")]
    [SerializeField] 
    private float mReviveTime = 3.0f;

    private float mReviveTimer = 0;

    // every player have a liquid bar.
    private JCS_3DLiquidBar mLiquidBar = null;

    // actions
    private bool mReviving = false;
    private bool mWait = false;

    /* Setter & Getter */

    public void SetRCPlayerPointer(RC_PlayerPointer pp) { mRCPlayerPointer = pp; }
    public void SetRCRevivePointer(RC_RevivePointer rp) { mRCRevivePointer = rp; }
    public bool isDead { get { return mIsDead; } set { mIsDead = value; } }
    public bool reviving { get { return mReviving; } set { mReviving = value; } }
    public int controlIndex { get { return mControlIndex; } set { mControlIndex = value; } }
    public void SetWait(bool act)
    {
        mWait = act;
    }
    public int currentGold { get { return mCurrentGold; } set { mCurrentGold = value; } }
    public void SetLiquidBar(JCS_3DLiquidBar rb) { mLiquidBar = rb; }
    public RC_PlayerPointer GetRCPlayerPointer() { return mRCPlayerPointer; }
    public RC_RevivePointer GetRCRevivePointer() { return mRCRevivePointer; }

    /* Functions */

    protected override void Awake()
    {
        base.Awake();

        mRecordSpeed = mMoveSpeed;

        // record down the jump force
        {
            mRecordJumpForce = new float[mJumpYForces.Length];
            mRecordJumpForce[0] = mJumpYForces[0];

            for (int index = 0; index < mRecordJumpForce.Length; ++index)
            {
                mRecordJumpForce[index] = mJumpYForces[index];
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        // load the gold information from game data.
        mCurrentGold = RC_AppSettings.FirstInstance().APP_DATA.gold;
    }

    protected override void Update()
    {
        // this should always be on top of update function.
        base.Update();

        ProcessInput();

        // when get effect, trying to get back to orignal power
        {
            mMoveSpeed += (mRecordSpeed - mMoveSpeed) / mSpeedFriction * Time.deltaTime;

            for (int index = 0; index < mRecordJumpForce.Length; ++index)
            {
                mJumpYForces[index] += (mRecordJumpForce[index] - mJumpYForces[index]) / mJumpFriction * Time.deltaTime;
            }
        }

#if UNITY_EDITOR
        Test();
#endif

        if (mWait)
            mMoveSpeed = 0;

        WinLose();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        UpdateBar();

        DoIsDead();
    }

#if UNITY_EDITOR
    private void Test()
    {
        if (JCS_Input.GetKey(KeyCode.V))
            Die();
    }
#endif

    public void Block()
    {
        // when it get block minus the speed a bit.
        DeltaSpeed(-10);
    }

    public void DeltaSpeed(float deltaSpeed)
    {
        mMoveSpeed += deltaSpeed;
    }

    public void DeltaJumpForce(float deltaVal, int index)
    {
        mJumpYForces[index] += deltaVal;
    }
    /// <summary>
    /// 挑釁用...
    /// </summary>
    public void ToggleWait()
    {
        // if is climbing return!
        if (characterState == JCS_2DCharacterState.CLIMBING)
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
        mVelocity.y += val;
    }
    public void DeltaPoint(float val)
    {
        mLiquidBar.DeltaCurrentValue(val);
    }

    private void ProcessInput()
    {
        if (RC_GameSettings.FirstInstance().GAME_OVER)
            return;

        // process cross platform input.
        switch (JCS_AppManager.FirstInstance().platformType)
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
            JCS_SceneManager.FirstInstance().LoadScene("RC_LogoScene");

    }
    private void MobileInput()
    {
        // TODO.
    }
    private void DoIsDead()
    {
        // if is game over no need to check.
        if (RC_GameSettings.FirstInstance().GAME_OVER)
            return;

        if (!mIsDead)
        {
            // if out of screen space
            if (!JCS_Camera.main.CheckInScreenSpace(GetCharacterController()))
            {
                // minus a live
                --mLife;

                Die();

                JCS_DynamicScene ds = JCS_SceneManager.FirstInstance().GetDynamicScene();
                JCS_3DShakeEffect se = ds.GetComponent<JCS_3DShakeEffect>();

                // shake once! if player is dead
                if (se != null)
                    se.DoShake();

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
                Vector3 newPos = transform.position;

                // set to the same depth
                newPos.x = JCS_Camera.main.transform.position.x;

                transform.position = newPos;

                velX = mMoveSpeed;

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
        Vector3 newPos = transform.position;
        newPos.y = mReviveHeight;
        newPos.x = JCS_Camera.main.transform.position.x;
        transform.position = newPos;
    }
    private void WinLose()
    {
        if (!RC_GameSettings.FirstInstance().GAME_OVER)
            return;

        // the winning player does not need to prone.
        if (RC_Camera.instance.GetTrackingPlayer().transform == transform)
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

        Vector3 newPos = transform.position;
        newPos += RC_GameSettings.FirstInstance().LIQUIDBAR_OFFSET;
        mLiquidBar.transform.position = newPos;
    }
}
