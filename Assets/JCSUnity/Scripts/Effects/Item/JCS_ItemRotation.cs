/**
 * $File: JCS_ItemRotation.cs $
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

    [RequireComponent(typeof(JCS_OneJump))]
    public class JCS_ItemRotation
        : JCS_Rotation
    {

        //----------------------
        // Public Variables

        //----------------------
        // Private Variables

        // gravity object
        private JCS_OneJump mOneJump = null;

        //----------------------
        // Protected Variables

        //========================================
        //      setter / getter
        //------------------------------

        //========================================
        //      Unity's function
        //------------------------------
        private void Start()
        {
            mOneJump = this.GetComponent<JCS_OneJump>();
            Effect = true;
        }
        protected override void Update()
        {
            base.Update();

            if (mOneJump.GetVelocity().y == 0)
                Stop();
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
