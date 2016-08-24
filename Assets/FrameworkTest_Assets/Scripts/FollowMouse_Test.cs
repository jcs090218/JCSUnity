/**
 * $File: FollowMouse_Test.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;

public class FollowMouse_Test : MonoBehaviour 
{
    
    //----------------------
    // Public Variables
    
    //----------------------
    // Private Variables
    
    //----------------------
    // Protected Variables

    //========================================
    //      setter / getter
    //------------------------------
    
    //========================================
    //      Unity's function
    //------------------------------
    private void Awake() 
    {
        
    }
    
    private void Update() 
    {
        //this.transform.position = Input.mousePosition;

        Vector2 pos;
        Canvas myCanvas = JCS_Canvas.instance.GetCanvas();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = JCS_Input.CanvasMousePosition();

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
