/**
 * $File: JCS_DestroyReminder.cs $
 * $Date: $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information 
 *                   Copyright (c) 2016 by Shen, Jen-Chieh $
 */
using UnityEngine;
using System.Collections;

namespace JCSUnity
{

    public class JCS_DestroyReminder
        : MonoBehaviour
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables
        private float mTimer = 0;
        private float mRemindPerTime = 5;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------

        private void Update()
        {
            mTimer += Time.deltaTime;

            if (mRemindPerTime < mTimer)
            {
                JCS_GameErrors.JcsReminders(
                    "JCS_DestroyReminder", 
                      
                    "Object u sound destroy but u did not...");

#if (UNITY_EDITOR)
                gameObject.name = "Object u sound destroy but u did not...";
#endif
            }
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
}
