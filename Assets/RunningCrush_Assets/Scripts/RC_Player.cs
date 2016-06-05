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

    [Tooltip("How fast the speed goes back to original speed.")]
    [SerializeField] private float mSpeedFriction = 0.5f;

    private bool mIsDead = false;
    [SerializeField] private int mLife = 3;

    [Tooltip("How high the player should revive.")]
    [SerializeField] private float mReviveHeight = 50;
    [Tooltip("How many time player need to join the next game.")]
    [SerializeField] private float mReviveTime = 3.0f;
    private float mReviveTimer = 0;

    private bool mReviving = false;
    

    private bool mWait = false;

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------
    public bool IsDead { get { return this.mIsDead; } set { this.mIsDead = value; } }

    //========================================
    //      Unity's function
    //------------------------------
    protected override void Awake()
    {
        base.Awake();

        this.mRecordSpeed = MoveSpeed;
    }

    protected override void Update() 
    {
        // this should always be on top of update function.
        base.Update();


#if (UNITY_EDITOR)
        Test();
#endif
        

        this.MoveSpeed += (this.mRecordSpeed - this.MoveSpeed) / mSpeedFriction * Time.deltaTime;

        if (mWait)
            MoveSpeed = 0;

        DoIsDead();
    }

    private void Test()
    {
        if (mControlIndex == 0)
        {
            if (JCS_Input.GetKeyDown(KeyCode.RightAlt))
                Jump();
            else
                MoveRight();
        }
        else
        {
            if (JCS_Input.GetKeyDown(KeyCode.LeftAlt))
                Jump();
            else
                MoveRight();
        }

        if (JCS_Input.GetKeyDown(KeyCode.A))
            ToggleWait();
    }
    
    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions

    public void Block()
    {
        // when it get block minus the speed a bit.
        SpeedUp(-10);
    }

    public void SpeedUp(float deltaSpeed)
    {
        this.MoveSpeed += deltaSpeed;
    }

    public void ToggleWait()
    {
        mWait = !mWait;

        // Waiting animation trigger!
        if (mWait)
            DoAnimation(JCS_PlayerState.SIT);
    }

    public void Dead()
    {

    }
    
    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
    private void DoIsDead()
    {

        if (!mIsDead)
        {
            // if out of screen space
            if (!JCS_Camera.main.CheckInScreenSpace(GetCharacterController()))
            {
                // minus a live
                --mLife;

                mSpriteRenderer.enabled = false;

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
                this.transform.position += JCS_Camera.main.Velocity * Time.deltaTime;

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
    
}
