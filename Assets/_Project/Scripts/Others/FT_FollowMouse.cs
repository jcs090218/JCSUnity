/**
 * $File: FT_FollowMouse.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class FT_FollowMouse : MonoBehaviour 
{
    /* Variables */

    /* Setter & Getter */

    /* Functions */

    private void Awake() 
    {
        
    }
    
    private void Update() 
    {
        //this.transform.position = Input.mousePosition;

        Vector2 pos;
        Canvas myCanvas = JCS_Canvas.GuessCanvas().canvas;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = JCS_Input.CanvasMousePosition();
    }
}
