/**
 * $File: FT_InputField_Test.cs $
 * $Date: 2018-08-28 01:49:02 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Test of the input field.
/// </summary>
[RequireComponent(typeof(InputField))]
public class FT_InputField_Test 
    : MonoBehaviour 
{

    /*******************************************/
    /*            Public Variables             */
    /*******************************************/

    public InputField inputField = null;

    /*******************************************/
    /*           Private Variables             */
    /*******************************************/

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
        this.inputField = this.GetComponent<InputField>();

        inputField.text = "Hello World";
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
