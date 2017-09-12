/**
 * $File: FT_RotatePointTest.cs $
 * $Date: 2017-09-11 19:00:53 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright (c) 2017 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JCSUnity;


/// <summary>
/// 
/// </summary>
public class FT_RotatePointTest 
    : MonoBehaviour 
{

    /*******************************************/
    /*            Public Variables             */
    /*******************************************/

    /*******************************************/
    /*           Private Variables             */
    /*******************************************/
    public Transform mOrigin = null;
    public float mAngle = 10;

    /*******************************************/
    /*           Protected Variables           */
    /*******************************************/

    /*******************************************/
    /*             setter / getter             */
    /*******************************************/

    /*******************************************/
    /*            Unity's function             */
    /*******************************************/
    private void Awake()
    {

    }

    private void Update()
    {
        if (JCS_Input.GetKeyDown(KeyCode.C))
        {
            this.transform.position = JCS_Mathf.RotatePointY(this.transform.position, mOrigin.transform.position, mAngle);
        }
    }
    
    /*******************************************/
    /*              Self-Define                */
    /*******************************************/
    //----------------------
    // Public Functions
    
    //----------------------
    // Protected Functions
    
    //----------------------
    // Private Functions
    
}
