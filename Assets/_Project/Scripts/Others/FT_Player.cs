/**
 * $File: FT_Player.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class FT_Player : JCS_2DSideScrollerPlayer
{
    /* Variables */

    /* Setter/Getter */

    /* Functions */

    protected override void Update()
    {
        base.Update();

        if (JCS_Input.GetKeyDown(KeyCode.LeftAlt) ||
            JCS_Input.GetKeyDown(KeyCode.RightAlt))
            Jump();

        if (JCS_Input.GetKey(KeyCode.UpArrow))
            ClimbOrTeleport();

        if (JCS_Input.GetKey(KeyCode.Space))
            Attack();
        else if (JCS_Input.GetKey(KeyCode.RightArrow))
            MoveRight();
        else if (JCS_Input.GetKey(KeyCode.LeftArrow))
            MoveLeft();
        else if (JCS_Input.GetKey(KeyCode.DownArrow))
            Prone();

        if (JCS_Input.GetKeyDown(KeyCode.H))
        {
            Hit();
        }
    }
}
