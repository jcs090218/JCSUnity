/**
 * $File: RC_BackToMenuButton.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class RC_BackToMenuButton : JCS_Button
{
    /* Variables */

    private JCS_SlideScreenCamera mSlideCamera = null;

    /* Setter & Getter */

    /* Functions */

    protected override void Awake()
    {
        base.Awake();

        // lazy code
        mSlideCamera = GameObject.Find("JCS_SlideScreenCamera").GetComponent<JCS_SlideScreenCamera>();
    }

    public override void OnClick()
    {
        if (mSlideCamera == null)
        {
            JCS_Debug.LogError("No JCS_SlideScreenCamera in the scene");
            return;
        }

        mSlideCamera.SwitchScene(JCS_2D4Direction.BOTTOM);
    }
}
