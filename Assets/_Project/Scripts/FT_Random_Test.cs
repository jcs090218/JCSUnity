/**
 * $File: FT_Random_Test.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using JCSUnity;

public class FT_Random_Test : MonoBehaviour 
{
    /* Variables */

    /* Setter/Getter */

    /* Functions */

    private void Awake() 
    {
        
    }
    
    private void Update() 
    {
        if (JCS_Input.GetKeyDown(KeyCode.T))
        {
            print(JCS_TimeManager.GetCurrentTime());
            print(JCS_TimeManager.isAfternoon());
        }
    }
}
