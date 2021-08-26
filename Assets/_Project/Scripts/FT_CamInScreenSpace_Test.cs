/**
 * $File: FT_CamInScreenSpace_Test.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

[RequireComponent(typeof(RectTransform))]
public class FT_CamInScreenSpace_Test : MonoBehaviour 
{
    /* Variables */

    private RectTransform mRectTransform = null;

    /* Setter/Getter */

    /* Functions */

    private void Awake() 
    {
        mRectTransform = this.GetComponent<RectTransform>();

    }
    
    private void Update() 
    {
        print(JCS_Camera.main.CheckInScreenSpace(mRectTransform));
    }
}
