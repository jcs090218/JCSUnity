using UnityEngine;
using System.Collections;
using JCSUnity;

public class Player : JCS_2DSideScrollerPlayer
{

    //------------------------------
    // setter / getter
    //------------------------------


    //------------------------------
    // Unity's function
    //------------------------------
    protected override void Update()
    {
        base.Update();

        
        if (JCS_Input.GetKeyDown(KeyCode.LeftAlt) || 
            Input.GetKeyDown(KeyCode.RightAlt))
            Jump();

        if (JCS_Input.GetKey(KeyCode.Space))
            Attack();
        else if (JCS_Input.GetKey(KeyCode.RightArrow))
            MoveRight();
        else if (JCS_Input.GetKey(KeyCode.LeftArrow))
            MoveLeft();
        else
            Stand();

        

    }
}
