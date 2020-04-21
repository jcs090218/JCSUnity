/**
 * $File: FT_LiquidBarObject.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;
using JCSUnity;
using UnityEngine.UI;


public class FT_LiquidBarObject
    : MonoBehaviour 
{
    /* Variables */

    public JCS_GUILiquidBar mBar = null;
    public Vector3 mOffset = Vector3.zero;

    /* Setter/Getter */

    /* Functions */

    private void Awake()
    {

    }

    private void LateUpdate()
    {
        if (mBar != null)
            FollowObject();
    }

    private void FollowObject()
    {
        Camera cam = JCS_Camera.main.GetCamera();

        Vector3 newPos = cam.WorldToViewportPoint(this.transform.position);;
        newPos.z = mBar.GetMask().rectTransform.parent.position.z;
        mBar.GetMask().rectTransform.parent.position = newPos;
    }
}
