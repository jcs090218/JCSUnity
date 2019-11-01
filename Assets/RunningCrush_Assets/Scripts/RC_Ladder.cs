/**
 * $File: RC_Ladder.cs $
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
/// 
/// </summary>
public class RC_Ladder 
    : JCS_2DLadder
{

    //----------------------
    // Public Variables

    //----------------------
    // Private Variables
    [Header("** Runtime Variables (RC_Ladder) **")]
    [SerializeField]
    private JCS_ClimbMoveType mAutoClimbDirection = JCS_ClimbMoveType.MOVE_UP;

    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------

    //========================================
    //      Unity's function
    //------------------------------
    private void OnTriggerStay(Collider other)
    {
        JCS_2DSideScrollerPlayer p = other.GetComponent<JCS_2DSideScrollerPlayer>();
        if (p == null)
            return;

        bool isTopOfBox = JCS_Physics.TopOfBox(
                        p.GetCharacterController(),
                        mPositionPlatform.GetPlatformCollider());

        if (isTopOfBox)
        {
            p.AutoClimb = false;
            return;
        }

        p.AutoClimb = true;
        p.AutoClimbDirection = mAutoClimbDirection;


        // auto climb
        switch (mAutoClimbDirection)
        {
            case JCS_ClimbMoveType.MOVE_DOWN:
                p.Prone();
                break;
            case JCS_ClimbMoveType.MOVE_UP:
                p.ClimbOrTeleport();
                break;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        JCS_2DSideScrollerPlayer p = other.GetComponent<JCS_2DSideScrollerPlayer>();
        if (p == null)
            return;

        p.AutoClimb = false;
    }

    //========================================
    //      Self-Define
    //------------------------------
    //----------------------
    // Public Functions

    //----------------------
    // Protected Functions

    //----------------------
    // Private Functions

}
