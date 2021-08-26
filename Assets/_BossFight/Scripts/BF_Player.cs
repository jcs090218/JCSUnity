/**
 * $File: BF_Player.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

/// <summary>
/// Main character movement of this game.
/// </summary>
public class BF_Player : JCS_2DSideScrollerPlayer
{
    /* Variables */
    
    [Header("** Optional Variables (BF_Player) **")]
    [SerializeField] private JCS_2DCursorShootAction mCursorShootAction = null;

    private JCS_SequenceShootAction mSequenceShootAction = null;

    [SerializeField] [Range(0, 100)]
    private float mManaCastPerShoot = 1;

    /* Setter & Getter */

    /* Functions */

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        mSequenceShootAction = this.GetComponent<JCS_SequenceShootAction>();
        // set the call back
        if (mSequenceShootAction != null)
            mSequenceShootAction.SetShootCallback(ShootActionFunc);

        mCursorShootAction = this.GetComponent<JCS_2DCursorShootAction>();
        // set the call back
        if (mCursorShootAction != null)
        {
            mCursorShootAction.SetShootCallback(ShootActionFunc);
            mCursorShootAction.SetCheckAbleToShootFunction(CheckAbleToShoot);
        }

        // turn off can damage if is player
        JCS_2DLiveObject obj = this.GetComponent<JCS_2DLiveObject>();
        if (obj != null)
        {
            // this object is player, dont damage him.
            obj.CanDamage = false;
        }

    }

#if (UNITY_EDITOR)
    protected override void Update()
    {
        base.Update();

        Stand();

        if (JCS_Input.GetKeyDown(KeyCode.LeftAlt) ||
            JCS_Input.GetKeyDown(KeyCode.RightAlt))
            Jump();


        if (JCS_Input.GetKey(KeyCode.Space))
            Attack();
        else if (JCS_Input.GetKey(KeyCode.UpArrow))
            ClimbOrTeleport();
        else if (JCS_Input.GetKey(KeyCode.RightArrow))
            MoveRight();
        else if (JCS_Input.GetKey(KeyCode.LeftArrow))
            MoveLeft();
        else if (JCS_Input.GetKey(KeyCode.DownArrow))
            Prone();
    }
#endif

    private void ShootActionFunc()
    {
        DoAnimation(JCS_LiveObjectState.RAND_ATTACK);
        mAudioController.AttackSound();
    }

    private bool CheckAbleToShoot()
    {
        if (BF_GameManager.instance.GAME_IS_OVER)
            return false;

        BF_LiquidBarHandler bfmh = BF_GameManager.instance.MANA_LIQUIDBAR;

        // if is enough mana, will shoot.
        return bfmh.IsAbleToCastCast(JCS_Mathf.ToNegative(mManaCastPerShoot));
    }

}
