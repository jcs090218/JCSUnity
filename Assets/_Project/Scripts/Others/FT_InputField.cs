/**
 * $File: FT_InputField.cs $
 * $Date: 2018-08-28 01:49:02 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *	                 Copyright © 2018 by Shen, Jen-Chieh $
 */
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Test of the input field.
/// </summary>
[RequireComponent(typeof(InputField))]
public class FT_InputField : MonoBehaviour
{
    /* Variables */

    public InputField inputField = null;

    /* Setter/Getter */

    /* Functions */

    private void Awake()
    {
        this.inputField = this.GetComponent<InputField>();

        inputField.text = "Hello World";
    }
}
